using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private SwordMan player;

    public void BuyDamageUpgrade()
    {
        if (Inventory.Instance == null || player == null)
            return;

        if (Inventory.Instance.SpendWood(3))
        {
            player.UpgradeDamage(5f);
        }
        else
        {
            Debug.Log("Wood ไม่พอ");
        }
    }


    public void BuySpeedUpgrade()
    {
        if (Inventory.Instance == null || player == null)
            return;

        if (Inventory.Instance.SpendHide(2))
        {
            player.UpgradeSpeed(1f);
        }
        else
        {
            Debug.Log("Hide ไม่พ");
        }
    }

    public void BuyMaxHPUpgrade()
    {
        if (Inventory.Instance == null || player == null)
            return;

        if (Inventory.Instance.SpendBone(2))
        {
            player.UpgradeMaxHP(20f);
        }
        else
        {
            Debug.Log("Bone ไม่พอ");
        }
    }

    public void BuyHeal()
    {
        if (Inventory.Instance == null || player == null)
            return;

        if (Inventory.Instance.SpendBlood(1))
        {
            player.Heal(30f);
        }
        else
        {
            Debug.Log("Blood ไม่พอ");
        }
    }


}