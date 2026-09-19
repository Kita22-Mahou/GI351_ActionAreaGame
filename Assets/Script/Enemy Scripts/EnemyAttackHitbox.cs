using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private HashSet<GameObject> hitTargets = new HashSet<GameObject>(); // store damaged object

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthPoint target = collision.GetComponentInParent<PlayerHealthPoint>();

            if (target == null)
            {
                Debug.Log("Enemy | Target = null");
                return;
            }

            if (hitTargets.Contains(collision.gameObject))
            {
                return;
            }

            target.GetComponent<PlayerHealthPoint>().TakeDamage(damage);
            Debug.Log($"Enemy | Player takes {damage} damage");
        }

        if (collision.CompareTag("Car"))
        {
            CarHealthPoint target = collision.GetComponentInParent<CarHealthPoint>();

            if (target == null)
            {
                Debug.Log("Enemy | Target = null");
                return;
            }

            if (hitTargets.Contains(collision.gameObject))
            {
                return;
            }

            target.GetComponent<CarHealthPoint>().TakeDamage(damage);
            Debug.Log($"Enemy | Car takes {damage} damage");
        }

        hitTargets.Add(collision.gameObject);
    }

    public void HashSetClear() // clear when attack
    {
        hitTargets.Clear();
    }
}
