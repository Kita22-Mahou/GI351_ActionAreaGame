using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    public enum GemType
    {
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
    private int maxGemAmount = 3;
    [SerializeField] public List<GemSummsion> Pool = new();

    [Header("UI")]
    [SerializeField] private Button[] submitButtons;
    [SerializeField] private TMP_Text[] nameTexts;
    [SerializeField] private GameObject[] donePanel;
    [SerializeField] private GameObject GatePanel;


    [Header("Referent")]
    private GemInventory gemInventory;

    private void Awake()
    {
        if (GatePanel != null)
            GatePanel.SetActive(false);
    }

    private void Start()
    {
        gemInventory = FindAnyObjectByType<GemInventory>();

        for (int i = 0; i < Pool.Count; i++)
        {
            int index = i;

            nameTexts[index].text = Pool[index].name;

            submitButtons[index].onClick.RemoveAllListeners();
            submitButtons[index].onClick.AddListener(
                () => GemSubmission(Pool[index].gemtype, index)
            );
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            Close();
    }

    private void GemSubmission(GemType gemType, int index)
    {
        GemSummsion gemsubmission = Pool[index];

        gemInventory.AddGem(gemType, -1, index);

        if (gemsubmission.neededAmount == 0)
        {
            donePanel[index].SetActive(true);
            submitButtons[index].onClick.RemoveAllListeners();

            CheckWin();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GatePanel.SetActive(true);

            Time.timeScale = 0f;

        }
    }

    public void Close()
    {
        GatePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void CheckWin()
    {
        if (Pool[0].neededAmount == 0 &&
            Pool[1].neededAmount == 0 &&
            Pool[2].neededAmount == 0)
        {
            Debug.Log("YOU WIN!!!");
        }
    }

}