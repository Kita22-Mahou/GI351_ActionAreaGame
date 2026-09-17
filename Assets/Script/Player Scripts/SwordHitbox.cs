using UnityEngine;
using System.Collections;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.SendMessage(
                "TakeDamage",
                 damage,
                 SendMessageOptions.DontRequireReceiver);
        }
        


        if (collision.CompareTag("Tree"))
        {
            collision.SendMessage(
                "TakeDamage",
                damage,
                SendMessageOptions.DontRequireReceiver);

            Debug.Log("Hit Tree!");
        }

            
    }
}
