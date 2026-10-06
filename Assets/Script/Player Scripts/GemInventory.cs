using UnityEngine;
using static Gate;

public class GemInventory : MonoBehaviour
{
    private Gate gate;

    [Header("Gem Amount")]
    [SerializeField] private int GreenGemAmount = 0;
    [SerializeField] private int BlueGemAmount = 0;
    [SerializeField] private int YellowGemAmount = 0;

    private void LateUpdate()
    {
        gate = FindAnyObjectByType<Gate>();
    }

    public bool AddGem(GemType gemType, int addAmount, int index)
    {
        switch (gemType)
        {
            case GemType.Green:
                if (GreenGemAmount + addAmount < 0)
                    return false;

                GreenGemAmount += addAmount;
                break;

            case GemType.Blue:
                if (BlueGemAmount + addAmount < 0)
                    return false;

                BlueGemAmount += addAmount;
                break;

            case GemType.Yellow:
                if (YellowGemAmount + addAmount < 0)
                    return false;

                YellowGemAmount += addAmount;
                break;

            default:
                Debug.Log("Gem not found");
                return false;
        }

        return true;
    }

    public void GetGem(Gate.GemType gemType, int amount)
    {
        switch (gemType)
        {
            case Gate.GemType.Green:
                GreenGemAmount += amount;
                break;

            case Gate.GemType.Blue:
                BlueGemAmount += amount;
                break;

            case Gate.GemType.Yellow:
                YellowGemAmount += amount;
                break;
        }
    }

    public int GetGemAmount(GemType gemType)
    {
        switch (gemType)
        {
            case GemType.Green:
                return GreenGemAmount;

            case GemType.Blue:
                return BlueGemAmount;

            case GemType.Yellow:
                return YellowGemAmount;

            default:
                return 0;
        }
    }
}