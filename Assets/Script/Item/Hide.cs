using UnityEngine;

public class Hide : MonoBehaviour
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
        Inventory.AddHide(amount);

        Debug.Log("Get Hide +" + amount);

        Destroy(gameObject);
    }
}
