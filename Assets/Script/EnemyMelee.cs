using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float attackRange = 1.2f;
    private Transform player;
    private Transform car;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;

    [Header("HP")]
    [SerializeField] private float maxHP = 50f;
    [SerializeField] private float currentHP;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackTimer = 0f;

    #region Event System
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
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }
    #endregion

    void Attack(Transform target)
    {
        if (attackTimer > 0)
            return;


        attackTimer = attackCooldown;

        if (target.CompareTag("Player"))
        {
            SwordMan playerScript = target.GetComponent<SwordMan>();


            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);

                Debug.Log("Enemy Attack Player");
            }
        }


        else if (target.CompareTag("Car"))
        {
            Car carScript =
                target.GetComponent<Car>();


            if (carScript != null)
            {
                carScript.TakeDamage(attackDamage);

                Debug.Log("Enemy Attack Car");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        Debug.Log("Enemy HP: " + currentHP);


        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Enemy Dead");

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
