using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthPoint : MonoBehaviour
{
    private Rigidbody rb;

    [Header("HP")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float currentHP;

    [Header("Drop")]
    [SerializeField] private GameObject[] dropItems;
    [SerializeField] private float dropChance = 10f;
    [SerializeField] private float dropRadius = 0.5f;

    private bool hasDroppedItem = false;

    [SerializeField] private float minSeparation = 0.8f;
    [SerializeField] private float maxSeparation = 1.2f;
    [SerializeField] private float separationStrength = 2f;

    private float separationDistance;

    private void Awake()
    {
        separationDistance = Random.Range(minSeparation,maxSeparation);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        AudioManager.Instance.HitMontsers();


        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Dead");

        AudioManager.Instance.PlayDeath();
        Destroy(gameObject);

        DropItems();
    }

    void DropItems()
    {
        foreach (GameObject item in dropItems)
        {
            DropChance(item);
        }
    }

    void DropChance(GameObject item)
    {
        if (hasDroppedItem || item == null)
            return;

        float randomChance = Random.Range(0f, 100f);

        if (randomChance > dropChance)
            return;

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

        hasDroppedItem = true;
    }

    private Vector2 GetSeparation()
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position, separationDistance);

        Vector2 separation = Vector2.zero;

        foreach (Collider2D hit in enemies)
        {
            if (hit.gameObject == gameObject)
                continue;

            EnemyHealthPoint enemy = hit.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null)
                continue;

            Vector2 direction =
                (Vector2)transform.position - (Vector2)hit.transform.position;

            if (direction.sqrMagnitude > 0.01f)
                separation += direction.normalized;
        }

        return separation;
    }

}
