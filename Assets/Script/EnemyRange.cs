using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [Header("Target")]
    public float detectRange = 10f;
    public float attackRange = 8f;
    public float keepDistance = 3f;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("HP")]
    public float maxHP = 40f;
    public float currentHP;

    [Header("Attack")]
    public float attackDamage = 5f;
    public float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    [Header("Projectile")]
    public GameObject BulletPrefab;
    public Transform firePoint;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
    }
    void Start()
    {

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        GameObject CarObject = GameObject.FindGameObjectWithTag("Car");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (CarObject != null)
        {
            car = CarObject.transform;
        }
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Transform target = FindClosestTarget();

        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Filp(target);

        AimAtTarget(target);

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance > keepDistance)
        {
            MoveToTarget(target);
        }

        else if (distance < keepDistance - 1f)
        {
            MoveAwayFromTarget(target);
        }

        else
        {
            rb.linearVelocity = Vector2.zero;

            if (distance <= attackRange)
            {
                Attack(target);
            }
        }

        //if (distance <= attackRange)
        //{
        //    rb.linearVelocity = Vector2.zero;

        //    Attack(target);

        //    return;
        //}

        //if (distance <= detectRange)
        //{
        //    MoveToTarget(target);
        //}
        //else
        //{
        //    rb.linearVelocity = Vector2.zero;
        //}
    }

    void Filp(Transform target)
    {
        if (target == null)
            return;

        Vector3 direction =
            target.position - transform.position;

        if (direction.x > 0)
        {
            transform.localScale =
                new Vector3(
                    Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
        }
        else if (direction.x < 0)
        {
            transform.localScale =
                new Vector3(
                    -Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
        }
    }


    Transform FindClosestTarget()
    {
        Transform closestTarget = null;

        float closestDistance =
            Mathf.Infinity;

        if (player != null)
        {
            float playerDistance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            if (playerDistance <= detectRange &&
                playerDistance < closestDistance)
            {
                closestTarget = player;
                closestDistance = playerDistance;
            }
        }

        if (car != null)
        {
            float carDistance =
                Vector2.Distance(
                    transform.position,
                    car.position
                );

            if (carDistance <= detectRange &&
                carDistance < closestDistance)
            {
                closestTarget = car;
                closestDistance = carDistance;
            }
        }

        return closestTarget;
    }

    void MoveToTarget(Transform target)
    {
        Vector2 direction =
            (
                (Vector2)target.position -
                (Vector2)transform.position
            ).normalized;

        rb.linearVelocity =
            direction * moveSpeed;
    }

    void Attack(Transform target)
    {
        if (attackTimer > 0)
            return;

        attackTimer =
            attackCooldown;

        Shoot(target);
    }

    void MoveAwayFromTarget(Transform target)
    {
        Vector2 direction = ((Vector2)transform.position -(Vector2)target.position).normalized;

        rb.linearVelocity =direction * moveSpeed;
    }

    void AimAtTarget(Transform target)
    {
        if (target == null)
            return;

        Vector2 direction =
            (Vector2)target.position -
            (Vector2)firePoint.position;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        firePoint.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle
            );
    }

    void Shoot(Transform target)
    {
        if (BulletPrefab == null)
        {
            Debug.LogWarning(
                "Projectile Prefab ยังไม่ได้ใส่!"
            );

            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning(
                "Fire Point ยังไม่ได้ใส่!"
            );

            return;
        }

        GameObject projectile =
            Instantiate(
                BulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

        EnemyBullet projectileScript = projectile.GetComponent<EnemyBullet>();

        if (projectileScript != null)
        {
            Vector2 direction =
                (
                    (Vector2)target.position -
                    (Vector2)firePoint.position
                ).normalized;

            projectileScript.SetDirection(
                direction,
                attackDamage
            );
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP =
            Mathf.Clamp(
                currentHP,
                0f,
                maxHP
            );

        Debug.Log("Range Enemy HP: " +currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        rb.linearVelocity =Vector2.zero;

        Debug.Log("Range Enemy Dead!");

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,detectRange);

        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
