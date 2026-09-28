using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Gem")]
    [SerializeField] private Gate.GemType gemType;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GemInventory inventory = collision.GetComponent<GemInventory>();

        if (inventory == null)
            return;

        inventory.GetGem(gemType, amount);

        Destroy(gameObject);
    }
}