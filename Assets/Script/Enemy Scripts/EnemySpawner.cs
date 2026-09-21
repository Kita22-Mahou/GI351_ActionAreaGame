using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject EnemyPrefab;

    [Range(0f, 100f)]
    public float spawnChance;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemySpawnData[] enemies;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnRadius = 4f;
    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int maxMonsters = 20;

    private float spawnTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null || enemies.Length == 0)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxMonsters)
            {
                SpawnMonster();
            }
        }
    }

    private void SpawnMonster()
    {
        GameObject selectedMonster = GetRandomMonster();

        if (selectedMonster == null)
            return;


        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float randomDistance = Random.Range(minSpawnRadius, spawnRadius);

        Vector2 spawnPosition = (Vector2)player.position + randomDirection * randomDistance;

        Instantiate(
            selectedMonster,
            spawnPosition,
            Quaternion.identity
        );
    }

    private GameObject GetRandomMonster()
    {
        float totalChance = 0f;

        foreach (EnemySpawnData enemy in enemies)
        {
            if (enemy.EnemyPrefab != null)
                totalChance += enemy.spawnChance;
        }

        if (totalChance <= 0f)
            return null;

        float randomValue = Random.Range(0f, totalChance);

        foreach (EnemySpawnData enemy in enemies)
        {
            if (enemy.EnemyPrefab == null)
                continue;

            randomValue -= enemy.spawnChance;

            if (randomValue <= 0f)
                return enemy.EnemyPrefab;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}