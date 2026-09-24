using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum UpgradeType
{
    Damage,
    Speed,
    MaxHP,
    Heal
}

public enum ResourceType
{
    Wood,
    Bone,
    Hide,
    Blood
}

[System.Serializable]
public class ShopUpgrade
{
    public string upgradeName;
    public string description;

    public UpgradeType upgradeType;
    public ResourceType resourceType;

    public int price;
    public float amount;
}

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [Header("Player")]
    [SerializeField] private SwordMan player;
    [SerializeField] private SwordHitBox HitBox;


    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] nameTexts;
    [SerializeField] private TMP_Text[] descriptionTexts;
    [SerializeField] private TMP_Text[] priceTexts;

    [Header("Kill Settings")]
    [SerializeField] private int KillOpenShop = 10;

    [Header("Upgrade Pool")]
    [SerializeField] private List<ShopUpgrade> upgradePool = new();

    private readonly List<ShopUpgrade> currentOffers = new();

    public int killCount = 0;
    private bool shopOpen = false;

    private void Awake()
    {
        Instance = this;

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void KillCountEnemy()
    {
        if (shopOpen)
            return;

        killCount++;

        if (killCount >= KillOpenShop)
        {
            killCount = 0;
            OpenShop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
    }

    private void OpenShop()
    {
        if (shopPanel == null)
            return;

        shopOpen = true;
        Time.timeScale = 0f;

        GenerateOffers();
        shopPanel.SetActive(true);
    }


    private void GenerateOffers()
    {
        currentOffers.Clear();

        List<ShopUpgrade> available = new List<ShopUpgrade>(upgradePool);

        for (int i = 0; i < 3 && available.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, available.Count);

            currentOffers.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < currentOffers.Count)
            {
                ShopUpgrade offer = currentOffers[i];

                nameTexts[i].text = offer.upgradeName;
                descriptionTexts[i].text = offer.description;
                priceTexts[i].text =
                    offer.price + " " + offer.resourceType;

                upgradeButtons[i].gameObject.SetActive(true);

                int index = i;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(
                    () => BuyUpgrade(index)
                );
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void BuyUpgrade(int index)
    {
        if (index < 0 || index >= currentOffers.Count)
            return;

        ShopUpgrade offer = currentOffers[index];

        if (!SpendResource(offer.resourceType, offer.price))
        {
            return;
        }

        ApplyUpgrade(offer);

        Debug.Log("ซื้อ " + offer.upgradeName + " สำเร็จ!");

        CloseShop();
    }

    private bool SpendResource(ResourceType type, int price)
    {
        if (Inventory.Instance == null)
            return false;

        switch (type)
        {
            case ResourceType.Wood:
                return Inventory.Instance.SpendWood(price);

            case ResourceType.Bone:
                return Inventory.Instance.SpendBone(price);

            case ResourceType.Hide:
                return Inventory.Instance.SpendHide(price);

            case ResourceType.Blood:
                return Inventory.Instance.SpendBlood(price);
        }

        return false;
    }

    private void ApplyUpgrade(ShopUpgrade offer)
    {
        if (player == null)
            return;

        switch (offer.upgradeType)
        {
            case UpgradeType.Damage:
                HitBox.UpgradeDamage(offer.amount);
                break;

            case UpgradeType.Speed:
                player.UpgradeSpeed(offer.amount);
                break;

            //case UpgradeType.MaxHP:
            //    player.UpgradeMaxHP(offer.amount);
            //    break;

            //case UpgradeType.Heal:
            //    player.Heal(offer.amount);
            //    break;
        }
    }

    public void CloseShop()
    {
        shopOpen = false;

        if (shopPanel != null)
            shopPanel.SetActive(false);

        Time.timeScale = 1f;
    }

}