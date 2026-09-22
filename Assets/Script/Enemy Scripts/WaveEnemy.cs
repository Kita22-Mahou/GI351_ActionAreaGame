using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaveEnemyData
{
    [Header("Wave")]
    public int enemyCount = 10;

    [Header("Spawn")]
    public float spawnInterval = 0.5f;

    [Header("Enemies")]
    public EnemySpawnData[] enemies;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject EnemyPrefab;

    [Range(0f, 100f)]
    public float spawnChance = 100f;
}

public class WaveEnemy : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;

    [Header("Wave Settings")]
    [SerializeField] private WaveEnemyData[] waves;

    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("Spawn Settings")]
    [SerializeField] private float offScreenDistance = 2f;

    [SerializeField] private int maxMonsters = 50;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < waves.Length; i++)
        {

            yield return StartCoroutine(SpawnWave(waves[i]));


            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(WaveEnemyData wave)
    {
        if (wave == null)
            yield break;

        if (wave.enemies == null || wave.enemies.Length == 0)
        {

            yield break;
        }

        for (int i = 0; i < wave.enemyCount; i++)
        {
            int currentMonsterCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (currentMonsterCount >= maxMonsters)
            {
            yield return new WaitUntil(() =>GameObject.FindGameObjectsWithTag("Enemy").Length< maxMonsters);
            }

            GameObject enemyPrefab =
                GetRandomEnemy(wave.enemies);

            if (enemyPrefab != null)
            {
                SpawnEnemy(enemyPrefab);
            }
            yield return new WaitForSeconds(
                wave.spawnInterval
            );
        }

    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (player == null)
            return;

        Vector2 spawnPosition =
            GetOffScreenPosition();

        GameObject enemy = Instantiate(enemyPrefab,spawnPosition,Quaternion.identity);
    }

    private GameObject GetRandomEnemy(
        EnemySpawnData[] enemyList)
    {
        float totalChance = 0f;

        foreach (EnemySpawnData enemy in enemyList)
        {
            if (enemy == null)
                continue;

            if (enemy.EnemyPrefab == null)
                continue;

            totalChance += enemy.spawnChance;
        }

        if (totalChance <= 0f)
            return null;

        float randomValue = Random.Range(0f, totalChance);

        foreach (EnemySpawnData enemy in enemyList)
        {
            if (enemy == null)
                continue;

            if (enemy.EnemyPrefab == null)
                continue;

            randomValue -= enemy.spawnChance;

            if (randomValue <= 0f)
            {
                return enemy.EnemyPrefab;
            }
        }

        return null;
    }

    private Vector2 GetOffScreenPosition()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        float height =
            mainCamera.orthographicSize;

        float width =
            height * mainCamera.aspect;

        Vector2 cameraPosition =
            mainCamera.transform.position;

        
        int side = Random.Range(0, 4);

        float x;
        float y;

        switch (side)
        {
            case 0:

                x = cameraPosition.x - width - offScreenDistance;

                y = Random.Range(cameraPosition.y - height,cameraPosition.y + height);

                return new Vector2(x, y);

            case 1:

                x = cameraPosition.x + width + offScreenDistance;

                y =
                    Random.Range(cameraPosition.y - height,cameraPosition.y + height);

                return new Vector2(x, y);

            case 2:

                x = Random.Range(cameraPosition.x - width,cameraPosition.x + width);

                y = cameraPosition.y - height - offScreenDistance;

                return new Vector2(x, y);

            
            default:

                x = Random.Range(cameraPosition.x - width,cameraPosition.x + width);

                y = cameraPosition.y + height + offScreenDistance;

                return new Vector2(x, y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null)
            return;

        float height = Camera.main.orthographicSize;

        float width = height * Camera.main.aspect;

        Vector3 center = Camera.main.transform.position;

        Vector3 size = new Vector3(width * 2f,height * 2f,0f);

        Gizmos.DrawWireCube(center,size);

        Gizmos.DrawWireCube(center,size +new Vector3(offScreenDistance * 2f,offScreenDistance * 2f,0f));
    }
}