using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyHealthPoint target = collision.GetComponent<EnemyHealthPoint>();
            if (target == null)
            {
                Debug.Log("Enemy | Target = null");
                return;
            }

            target.GetComponent<EnemyHealthPoint>().TakeDamage(damage);
            Debug.Log($"Enemy | Player takes {damage} damage");
        }
    }
}
