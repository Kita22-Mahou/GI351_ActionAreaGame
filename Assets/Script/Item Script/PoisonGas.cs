using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float damageInterval = 1f;

    private float damageTimer;


    private void Update()
    {
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyHealthPoint enemy =
            collision.GetComponentInParent<EnemyHealthPoint>();

        if (enemy == null || !enemy.CompareTag("Enemy"))
            return;

        if (damageTimer > 0f)
            return;

        enemy.TakeDamage(damage);

        damageTimer = damageInterval;
    }
}