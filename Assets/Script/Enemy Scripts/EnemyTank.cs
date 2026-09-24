using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyTank : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int faceDirection = 2;
    private Rigidbody2D rb;

    [Header("Attack")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDamage = 30f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackTime = 0f;

    [SerializeField] private bool isAttacking = false;

    [Header("Referent")]
    private FaceDetector faceDetector;
    private NavMeshAgent navMeshAgent;

    #region Event System
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        faceDetector = GetComponent<FaceDetector>();
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

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
        faceDirection = faceDetector.faceDetectDirection;

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
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (attackTime <= 0)
            {
                AttackDirection(target);
            }

            return;
        }

        if (distance <= detectRange)
        {
            //NavMeshMove(target);
            MoveToTarget(target);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        //FlipToTarget(target);
    }
    #endregion

    #region Movement
    //void FlipToTarget(Transform target)
    //{
    //    if (target.position.x > transform.position.x)
    //    {
    //        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)
    //            ,transform.localScale.y,transform.localScale.z);
    //    }
    //    else if (target.position.x < transform.position.x)
    //    {
    //        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x)
    //            ,transform.localScale.y,transform.localScale.z);
    //    }
    //}

    Transform FindClosestTarget()
    {
        Transform closestTarget = null;

        float closestDistance = Mathf.Infinity;

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
            float CarDistance =
                Vector2.Distance(
                    transform.position,
                    car.position
                );


            if (CarDistance <= detectRange &&
                CarDistance < closestDistance)
            {
                closestTarget = car;

                closestDistance = CarDistance;
            }
        }


        return closestTarget;
    }

    //void NavMeshMove(Transform target)
    //{
    //    if (isAttacking)
    //    {
    //        rb.linearVelocity = Vector2.zero;
    //        return;

    //    }

    //    navMeshAgent.SetDestination(target.position);
    //}

    void MoveToTarget(Transform target)
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;


        rb.linearVelocity = direction * moveSpeed;
    }
    #endregion

    void AttackDirection(Transform target) // คำนวณทิศทางการโจมตี และหมุนไปทางที่โจมตี
    {
        Vector2 direction = (target.position - transform.position).normalized;

        Vector2 attackPosition = direction * attackRange;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        Attack(attackPosition, rotation);
    }

    void Attack(Vector2 pos, Quaternion rot)
    {
        isAttacking = true;
        attackHitbox.transform.position = (Vector2)transform.position + pos;
        attackHitbox.transform.rotation = rot;
        attackHitbox.SetActive(true);
        attackHitbox.GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(DisableHitbox());
    }

    IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        attackHitbox.SetActive(false);
        attackHitbox.GetComponent<CircleCollider2D>().enabled = false;
        attackHitbox.GetComponent<EnemyAttackHitbox>().HashSetClear();
        attackTime = attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,detectRange);

        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}