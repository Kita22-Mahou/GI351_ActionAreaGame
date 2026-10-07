using UnityEngine;
using UnityEngine.AI;

public class Poko : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject characterSprite;

    private NavMeshAgent navMeshAgent;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float stopDistance = 0.05f;

    [Header("Follow")]
    [SerializeField] private bool followMode;
    [SerializeField] private float followSmooth = 8f;

    private Vector2 followTarget;

    [Header("Attack")]
    [SerializeField] private bool attackMonsterMode;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer;

    [Header("Skill 1 - Area Attack")]
    [SerializeField] private float skillRadius = 2f;
    [SerializeField] private float skillDamage = 40f;
    [SerializeField] private float skillCooldown = 5f;

    private float skillTimer;

    [Header("Skill 2 - Boom Shroom")]
    [SerializeField] private GameObject boomShroomPrefab;
    [SerializeField] private bool boomShroomUnlocked;
    [SerializeField] private float boomShroomCooldown = 8f;
    [SerializeField] private float boomPlantDistance = 1f;
    [SerializeField] private float boomPlantOffset = 0.5f;

    private float boomShroomTimer;
    private bool hasBoomTarget;
    private EnemyHealthPoint boomTarget;

    [Header("Call")]
    private bool hasCallTarget;
    private Vector2 callPosition;


    // =========================================================
    // Unity
    // =========================================================

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Poko ไม่มี NavMeshAgent");
            return;
        }

        // สำคัญสำหรับ NavMeshPlus 2D
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = stopDistance;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
                player = obj.transform;
        }

        if (player != null)
            followTarget = player.position;
    }

    private void Update()
    {
        if (navMeshAgent == null)
            return;

        attackTimer = CountDown(attackTimer);
        skillTimer = CountDown(skillTimer);
        boomShroomTimer = CountDown(boomShroomTimer);

        UpdateMovement();

        UpdateFollow();
        CheckAttack();
        CheckSkill();
        CheckBoomShroom();
    }


    // =========================================================
    // Movement
    // =========================================================

    private void UpdateMovement()
    {
        if (!navMeshAgent.isOnNavMesh)
            return;

        // Call มี priority สูงสุด
        if (hasCallTarget)
        {
            MoveToPosition(callPosition);
            return;
        }

        // Boom Shroom
        if (hasBoomTarget)
        {
            MoveToBoomShroom();
            return;
        }

        // Follow
        if (followMode)
        {
            MoveToPlayer();
            return;
        }

        // Attack Monster
        if (attackMonsterMode)
        {
            MoveToEnemy();
            return;
        }

        // ไม่มีคำสั่งให้เดิน
        StopMovement();
    }

    private bool MoveTo(Vector2 target, float targetStopDistance)
    {
        if (!navMeshAgent.isOnNavMesh)
            return false;

        float distance =
            Vector2.Distance(transform.position, target);

        if (distance <= targetStopDistance)
        {
            StopMovement();
            return true;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = targetStopDistance;

        navMeshAgent.SetDestination(target);

        Vector2 direction =
            target - (Vector2)transform.position;

        if (direction.sqrMagnitude > 0.001f)
            Flip(direction.x);

        return false;
    }

    private void StopMovement()
    {
        if (!navMeshAgent.isOnNavMesh)
            return;

        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
    }


    // =========================================================
    // Call
    // =========================================================

    private void MoveToPosition(Vector2 target)
    {
        if (MoveTo(target, stopDistance))
        {
            hasCallTarget = false;
        }
    }

    public void CallToPosition(Vector2 position)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PokoBark();

        // ยกเลิกคำสั่งอื่น
        followMode = false;
        attackMonsterMode = false;

        hasBoomTarget = false;
        boomTarget = null;

        // ตั้งตำแหน่งที่จะไป
        hasCallTarget = true;
        callPosition = position;
    }


    // =========================================================
    // Follow
    // =========================================================

    private void UpdateFollow()
    {
        if (!followMode || player == null)
            return;

        followTarget = Vector2.Lerp(
            followTarget,
            player.position,
            followSmooth * Time.deltaTime
        );
    }

    private void MoveToPlayer()
    {
        MoveTo(followTarget, followDistance);
    }

    public void ToggleFollow()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PokoBark();

        followMode = !followMode;

        if (!followMode)
        {
            StopMovement();
            return;
        }

        // เปิด Follow → ปิด Attack
        attackMonsterMode = false;

        // ยกเลิก Call
        hasCallTarget = false;

        // ยกเลิก Boom Shroom target
        hasBoomTarget = false;
        boomTarget = null;

        if (player != null)
            followTarget = player.position;
    }

    public bool IsFollowing()
    {
        return followMode;
    }


    // =========================================================
    // Attack Mode
    // =========================================================

    private void MoveToEnemy()
    {
        EnemyHealthPoint enemy = FindNearestEnemy();

        if (enemy == null)
        {
            StopMovement();
            return;
        }

        MoveTo(
            enemy.transform.position,
            attackRange
        );
    }

    private void CheckAttack()
    {
        if (!attackMonsterMode)
            return;

        if (attackTimer > 0f)
            return;

        EnemyHealthPoint enemy = FindNearestEnemy();

        if (enemy == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

        if (distance > attackRange)
            return;

        enemy.TakeDamage(attackDamage);

        attackTimer = attackCooldown;
    }

    private EnemyHealthPoint FindNearestEnemy()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(
                transform.position,
                detectRange
            );

        EnemyHealthPoint nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            EnemyHealthPoint enemy =
                col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null)
                continue;

            if (!enemy.CompareTag("Enemy"))
                continue;

            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }

    public void ToggleAttackMonster()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PokoBark();

        attackMonsterMode = !attackMonsterMode;

        if (attackMonsterMode)
        {
            // เปิด Attack → ปิด Follow
            followMode = false;

            // ยกเลิก Call
            hasCallTarget = false;

            // ยกเลิก Boom Shroom target
            hasBoomTarget = false;
            boomTarget = null;
        }
        else
        {
            StopMovement();
        }
    }

    public bool IsAttackMonsterMode()
    {
        return attackMonsterMode;
    }


    // =========================================================
    // Skill 1 - Area Attack
    // =========================================================

    private void CheckSkill()
    {
        if (!attackMonsterMode)
            return;

        if (skillTimer > 0f)
            return;

        EnemyHealthPoint enemy =
            FindNearestEnemy();

        if (enemy == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

        if (distance <= skillRadius)
            UseAreaSkill();
    }

    private void UseAreaSkill()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                transform.position,
                skillRadius
            );

        foreach (Collider2D col in enemies)
        {
            EnemyHealthPoint enemy =
                col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null)
                continue;

            if (!enemy.CompareTag("Enemy"))
                continue;

            enemy.TakeDamage(skillDamage);
        }

        skillTimer = skillCooldown;
    }


    // =========================================================
    // Skill 2 - Boom Shroom
    // =========================================================

    private void CheckBoomShroom()
    {
        if (!boomShroomUnlocked)
            return;

        if (boomShroomPrefab == null)
            return;

        if (boomShroomTimer > 0f)
            return;

        if (hasBoomTarget)
            return;

        boomTarget = FindNearestEnemy();

        if (boomTarget != null)
            hasBoomTarget = true;
    }

    private void MoveToBoomShroom()
    {
        if (boomTarget == null)
        {
            hasBoomTarget = false;
            return;
        }

        Vector2 target =
            boomTarget.transform.position;

        if (MoveTo(target, boomPlantDistance))
        {
            PlaceBoomShroom(
                target -
                (Vector2)transform.position
            );

            hasBoomTarget = false;
            boomTarget = null;
        }
    }

    private void PlaceBoomShroom(Vector2 direction)
    {
        if (boomShroomPrefab == null)
            return;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        direction.Normalize();

        Vector2 position =
            (Vector2)transform.position
            + direction * boomPlantOffset;

        Instantiate(
            boomShroomPrefab,
            position,
            Quaternion.identity
        );

        boomShroomTimer =
            boomShroomCooldown;
    }

    public void UnlockBoomShroom()
    {
        boomShroomUnlocked = true;
    }

    public bool IsBoomShroomUnlocked()
    {
        return boomShroomUnlocked;
    }

    public float GetBoomShroomCooldown()
    {
        return boomShroomCooldown;
    }


    // =========================================================
    // Utility
    // =========================================================

    private float CountDown(float timer)
    {
        return Mathf.Max(
            0f,
            timer - Time.deltaTime
        );
    }

    private void Flip(float x)
    {
        if (characterSprite == null)
            return;

        Vector3 scale =
            characterSprite.transform.localScale;

        if (x > 0f)
        {
            scale.x =
                Mathf.Abs(scale.x);
        }
        else if (x < 0f)
        {
            scale.x =
                -Mathf.Abs(scale.x);
        }

        characterSprite.transform.localScale =
            scale;
    }


    // =========================================================
    // Gizmos
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectRange
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            skillRadius
        );
    }
}