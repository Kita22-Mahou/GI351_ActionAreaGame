using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Target")]

    public float detectRange = 8f;
    public float attackRange = 1.2f;

    [Header("Movement")]

    public float moveSpeed = 2f;

    [Header("Attack")]

    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private float attackTimer = 0f;


    private Transform player;
    private Transform car;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        GameObject CarObject =
            GameObject.FindGameObjectWithTag("Car");


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

        float distance =
            Vector2.Distance(
                transform.position,
                target.position
            );

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            Attack(target);

            return;
        }

        if (distance <= detectRange)
        {
            MoveToTarget(target);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

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
            float vehicleDistance =
                Vector2.Distance(
                    transform.position,
                    car.position
                );


            if (vehicleDistance <= detectRange &&
                vehicleDistance < closestDistance)
            {
                closestTarget = car;

                closestDistance = vehicleDistance;
            }
        }


        return closestTarget;
    }

    void MoveToTarget(Transform target)
    {
        Vector2 direction =
            ((Vector2)target.position -
            (Vector2)transform.position).normalized;


        rb.linearVelocity =
            direction * moveSpeed;
    }

    void Attack(Transform target)
    {
        if (attackTimer > 0)
            return;


        attackTimer = attackCooldown;

        if (target.CompareTag("Player"))
        {
            SwordMan playerScript =
                target.GetComponent<SwordMan>();


            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);

                Debug.Log(
                    "👾 Enemy Attack Player"
                );
            }
        }


        else if (target.CompareTag("Car"))
        {
            Car carScript =
                target.GetComponent<Car>();


            if (carScript != null)
            {
                carScript.TakeDamage(attackDamage);

                Debug.Log(
                    "👾 Enemy Attack Car"
                );
            }
        }
    }
}