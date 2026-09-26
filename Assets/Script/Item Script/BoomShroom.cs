using UnityEngine;

public class BoomShroom : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionDamage = 30f;
    [SerializeField] private float explodeTime = 3f;

    [Header("Poison Gas")]
    [SerializeField] private GameObject poisonGasPrefab;
    [SerializeField] private float gasDuration = 20f;

    private float timer;
    private bool exploded;


    private void Start()
    {
        timer = explodeTime;
    }


    private void Update()
    {
        if (exploded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Explode();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exploded)
            return;

        EnemyHealthPoint enemy = collision.GetComponentInParent<EnemyHealthPoint>();

        if (enemy == null || !enemy.CompareTag("Enemy"))
            return;

        Explode();
    }


    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position,explosionRadius);

        foreach (Collider2D col in enemies)
        {
            EnemyHealthPoint enemy = col.GetComponentInParent<EnemyHealthPoint>();

            if (enemy == null || !enemy.CompareTag("Enemy"))
                continue;

            enemy.TakeDamage(explosionDamage);
        }


        if (poisonGasPrefab != null)
        {
            GameObject gas = Instantiate(poisonGasPrefab,transform.position,Quaternion.identity);

            Destroy(gas, gasDuration);
        }

        Destroy(gameObject);
    }
}