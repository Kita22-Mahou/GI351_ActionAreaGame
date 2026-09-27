using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    //[SerializeField] private int faceDirection = 2;

    [Header("Attack")]
    [SerializeField] private GameObject[] attackFX;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDamage = 30f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float chargeTime = 0.5f;

    [SerializeField] private bool isAttacking = false;

    [Header("Referent")]
    //private FaceDetector faceDetector;
    private NavMeshAgent navMeshAgent;

    #region Event System
    private void Awake()
    {
        //faceDetector = GetComponent<FaceDetector>();
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
        //faceDirection = faceDetector.faceDetectDirection;

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

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance <= attackRange) // Attack
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            if (!isAttacking && attackTime <= 0f)
            {
                GetAttackDirection(target);
            }

            return;
        }

        NavMeshMovement(target, distance);

        //FlipToTarget(target);
    }
    #endregion

    #region Movement
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

    void NavMeshMovement(Transform target, float distance)
    {
        if (isAttacking || distance <= attackRange)
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

    void GetAttackDirection(Transform target) // คำนวณทิศทางการโจมตี และหมุนไปทางที่โจมตี
    {
        Vector2 direction = (target.position - transform.position).normalized;

        Vector2 attackPosition = direction * attackRange;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        StartCoroutine(EnableHitbox(attackPosition, rotation));
    }

    IEnumerator EnableHitbox(Vector2 pos, Quaternion rot)
    {
        int rand = Random.Range(0, attackFX.Length);
        int randRotation = Random.Range(0, 360);
        Quaternion rotation = Quaternion.Euler(0, 0, randRotation);
        isAttacking = true;
        attackHitbox.transform.position = (Vector2)transform.position + pos;
        attackHitbox.transform.rotation = rot;
        attackHitbox.SetActive(true);
        attackHitbox.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(chargeTime);
        Instantiate(attackFX[rand], (Vector2)transform.position + pos, rotation);
        attackHitbox.GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(DisableHitbox());
    }

    IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        attackHitbox.SetActive(false);
        attackHitbox.GetComponent<SpriteRenderer>().enabled = false;
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