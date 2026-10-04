using UnityEngine;

public class SkillHitBox : MonoBehaviour
{
    [SerializeField] private float damage = 40f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealthPoint enemy =
            collision.GetComponentInParent<EnemyHealthPoint>();

        if (enemy == null)
            return;

        enemy.TakeDamage(damage);
    }
}