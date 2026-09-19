using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float keepDistance = 3f;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    private Rigidbody2D rb;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackTimer = 0f;

    [Header("Projectile")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform firePoint;

    #region Event System
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Flip(target);

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
    #endregion

    void Flip(Transform target)
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

    #region Movement
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
    #endregion

    #region Abilities
    void Attack(Transform target)
    {
        if (attackTimer > 0)
            return;

        attackTimer = attackCooldown;

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
        GameObject projectile =
            Instantiate(
                BulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

        EnemyBullet projectileScript = projectile.GetComponent<EnemyBullet>();

        if (projectileScript != null) // Set target and damage then send to EnemyBullet
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
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,detectRange);

        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
