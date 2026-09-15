using UnityEngine;

public class AreaBlankGeneration : MonoBehaviour
{
    public LayerMask Area;
    public AreaGeneration areaGeneration;

    void Update()
    {
        Collider2D areaDetection = Physics2D.OverlapCircle(transform.position, 1, Area);
        if (areaDetection == null && areaGeneration.stopGeneration == true)
        {
            int rand = Random.Range(0, areaGeneration.areas.Length); // Select Random Area
            Instantiate(areaGeneration.areas[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}