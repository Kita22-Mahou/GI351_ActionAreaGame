using UnityEngine;

public class Poko : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject characterSprite;

    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float stopDistance = 0.05f;

    [Header("Follow")]
    [SerializeField] private bool followMode;
    [SerializeField] private float followDelay = 0.15f;

    private float followTimer;
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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        followTimer = followDelay;
    }

    private void Update()
    {
        attackTimer = CountDown(attackTimer);
        skillTimer = CountDown(skillTimer);
        boomShroomTimer = CountDown(boomShroomTimer);

        UpdateFollow();
        CheckAttack();
        CheckSkill();
        CheckBoomShroom();
    }

    private void FixedUpdate()
    {
        Move();
    }


    // Movement
    private void Move()
    {
        if (hasCallTarget)
        {
            MoveToPosition(callPosition);
            return;
        }

        if (hasBoomTarget)
        {
            MoveToBoomShroom();
            return;
        }

        if (followMode)
        {
            MoveToPlayer();
            return;
        }

        if (attackMonsterMode)
        {
            MoveToEnemy();
            return;
        }

        rb.linearVelocity = Vector2.zero;
    }

    private bool MoveTo(Vector2 target, float stopDistance)
    {
        Vector2 direction = target - rb.position;

        if (direction.magnitude <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return true;
        }

        direction.Normalize();
        rb.linearVelocity = direction * moveSpeed;
        Flip(direction.x);

        return false;
    }

    private void MoveToPosition(Vector2 target)
    {
        if (MoveTo(target, stopDistance))
        {
            rb.position = target;
            hasCallTarget = false;
        }
    }

    // Boom Shroom
    private void MoveToBoomShroom()
    {
        if (boomTarget == null)
        {
            hasBoomTarget = false;
            return;
        }

        Vector2 target = boomTarget.transform.position;
        Vector2 direction = target - rb.position;

        if (direction.magnitude <= boomPlantDistance)
        {
            rb.linearVelocity = Vector2.zero;

            PlaceBoomShroom(direction);

            hasBoomTarget = false;
            boomTarget = null;
            return;
        }

        direction.Normalize();
        rb.linearVelocity = direction * moveSpeed;

        Flip(direction.x);
    }

    private void PlaceBoomShroom(Vector2 direction)
    {
        if (boomShroomPrefab == null)
            return;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        direction.Normalize();

        Vector2 position =
            rb.position + direction * boomPlantOffset;

        Instantiate(boomShroomPrefab,position,Quaternion.identity);

        boomShroomTimer = boomShroomCooldown;
    }

    // Follow
    private void UpdateFollow()
    {
        if (!followMode || player == null)
            return;

        followTimer -= Time.deltaTime;

        if (followTimer <= 0f)
        {
            followTarget = player.position;
            followTimer = followDelay;
        }
    }

    private void MoveToPlayer()
    {
        MoveTo(followTarget, followDistance);
    }

    // Attack
    private void CheckAttack()
    {
        if (!attackMonsterMode || attackTimer > 0f)
            return;

        EnemyHealthPoint enemy = FindNearestEnemy();

        if (enemy == null)
            return;

        float distance = Vector2.Distance(rb.position,enemy.transform.position);

        if (distance > attackRange)
            return;

        enemy.TakeDamage(attackDamage);
        attackTimer = attackCooldown;
    }

    private void MoveToEnemy()
    {
        EnemyHealthPoint enemy = FindNearestEnemy();

        if (enemy == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        MoveTo(enemy.transform.position,attackRange);
    }

    private EnemyHealthPoint FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.position,detectRange);

        EnemyHealthPoint nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            EnemyHealthPoint enemy = col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null ||
                !enemy.CompareTag("Enemy"))
                continue;

            float distance = Vector2.Distance(rb.position,enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }

    // Skill 1
    private void CheckSkill()
    {
        if (!attackMonsterMode || skillTimer > 0f)
            return;

        EnemyHealthPoint enemy = FindNearestEnemy();

        if (enemy == null)
            return;

        float distance = Vector2.Distance(
            rb.position,
            enemy.transform.position
        );

        if (distance <= skillRadius)
            UseAreaSkill();
    }

    private void UseAreaSkill()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(rb.position,skillRadius);

        foreach (Collider2D col in enemies)
        {
            EnemyHealthPoint enemy = col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null || !enemy.CompareTag("Enemy"))
                continue;

            enemy.TakeDamage(skillDamage);
        }

        skillTimer = skillCooldown;
    }

    // Skill 2 - Boom Shroom
    private void CheckBoomShroom()
    {
        if (!boomShroomUnlocked ||
            boomShroomPrefab == null ||
            boomShroomTimer > 0f ||
            hasBoomTarget)
        {
            return;
        }

        boomTarget = FindNearestEnemy();

        if (boomTarget != null)
            hasBoomTarget = true;
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

    // Call
    public void CallToPosition(Vector2 position)
    {
        hasBoomTarget = false;
        boomTarget = null;

        hasCallTarget = true;
        callPosition = position;
    }

    // Follow
    public void ToggleFollow()
    {
        followMode = !followMode;

        if (!followMode)
            return;

        attackMonsterMode = false;

        if (player != null)
            followTarget = player.position;

        followTimer = followDelay;
    }

    public bool IsFollowing()
    {
        return followMode;
    }

    // Attack Mode
    public void ToggleAttackMonster()
    {
        attackMonsterMode = !attackMonsterMode;

        if (attackMonsterMode)
            followMode = false;
    }

    public bool IsAttackMonsterMode()
    {
        return attackMonsterMode;
    }

    private float CountDown(float timer)
    {
        return Mathf.Max(0f, timer - Time.deltaTime);
    }

    // Sprite
    private void Flip(float x)
    {
        if (characterSprite == null)
            return;

        Vector3 scale = characterSprite.transform.localScale;

        if (x > 0f)
            scale.x = Mathf.Abs(scale.x);
        else if (x < 0f)
            scale.x = -Mathf.Abs(scale.x);

        characterSprite.transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position,detectRange);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position,attackRange);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position,skillRadius);
    }
}