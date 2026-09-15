using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private float attackDamage = 30f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackTimer = 0f;

    [Header("HP")]

    [SerializeField] private float maxHP = 250f;
    [SerializeField] private float currentHP;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D rb;

    [Header("Target")]
    [SerializeField] private float detectRange = 12f;
    [SerializeField] private float attackRange = 1.5f;
    private Transform player;
    private Transform car;

    [Header("Drop")]
    [SerializeField] private GameObject dropItem1;
    [SerializeField] private GameObject dropItem2;
    [SerializeField] private GameObject dropItem3;
    [SerializeField] private float dropChance = 10f;
    [SerializeField] private float dropRadius = 0.5f;

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

        FlipToTarget(target);
    }
    #endregion

    #region Movement
    void FlipToTarget(Transform target)
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)
                ,transform.localScale.y,transform.localScale.z);
        }
        else if (target.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x)
                ,transform.localScale.y,transform.localScale.z);
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

                Debug.Log("Tank Attack Player");
            }
        }


        else if (target.CompareTag("Car"))
        {
            Car carScript =
                target.GetComponent<Car>();


            if (carScript != null)
            {
                carScript.TakeDamage(attackDamage);

                Debug.Log("Tank Attack Car");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP,0f,maxHP);

        Debug.Log("Enemy HP: " + currentHP);


        if (currentHP <= 0)
        {
            Die();
        }
    }

    void DropItems()
    {
        CheckDrop(dropItem1);
        CheckDrop(dropItem2);
        CheckDrop(dropItem3);
    }

    void CheckDrop(GameObject item)
    {
        if (item == null)
            return;

        float randomChance = Random.Range(0f, 100f);

        if (randomChance <= dropChance)
        {
            Vector2 randomPosition =
                Random.insideUnitCircle * dropRadius;

            Vector3 spawnPosition =
                transform.position +
                new Vector3(
                    randomPosition.x,
                    randomPosition.y,
                    0f
                );

            Instantiate(
                item,
                spawnPosition,
                Quaternion.identity
            );
        }
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Enemy Dead");

        Destroy(gameObject);

        DropItems();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,detectRange);

        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}