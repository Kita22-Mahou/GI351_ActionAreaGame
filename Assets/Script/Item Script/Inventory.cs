using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Item")]
    public int wood = 0;
    [SerializeField] private int maxwood = 10;

    public int bone = 0;
    [SerializeField] private int maxbone = 5;

    public int blood = 0;
    [SerializeField] private int maxblood = 5;

    public int hide = 0;
    [SerializeField] private int maxhide = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        wood = Mathf.Clamp(wood, 0, maxwood);
        bone = Mathf.Clamp(bone, 0, maxbone);
        blood = Mathf.Clamp(blood, 0, maxblood);
        hide = Mathf.Clamp(hide, 0, maxhide);
    }

    public static void AddWood(int amount)
    {
        if (Instance == null) return;

        Instance.wood = Mathf.Clamp(Instance.wood + amount, 0, Instance.maxwood);
    }

    public static void AddBone(int amount)
    {
        if (Instance == null) return;

        Instance.bone = Mathf.Clamp(Instance.bone + amount, 0, Instance.maxbone);
    }

    public static void AddBlood(int amount)
    {
        if (Instance == null) return;

        Instance.blood = Mathf.Clamp(Instance.blood + amount, 0, Instance.maxblood);
    }

    public static void AddHide(int amount)
    {
        if (Instance == null) return;

        Instance.hide = Mathf.Clamp(Instance.hide + amount, 0, Instance.maxhide);
    }

    public bool SpendWood(int amount)
    {
        if (wood < amount) return false;

        wood -= amount;
        return true;
    }

    public bool SpendBone(int amount)
    {
        if (bone < amount) return false;

        bone -= amount;
        return true;
    }

    public bool SpendBlood(int amount)
    {
        if (blood < amount) return false;

        blood -= amount;
        return true;
    }

    public bool SpendHide(int amount)
    {
        if (hide < amount) return false;

        hide -= amount;
        return true;
    }

    public int GetWood() => wood;
    public int GetBone() => bone;
    public int GetBlood() => blood;
    public int GetHide() => hide;
}