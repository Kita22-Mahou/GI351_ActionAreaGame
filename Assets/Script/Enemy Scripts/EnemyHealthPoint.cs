using UnityEngine;

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

        Shop.Instance.KillCountEnemy();
        
        Destroy(gameObject);

        DropItems();
    }

    void DropItems()
    {
        foreach (GameObject item in dropItems)
        {
            DropChance(item);

            Debug.Log("Skill Hit Enemy!");
        }
    }

    void DropChance(GameObject item)
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
}
