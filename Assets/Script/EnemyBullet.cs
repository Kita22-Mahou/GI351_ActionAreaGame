using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 3f;

    private Vector2 direction;
    private float damage;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection,float newDamage)
    {
        direction = newDirection.normalized;
        damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwordMan player =
                collision.GetComponent<SwordMan>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else if (collision.CompareTag("Car"))
        {
            Car car =
                collision.GetComponent<Car>();

            if (car != null)
            {
                car.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}