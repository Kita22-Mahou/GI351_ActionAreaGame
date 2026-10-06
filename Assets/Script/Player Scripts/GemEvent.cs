using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemEvent : MonoBehaviour
{
    [System.Serializable]
    public class MonsterSpawn
    {
        public GameObject prefab;
        public int amount;
    }

    [Header("Gem")]
    [SerializeField] private GameObject[] gemPrefabs;

    [Header("Monster")]
    [SerializeField] private MonsterSpawn[] monsters;
    [SerializeField] private int waveCount = 1;

    [Header("Spawn")]
    [SerializeField] private float spawnRadius = 6f;
    [SerializeField] private float minPlayerDistance = 4f;
    [SerializeField] private float nextSpawnDelay = 1f;

    private Transform player;
    private GemSpawnCheck gemSpawnCheck;

    private bool started;

    private List<GameObject> spawnedMonsters = new();


    private void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        if (obj != null)
            player = obj.transform;

        gemSpawnCheck = GetComponentInChildren<GemSpawnCheck>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (started || !collision.CompareTag("Player"))
            return;

        started = true;

        StartCoroutine(StartEvent());
    }


    private IEnumerator StartEvent()
    {
        for (int wave = 0; wave < waveCount; wave++)
        {
            for (int i = 0; i < 2; i++)
            {
                SpawnMonsters(i);

                yield return new WaitUntil(() => !HasMonster());

                yield return new WaitForSeconds(nextSpawnDelay);
            }
        }

        SpawnGem();
    }


    private bool HasMonster()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null)
                return true;
        }

        return false;
    }


    private void SpawnMonsters(int batch)
    {
        spawnedMonsters.Clear();

        foreach (MonsterSpawn monster in monsters)
        {
            int amount = monster.amount / 2;

            if (batch == 1)
                amount = monster.amount - amount;

            for (int i = 0; i < amount; i++)
            {
                GameObject enemy = Instantiate(
                    monster.prefab,
                    GetSpawnPosition(),
                    Quaternion.identity
                );

                spawnedMonsters.Add(enemy);
            }
        }
    }


    private Vector2 GetSpawnPosition()
    {
        Vector2 position;

        do
        {
            position =
                (Vector2)transform.position +
                Random.insideUnitCircle * spawnRadius;

        } while (
            player != null &&
            Vector2.Distance(position, player.position) < minPlayerDistance
        );

        return position;
    }


    private void SpawnGem()
    {
        if (gemSpawnCheck == null)
        {
            Debug.LogWarning("ไม่พบ GemSpawnCheck");
            return;
        }


        // หา Gem ที่ยังไม่เคย Spawn
        List<int> availableGems = new();


        for (int i = 0; i < gemPrefabs.Length; i++)
        {
            if (!IsGemSpawned(i))
            {
                availableGems.Add(i);
            }
        }


        // Gem ครบทั้ง 3 แบบแล้ว
        if (availableGems.Count == 0)
        {
            Debug.Log("Gem ทั้ง 3 แบบถูก Spawn ครบแล้ว");
            gameObject.SetActive(false);
            return;
        }


        // สุ่มจาก Gem ที่ยังไม่เคย Spawn
        int randomIndex =
            Random.Range(0, availableGems.Count);

        int gemIndex =
            availableGems[randomIndex];


        // บันทึกว่า Gem นี้ถูก Spawn แล้ว
        SetGemSpawned(gemIndex);


        // Spawn Gem
        Instantiate(
            gemPrefabs[gemIndex],
            transform.position,
            Quaternion.identity
        );


        Debug.Log(
            "Spawn Gem: " +
            gemPrefabs[gemIndex].name
        );


        gameObject.SetActive(false);
    }


    private bool IsGemSpawned(int index)
    {
        switch (index)
        {
            case 0:
                return gemSpawnCheck.isGreeneGemSpawned;

            case 1:
                return gemSpawnCheck.isBlueGemSpawned;

            case 2:
                return gemSpawnCheck.isYellowGemSpawned;

            default:
                return true;
        }
    }


    private void SetGemSpawned(int index)
    {
        switch (index)
        {
            case 0:
                gemSpawnCheck.isGreeneGemSpawned = true;
                break;

            case 1:
                gemSpawnCheck.isBlueGemSpawned = true;
                break;

            case 2:
                gemSpawnCheck.isYellowGemSpawned = true;
                break;
        }
    }
}