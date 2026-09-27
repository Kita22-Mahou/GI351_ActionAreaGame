using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifeTime = 3f;

    [SerializeField] private float damage;
    private Vector2 direction;

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
            PlayerHealthPoint player = collision.GetComponent<PlayerHealthPoint>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else if (collision.CompareTag("Car"))
        {
            CarHealthPoint car = collision.GetComponent<CarHealthPoint>();

            if (car != null)
            {
                car.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}