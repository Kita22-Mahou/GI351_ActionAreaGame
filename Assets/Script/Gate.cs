using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    public enum GemType
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    [System.Serializable]
    public class GemSummsion
    {
        public string name;
        public GemType gemtype;

        public int neededAmount;
    }

    [Header("Gem Pool")]
    [SerializeField] private List<GemSummsion> pool = new();
    private int maxGemAmount = 3;
    [SerializeField ] public List<GemSummsion> currentPool = new();

    [Header("UI")]
    [SerializeField] private Button[] submitButtons;
    [SerializeField] private TMP_Text[] nameTexts;
    [SerializeField] private GameObject[] donePanel;


    [Header("Referent")]
    private GemInventory gemInventory;

    void Start()
    {
        gemInventory = FindAnyObjectByType<GemInventory>();

        if (currentPool.Count < maxGemAmount) // Genarate Gate Gems
        {
            for (int i = 0; i < maxGemAmount; i++)
            {
                int rand = UnityEngine.Random.Range(0, pool.Count);

                currentPool.Add(pool[rand]);
            }
        }

        for (int j = 0; j < maxGemAmount; j++) // Put in UI
        {
            int index = j;
            Debug.Log($"Get {index}");

            nameTexts[index].text = currentPool[index].name;

            submitButtons[index].onClick.RemoveAllListeners();
            submitButtons[index].onClick.AddListener(
                () => GemSubmission(currentPool[index].gemtype , index)
            );
        }
    }

    void GemSubmission(GemType gemType, int index)
    {
        GemSummsion gemsubmission = currentPool[index];

        gemInventory.AddGem(gemType, -1, index);

        if (gemsubmission.neededAmount == 0) // dont then pop done panel
        {
            donePanel[index].SetActive(true);
            submitButtons[index].onClick.RemoveAllListeners();

            CheckWin();
        }
    }

    void CheckWin()
    {
        if (currentPool[0].neededAmount == 0 &&
            currentPool[1].neededAmount == 0 &&
            currentPool[2].neededAmount == 0)
        {
            Debug.Log("YOU WIN!!!");
        }
    }

}
