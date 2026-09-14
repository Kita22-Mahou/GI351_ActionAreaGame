using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AreaGeneration : MonoBehaviour
{
    public Transform[] startPosition;
    public GameObject[] areas;
    #region Areas index
    /*
    index 0 = LR
    index 1 = LRB
    index 2 = LRT
    index 3 = LRBT
    */
    #endregion

    private int direction;
    public float moveAmount;

    private float timeBtwArea;
    public float startTimeBtwArea = 0.25f;

    public LayerMask area;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;

    private void Start()
    {
        int randStartPos = Random.Range(0, startPosition.Length);
        transform.position = startPosition[randStartPos].position;
        Instantiate(areas[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwArea <= 0 && stopGeneration == false)
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
            if (transform.position.x < maxX)
            {
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, areas.Length);
                Instantiate(areas[rand], transform.position, Quaternion.identity);  // Spawn random pattern

                direction = Random.Range(1, 6);
                if (direction == 3) // Make not go LEFT
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else // Move BOTTOM
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) // Move LEFT
        {
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, areas.Length);
                Instantiate(areas[rand], transform.position, Quaternion.identity); // Spawn random pattern

                direction = Random.Range(3, 6); // Make not go RIGHT
            }
            else // Move BOTTOM
            {
                direction = 5;
            }
        }
        else if (direction == 5) // Move BOTTOM
        {
            if(transform.position.y > minY)
            {
                Collider2D areaDetection = Physics2D.OverlapCircle(transform.position, 1, area);
                if (areaDetection.GetComponent<AreaType>().type != 1 && areaDetection.GetComponent<AreaType>().type != 3)
                {
                    areaDetection.GetComponent<AreaType>().AreaDestruction();

                    int randBottomArea = Random.Range(1, 4);
                    if (randBottomArea == 2)
                    {
                        randBottomArea = 1;
                    }
                    Instantiate(areas[randBottomArea], transform.position, Quaternion.identity);
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(areas[rand], transform.position, Quaternion.identity); //

                direction = Random.Range(1, 6);
            }
            else
            {
                //STOP GENERATION
                stopGeneration = true;
            }
        }
    }
}
