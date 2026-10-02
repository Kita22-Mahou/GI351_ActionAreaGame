using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour
{
    [Header("Event Settings")]
    [SerializeField] private int eventAmount = 2;
    [SerializeField] private GameObject[] eventAreas;

    [Header("Area Positions")]
    [SerializeField] private GameObject[] areaPositions;

    private AreaGeneration areaGeneration;

    private int gemEventSpawnedAmount = 0;

    // เก็บเฉพาะตำแหน่ง ไม่เก็บ GameObject ที่อาจโดน Destroy
    private List<Vector2> availablePositions;


    private void Start()
    {
        areaGeneration = FindAnyObjectByType<AreaGeneration>();

        // สร้าง List ของตำแหน่ง
        availablePositions = new List<Vector2>();

        foreach (GameObject position in areaPositions)
        {
            if (position != null)
            {
                availablePositions.Add(position.transform.position);
            }
        }

        StartCoroutine(SpawnEvents());
    }


    private IEnumerator SpawnEvents()
    {
        // รอจนกว่า AreaGeneration จะสร้าง Map เสร็จ
        yield return new WaitUntil(() => areaGeneration.stopGeneration);


        while (gemEventSpawnedAmount < eventAmount)
        {
            // ไม่มีตำแหน่งเหลือ
            if (availablePositions.Count == 0)
            {
                Debug.LogWarning(
                    "ไม่มี Area Position เหลือสำหรับสร้าง Event"
                );

                yield break;
            }


            bool spawned = TrySpawnEvent();


            if (spawned)
            {
                gemEventSpawnedAmount++;

                Debug.Log(
                    "Event Spawned: " +
                    gemEventSpawnedAmount +
                    "/" +
                    eventAmount
                );
            }


            yield return null;
        }


        Debug.Log("Event Spawn Complete!");

        Destroy(gameObject);
    }


    private bool TrySpawnEvent()
    {
        // สุ่มตำแหน่ง
        int randomIndex = Random.Range(0, availablePositions.Count);

        Vector2 position = availablePositions[randomIndex];

        // ให้ EventSpawner ย้ายไปตำแหน่งที่สุ่มได้
        transform.position = position;

        // เอาตำแหน่งนี้ออกจาก List
        // เพื่อไม่ให้สุ่มซ้ำ
        availablePositions.RemoveAt(randomIndex);

        Debug.Log("EventSpawner moved to: " + transform.position);


        // หา Collider ทั้งหมดที่อยู่ตรงตำแหน่งนี้
        Collider2D[] collisions =
            Physics2D.OverlapPointAll(transform.position);


        if (collisions.Length == 0)
        {
            Debug.Log("ไม่เจอ Collider");
            return false;
        }


        // หา Room จาก Collider ทั้งหมด
        Collider2D roomCollider = null;

        foreach (Collider2D collision in collisions)
        {
            Debug.Log(
                "เจอ: " +
                collision.gameObject.name +
                " | Tag: " +
                collision.gameObject.tag
            );

            if (collision.CompareTag("Room"))
            {
                roomCollider = collision;
                break;
            }
        }


        // ไม่มี Room ตรงตำแหน่งนี้
        if (roomCollider == null)
        {
            Debug.Log("ไม่มี Room ตรงตำแหน่งนี้");
            return false;
        }


        // -------------------------
        // เจอ Room
        // -------------------------

        Vector2 eventPosition = transform.position;


        Debug.Log(
            "เจอ Room ที่: " +
            roomCollider.gameObject.name
        );


        // ทำลาย Room
        Destroy(roomCollider.gameObject);


        // สุ่ม Event
        int eventIndex = Random.Range(
            0,
            eventAreas.Length
        );


        // Spawn Event
        Instantiate(
            eventAreas[eventIndex],
            eventPosition,
            Quaternion.identity
        );


        Debug.Log(
            "Spawn Event: " +
            eventAreas[eventIndex].name
        );


        return true;
    }
}