using UnityEngine;

public class CarHealthPoint : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        Debug.Log("HP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;

            Destroyed();
        }
    }

    void Destroyed()
    {
        Destroy(gameObject);
        Debug.Log("DESTROYED!");
    }
}
