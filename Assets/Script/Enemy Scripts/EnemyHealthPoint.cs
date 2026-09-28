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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHP = maxHP;
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
        Debug.Log("Enemy Dead");

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
}
