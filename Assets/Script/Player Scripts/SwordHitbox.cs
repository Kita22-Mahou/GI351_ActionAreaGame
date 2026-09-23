using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    [SerializeField] private HashSet<GameObject> hitTargets = new HashSet<GameObject>(); // store damaged object

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthPoint target = collision.GetComponentInParent<EnemyHealthPoint>();

            if (target == null)
            {
                Debug.Log("Player | Target = null");
                return;
            }

            if (hitTargets.Contains(collision.gameObject))
            {
                return;
            }

            target.GetComponent<EnemyHealthPoint>().TakeDamage(damage);
            Debug.Log($"Player | Enemy takes {damage} damage");
        }

        if (collision.CompareTag("Tree"))
        {
            Tree target = collision.GetComponentInParent<Tree>();

            if (target == null)
            {
                Debug.Log("Player | Target = null");
                return;
            }

            if (hitTargets.Contains(collision.gameObject))
            {
                return;
            }

            target.GetComponent<Tree>().TakeDamage(damage);
            Debug.Log($"Player | Tree takes {damage} damage");
        }

        hitTargets.Add(collision.gameObject);
    }

    public void HashSetClear() // clear when attack
    {
        hitTargets.Clear();
    }

    public void UpgradeDamage(float amount)
    {
        damage += amount;
    }

}
