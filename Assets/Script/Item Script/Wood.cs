using UnityEngine;

public class Wood : MonoBehaviour
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
        Inventory.AddWood(amount);

        Debug.Log("Get Wood +" + amount);

        Destroy(gameObject);
    }



}
