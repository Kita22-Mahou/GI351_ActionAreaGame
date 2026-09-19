using UnityEngine;

public class Blood : MonoBehaviour
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
        Inventory.AddBlood(amount);

        Debug.Log("Get Blood +" + amount);

        Destroy(gameObject);
    }
}
