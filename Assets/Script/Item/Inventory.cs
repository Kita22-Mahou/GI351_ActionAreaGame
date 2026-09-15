using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Item")]
    public int wood = 0;
    private int maxwood = 10;
    public int bone = 0;
    private int maxbone = 5;
    public int blood = 0;
    private int maxblood = 5;

    public int hide = 0;
    private int maxhide =  5;


    private void Awake()
    {
         Instance = this;
    }

    private void Update()
    {
        if (wood >= maxwood) 
        {
        wood = maxwood;
        }

        if (blood >= maxblood)
        {
            blood = maxblood;
        }

        if (hide >= maxhide)
        {
            hide = maxhide;
        }

        if (bone >= maxbone)
        {
            bone = maxbone;
        }
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

    public static void AddBone(int amount)
    {
        if (Instance == null)
        {

            return;
        }

        Instance.bone += amount;

        Debug.Log("Bone = " + Instance.bone);
    }

    public int GetBone()
    {
        return bone;
    }

    public static void AddBlood(int amount)
    {
        if (Instance == null)
        {

            return;
        }

        Instance.blood += amount;

        Debug.Log("Blood = " + Instance.blood);
    }

    public int GetBlood()
    {
        return blood;
    }

    public static void AddHide(int amount)
    {
        if (Instance == null)
        {

            return;
        }

        Instance.hide += amount;

        Debug.Log("Hide = " + Instance.hide);
    }

    public int GetHide()
    {
        return blood;
    }

}
