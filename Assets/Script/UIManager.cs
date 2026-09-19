using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    public PlayerHealthPoint playerHP;

    [Header("Car")]
    public CarHealthPoint carHP;
    private Car car;

    [Header("UI")]
    public Slider playerHPBar;
    public Slider CarHPBar;
    public Slider overheatBar;

    void Update()
    {
        UpdatePlayerHP();
        UpdateCarHP();
        UpdateOverheat();
    }

    void UpdatePlayerHP()
    {
        if (playerHP == null)
            return;

        playerHPBar.value = playerHP.currentHP / playerHP.maxHP;
    }

    void UpdateCarHP()
    {
        if (car == null)
            return;

        CarHPBar.value = carHP.currentHP / carHP.maxHP;
    }

    void UpdateOverheat()
    {
        if (car == null)
            return;

        overheatBar.value = car.currentOverheat / car.maxOverheat;

    }
}