using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Item")]
    public int wood = 0;

    private void Awake()
    {
         Instance = this;
    }

    public static void AddWood(int amount)
    {
        if (Instance == null)
        {

            return;
        }

        Instance.wood += amount;

        Debug.Log("Wood = " + Instance.wood);
    }

    public int GetWood()
    {
        return wood;
    }

}
