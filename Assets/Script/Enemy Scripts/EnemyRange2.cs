using UnityEngine;
using UnityEngine.AI;

public class EnemyRange2 : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackRange = 8f;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private int faceDirection = 2;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackTime = 0f;

    [SerializeField] private bool isAttacking = false;

    [Header("Projectile")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Referent")]
    private FaceDetector faceDetector;
    private NavMeshAgent navMeshAgent;

    #region Event System
    private void Awake()
    {
        faceDetector = GetComponent<FaceDetector>();
    }
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = moveSpeed;

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
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Transform target = FindClosestTarget();

        if (target == null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            return;
        }

        //Flip(target);

        AimAtTarget(target);

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance <= attackRange) // Attack
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            if (/*!isAttacking &&*/ attackTime <= 0f)
            {
                Attack(target);
            }

            return;
        }

        NavMeshMovement(target, distance);
    }
    #endregion

    //void Flip(Transform target)
    //{
    //    if (target == null)
    //        return;

    //    Vector3 direction =
    //        target.position - transform.position;

    //    if (direction.x > 0)
    //    {
    //        transform.localScale =
    //            new Vector3(
    //                Mathf.Abs(transform.localScale.x),
    //                transform.localScale.y,
    //                transform.localScale.z
    //            );
    //    }
    //    else if (direction.x < 0)
    //    {
    //        transform.localScale =
    //            new Vector3(
    //                -Mathf.Abs(transform.localScale.x),
    //                transform.localScale.y,
    //                transform.localScale.z
    //            );
    //    }
    //}

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

    void NavMeshMovement(Transform target, float distance)
    {
        if (/*isAttacking || */distance <= attackRange)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            return;

        }

        if (distance <= detectRange)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.position);
        }
        else
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
    }
    #endregion

    #region Abilities
    void Attack(Transform target)
    {
        if (attackTime > 0)
            return;

        attackTime = attackCooldown;

        Shoot(target);
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
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
