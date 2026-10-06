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
        FindUI();

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
                () => GemSubmission(
                    Pool[index].gemtype,
                    index
                )
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

        // ลด Gem ที่ผู้เล่นมี
        gemInventory.AddGem(
            gemType,
            -1,
            index
        );

        // ลดจำนวน Gem ที่ Gate ต้องการ
        gemsubmission.neededAmount--;


        Debug.Log(
            gemsubmission.name +
            " Needed Amount: " +
            gemsubmission.neededAmount
        );


        if (gemsubmission.neededAmount <= 0)
        {
            gemsubmission.neededAmount = 0;

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


    private void FindUI()
    {
        GameObject gateUIObject = null;

        GameObject[] allObjects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // ต้องเป็น Object ที่อยู่ใน Scene จริง
            if (!obj.scene.IsValid())
                continue;

            if (obj.name == "Gate UI")
            {
                gateUIObject = obj;
                break;
            }
        }


        if (gateUIObject == null)
        {
            Debug.LogError("ไม่พบ Gate UI");
            return;
        }


        Transform gateUI = gateUIObject.transform;


        // =========================
        // Gate Panel
        // =========================

        GatePanel = gateUIObject;


        // =========================
        // Buttons
        // =========================

        submitButtons =
            gateUI.GetComponentsInChildren<Button>(true);


        // =========================
        // Name Text
        // =========================

        TMP_Text[] allTexts =
            gateUI.GetComponentsInChildren<TMP_Text>(true);

        List<TMP_Text> foundNameTexts = new();

        foreach (TMP_Text text in allTexts)
        {
            if (text.gameObject.name == "Name Text")
            {
                foundNameTexts.Add(text);
            }
        }

        nameTexts = foundNameTexts.ToArray();


        // =========================
        // Done Image
        // =========================

        Transform[] allChildren =
            gateUI.GetComponentsInChildren<Transform>(true);

        List<GameObject> foundDonePanels = new();

        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Done Image")
            {
                foundDonePanels.Add(child.gameObject);
            }
        }

        donePanel = foundDonePanels.ToArray();


        // =========================
        // Debug
        // =========================

        //Debug.Log(
        //    "Gate UI Found | " +
        //    "Buttons: " + submitButtons.Length +
        //    " | Name Texts: " + nameTexts.Length +
        //    " | Done Panels: " + donePanel.Length +
        //    " | Gate Panel: " + (GatePanel != null)
        //);
    }
}