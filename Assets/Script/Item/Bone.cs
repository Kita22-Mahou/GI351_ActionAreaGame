using UnityEngine;

public class Bone : MonoBehaviour
{

    private int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }

    }

    public void Collect()
    {
        Inventory.AddBone(amount);

        Debug.Log("Get Bone +" + amount);

        Destroy(gameObject);
    }
}
