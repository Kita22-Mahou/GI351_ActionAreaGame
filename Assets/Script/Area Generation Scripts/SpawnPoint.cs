using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] objects;
    [SerializeField] bool isPointSpawn;

    [Header("Area Spawn")]
    [SerializeField] bool isAreaSpawn;
    [SerializeField] int objectAmount;
    [SerializeField] float gapBTWObject;
    [SerializeField] float delaySpawnBTWObject;
    private float delayAttempt = 0.05f;

    private PolygonCollider2D spawnArea;
    private List<Vector2> ObjectSpawnedPositions = new List<Vector2>();

    private void Start()
    {
        spawnArea = GetComponent<PolygonCollider2D>();
        PointSpawn();
        StartCoroutine(AreaSpawn());
    }

    void PointSpawn()
    {
        if (!isPointSpawn)
            return;

        int rand = Random.Range(0, objects.Length);
        GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.SetParent(transform.parent);
        Destroy(gameObject);
    }

    IEnumerator AreaSpawn()
    {
        if (!isAreaSpawn)
            yield break;

        Bounds bounds = spawnArea.bounds;

        for (int x = 1; x <= objectAmount; x++) // spawn in object amount
        {
            bool isValidPos = false;

            Vector2 spawnPosition = Vector2.zero;

            int maxAttempt = 50;
            for (int attempt = 0; attempt < maxAttempt; attempt++) // attempt to spawn an Object
            {
                float randX = Random.Range(bounds.min.x, bounds.max.x);
                float randY = Random.Range(bounds.min.y, bounds.max.y);

                spawnPosition = new Vector2(randX, randY);

                if (!IsPointInsidePolygon(spawnPosition))
                {
                    continue;
                }

                isValidPos = true;

                foreach (Vector2 previousPos in ObjectSpawnedPositions) // compare distance in List
                {
                    float distance = Vector2.Distance(spawnPosition, previousPos);

                    if (distance < gapBTWObject) // if too close -> break
                    {
                        isValidPos = false;
                        break;
                    }
                }

                if (isValidPos)
                {
                    break;
                }
                yield return new WaitForSeconds(delayAttempt);
            }

            if (!isValidPos)
            {
                Debug.LogWarning( $"object {x + 1} | Could not find valid position");

                continue;
            }

            int rand = Random.Range(0, objects.Length);
            GameObject instance = Instantiate(objects[rand], spawnPosition, Quaternion.identity);
            instance.transform.SetParent(transform.parent);
            ObjectSpawnedPositions.Add(spawnPosition);

            yield return new WaitForSeconds(delaySpawnBTWObject);
        }
        Destroy(gameObject);
    }
    private bool IsPointInsidePolygon(Vector2 point) // check if its int the shape
    {
        Vector2 localPoint =
            spawnArea.transform.InverseTransformPoint(point);

        Vector2[] points = spawnArea.points;

        bool isInside = false;

        for (int i = 0, j = points.Length - 1;
             i < points.Length;
             j = i++)
        {
            bool intersect =
                ((points[i].y > localPoint.y) !=
                 (points[j].y > localPoint.y))
                &&
                (localPoint.x <
                 (points[j].x - points[i].x) *
                 (localPoint.y - points[i].y) /
                 (points[j].y - points[i].y) +
                 points[i].x);

            if (intersect)
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }

}
