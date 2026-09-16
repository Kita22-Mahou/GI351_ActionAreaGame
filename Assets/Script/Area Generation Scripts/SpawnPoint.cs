using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.SetParent(transform.parent);
        Destroy(gameObject);
    }
}
