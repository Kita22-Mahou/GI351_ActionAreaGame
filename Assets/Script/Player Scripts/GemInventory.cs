using UnityEngine;
using static Gate;

public class GemInventory : MonoBehaviour
{
    private Gate gate;

    [Header("Gem Amount")]
    [SerializeField] private int GreenGemAmount = 0;
    [SerializeField] private int BlueGemAmount = 0;
    [SerializeField] private int YellowGemAmount = 0;

    private void Start()
    {
        gate = FindAnyObjectByType<Gate>();
    }

    public void AddGem(GemType gemType, int addAmount, int index)
    {
        switch (gemType)
        {
            case GemType.Green:
                if (GreenGemAmount > 0)
                {
                    GreenGemAmount += addAmount;
                    gate.Pool[index].neededAmount += addAmount;
                }
                break;

            case GemType.Blue:
                if (BlueGemAmount > 0)
                {
                    BlueGemAmount += addAmount;
                    gate.Pool[index].neededAmount += addAmount;
                }
                break;

            case GemType.Yellow:
                if (YellowGemAmount > 0)
                {
                    YellowGemAmount += addAmount;
                    gate.Pool[index].neededAmount += addAmount;
                }
                break;

            default:
                Debug.Log("Gem not found");
                break;
        }
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
}