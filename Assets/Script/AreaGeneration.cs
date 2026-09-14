using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AreaGeneration : MonoBehaviour
{
    public Transform[] startPosition;
    public GameObject[] areas;

    private int direction;
    public float moveAmount;

    private float timeBtwArea;
    public float startTimeBtwArea = 0.25f;

    private void Start()
    {
        int randStartPos = Random.Range(0, startPosition.Length);
        transform.position = startPosition[randStartPos].position;
        Instantiate(areas[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwArea <= 0)
        {
            Move();
            timeBtwArea = startTimeBtwArea;
        }
        else
        {
            timeBtwArea -= Time.deltaTime;
        }

    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // Move RIGHT
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            transform.position = newPos;
        }
        else if (direction == 3 || direction == 4) // Move LEFT
        {
            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
            transform.position = newPos;
        }
        else if (direction == 5)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
            transform.position = newPos;
        }

        Instantiate(areas[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);
    }
}
