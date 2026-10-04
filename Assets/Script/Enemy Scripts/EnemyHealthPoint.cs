using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [Header("HP Bar")]
    [SerializeField] private Scrollbar hpBar;
    [SerializeField] private float hpBarHideTime = 3f;
    private float hpBarTimer;
    private float targetHP;
    private float hpVelocity;

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
        currentHP = maxHP;
        targetHP = 1f;

        hpBar.size = 1f;
        hpBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        hpBar.size = Mathf.SmoothDamp(hpBar.size,targetHP,ref hpVelocity,0.15f);

        if (!hpBar.gameObject.activeSelf)
            return;

        hpBarTimer -= Time.deltaTime;

        if (hpBarTimer <= 0f)
            hpBar.gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        targetHP = currentHP / maxHP;

        hpBarTimer = hpBarHideTime;

        hpBar.gameObject.SetActive(true);

        if (currentHP <= 0f)
            Die();
    }



    void Die()
    {

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
