using UnityEngine;

public class Poko : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject characterSprite;
    [SerializeField] private GameObject skillEffect;

    private Rigidbody2D rb;


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float callStopDistance = 0.05f;


    [Header("Follow")]
    [SerializeField] private bool followMode;
    [SerializeField] private float followDelay = 0.15f;

    private float followTimer;
    private Vector2 followTarget;


    [Header("Attack Monster")]
    [SerializeField] private bool attackMonsterMode;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer;


    [Header("Skill")]
    [SerializeField] private float skillRadius = 2f;
    [SerializeField] private float skillDamage = 40f;
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillDuration = 0.1f;

    private float skillTimer;


    private bool hasCallTarget;
    private Vector2 callPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (skillEffect != null)
        {
            skillEffect.SetActive(false);
        }
    }


    private void Start()
    {
        if (player == null)
        {
            GameObject obj =
                GameObject.FindGameObjectWithTag("Player");

            if (obj != null)
            {
                player = obj.transform;
            }
        }

        if (player != null)
        {
            followTarget = player.position;
        }

        followTimer = followDelay;
    }


    private void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (skillTimer > 0f)
        {
            skillTimer -= Time.deltaTime;
        }

        UpdateFollow();
        CheckAttack();
        CheckSkill();
    }


    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (hasCallTarget)
        {
            MoveToCall();
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


    private void MoveToCall()
    {
        Vector2 direction =
            callPosition - rb.position;

        float distance =
            direction.magnitude;

        if (distance <= callStopDistance)
        {
            rb.linearVelocity = Vector2.zero;

            rb.position = callPosition;

            hasCallTarget = false;

            return;
        }

        direction.Normalize();

        rb.linearVelocity =
            direction * moveSpeed;

        Flip(direction.x);
    }


    private void MoveToPlayer()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            followTarget - rb.position;

        if (direction.magnitude <= followDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        direction.Normalize();

        rb.linearVelocity =
            direction * moveSpeed;

        Flip(direction.x);
    }


    private void MoveToEnemy()
    {
        EnemyHealthPoint enemy =
            FindNearestEnemy();

        if (enemy == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (Vector2)enemy.transform.position -
            rb.position;

        if (direction.magnitude <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        direction.Normalize();

        rb.linearVelocity =
            direction * moveSpeed;

        Flip(direction.x);
    }

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

    private void CheckAttack()
    {
        if (!attackMonsterMode)
            return;

        if (attackTimer > 0f)
            return;

        EnemyHealthPoint enemy =
            FindNearestEnemy();

        if (enemy == null)
            return;

        float distance =
            Vector2.Distance(
                rb.position,
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
                rb.position,
                detectRange
            );

        EnemyHealthPoint nearest = null;

        float nearestDistance =
            Mathf.Infinity;

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
                    rb.position,
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

    private void CheckSkill()
    {
        if (!attackMonsterMode)
            return;

        if (skillTimer > 0f)
            return;

        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(
                rb.position,
                skillRadius
            );

        foreach (Collider2D col in colliders)
        {
            EnemyHealthPoint enemy =
                col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null)
                continue;

            if (!enemy.CompareTag("Enemy"))
                continue;

            UseSkill();
            return;
        }
    }


    private void UseSkill()
    {
        Debug.Log("Poko Skill");

        if (skillEffect != null)
        {
            skillEffect.SetActive(true);

            Invoke(
                nameof(HideSkill),
                skillDuration
            );
        }

        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(
                rb.position,
                skillRadius
            );

        foreach (Collider2D col in colliders)
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


    private void HideSkill()
    {
        if (skillEffect != null)
        {
            skillEffect.SetActive(false);
        }
    }

    public void CallToPosition(Vector2 position)
    {
        callPosition = position;
        hasCallTarget = true;
    }

    public void ToggleFollow()
    {
        followMode = !followMode;

        if (followMode)
        {
            attackMonsterMode = false;

            if (player != null)
            {
                followTarget = player.position;
            }

            followTimer = followDelay;

            Debug.Log("[Follow ON");
        }
        else
        {
            Debug.Log("Follow OFF");
        }
    }


    public bool IsFollowing()
    {
        return followMode;
    }


    // ========================================
    // Attack Mode
    // ========================================

    public void ToggleAttackMonster()
    {
        attackMonsterMode =
            !attackMonsterMode;

        if (attackMonsterMode)
        {
            followMode = false;
        }
        else
        {
            Debug.Log("Attack OFF");
        }
    }


    public bool IsAttackMonsterMode()
    {
        return attackMonsterMode;
    }

    private void Flip(float x)
    {
        if (characterSprite == null)
            return;

        Vector3 scale =
            characterSprite.transform.localScale;

        if (x > 0f)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else if (x < 0f)
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        characterSprite.transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (!hasCallTarget)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawSphere(
            callPosition,
            0.15f
        );
    }
}
