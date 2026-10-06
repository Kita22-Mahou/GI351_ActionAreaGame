using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PokoHealthPoint : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float currentHP;

    [Header("HP Bar")]
    [SerializeField] private Scrollbar hpBar;
    [SerializeField] private Image hpImage;
    [SerializeField] private float hpSmooth = 0.15f;

    [Header("HP Color")]
    [SerializeField] private Color fullHPColor = Color.green;
    [SerializeField] private Color halfHPColor = Color.yellow;
    [SerializeField] private Color lowHPColor = Color.red;

    private float targetHP;
    private float hpVelocity;
    public GameOver gameOver;
    private void Start()
    {
        currentHP = maxHP;
        targetHP = 1f;

        hpBar.size = 1f;
        hpImage.color = fullHPColor;
    }

    private void Update()
    {
        hpBar.size = Mathf.SmoothDamp(hpBar.size,targetHP,ref hpVelocity,hpSmooth);

        hpImage.color = GetHPColor(targetHP);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        targetHP = currentHP / maxHP;

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            PokoDead();
        }
    }

    private Color GetHPColor(float hp)
    {
        if (hp > 0.5f)
            return Color.Lerp(halfHPColor, fullHPColor, (hp - 0.5f) * 2f);

        return Color.Lerp(lowHPColor, halfHPColor, hp * 2f);
    }

    private void PokoDead()
    {
        Destroy(gameObject);
        gameOver.ShowGameOver();
    }
}