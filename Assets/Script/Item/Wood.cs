using UnityEngine;

public class Wood : MonoBehaviour
{

    private int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Get Wood");

        Inventory.AddWood(amount);

        Destroy(gameObject);
    }



}
