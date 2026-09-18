using UnityEngine;
using System.Collections;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthPoint target = collision.GetComponent<EnemyHealthPoint>();
            if (target == null)
            {
                Debug.Log("Enemy | Target = null");
                return;
            }

            target.GetComponent<EnemyHealthPoint>().TakeDamage(damage);
            Debug.Log($"Player | Enemy takes {damage} damage");
        }

        if (collision.CompareTag("Tree"))
        {
            Tree target = collision.GetComponent<Tree>();
            if (target == null)
            {
                Debug.Log("Tree | Target = null");
                return;
            }

            target.GetComponent<Tree>().TakeDamage(damage);
            Debug.Log($"Player | Enemy takes {damage} damage");
        }
    }
}
