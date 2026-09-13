using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    public SwordMan player;

    [Header("Car")]
    public Car car;

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
        if (player == null)
            return;

        playerHPBar.value = player.currentHP / player.maxHP;
    }


    void UpdateCarHP()
    {
        if (car == null)
            return;

        CarHPBar.value = car.currentHP / car.maxHP;
    }


    void UpdateOverheat()
    {
        if (car == null)
            return;

        overheatBar.value = car.currentOverheat / car.maxOverheat;

    }
}