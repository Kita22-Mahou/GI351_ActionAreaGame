using UnityEngine;

public class PlayerHealthPoint : MonoBehaviour
{
    private Rigidbody rb;

    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        Debug.Log("PlayerHP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;

            PlayerDead();
        }
    }

    void PlayerDead()
    {
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Player Dead");

        Destroy(gameObject);

    }
}
