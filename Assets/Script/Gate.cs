using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] private List<GemSummsion> Pool = new();

    private int[] initialNeededAmounts;


    [Header("UI")]
    [SerializeField] private Button[] submitButtons;
    [SerializeField] private GameObject[] donePanel;
    [SerializeField] private GameObject GatePanel;
    [SerializeField] private GameObject winPanel;


    [Header("Reference")]
    private GemInventory gemInventory;


    #region Unity Event

    private void Awake()
    {
        SaveInitialGemAmount();

        FindUI();
        FindWinPanel();
    }


    private void Start()
    {
        SetupGame();
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartSence")
        {
            return;
        }

        FindUI();
        FindWinPanel();

        gemInventory = FindAnyObjectByType<GemInventory>();

        ResetGate();
        SetupSubmitButtons();
    }


    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (GatePanel == null)
        {
            Debug.LogError("GatePanel is null");
            return;
        }

        GatePanel.SetActive(true);

        SwordMan.instance.isGateOpen = true;

        AudioManager.Instance.StopWalk();

        Time.timeScale = 0f;
    }

    #endregion


    #region Setup

    private void SetupGame()
    {
        gemInventory = FindAnyObjectByType<GemInventory>();

        ResetGate();
        SetupSubmitButtons();
    }


    private void SaveInitialGemAmount()
    {
        initialNeededAmounts = new int[Pool.Count];

        for (int i = 0; i < Pool.Count; i++)
        {
            initialNeededAmounts[i] = Pool[i].neededAmount;
        }
    }


    private void SetupSubmitButtons()
    {
        if (submitButtons == null)
        {
            return;
        }

        for (int i = 0; i < Pool.Count; i++)
        {
            if (i >= submitButtons.Length)
            {
                return;
            }

            int index = i;

            submitButtons[index].onClick.RemoveAllListeners();

            submitButtons[index].onClick.AddListener(
                () => GemSubmission(Pool[index].gemtype, index)
            );
        }
    }

    #endregion


    #region Gem System

    private void GemSubmission(GemType gemType, int index)
    {
        if (gemInventory == null)
        {
            Debug.LogError("ไม่พบ GemInventory");
            return;
        }

        GemSummsion gemSubmission = Pool[index];

        if (gemInventory.GetGemAmount(gemType) <= 0)
        {
            return;
        }

        gemInventory.AddGem(gemType, -1, index);

        gemSubmission.neededAmount--;

        Debug.Log(gemSubmission.name +" Needed Amount: " +gemSubmission.neededAmount);


        if (gemSubmission.neededAmount <= 0)
        {
            gemSubmission.neededAmount = 0;

            donePanel[index].SetActive(true);

            submitButtons[index].gameObject.SetActive(false);

            submitButtons[index].onClick.RemoveAllListeners();

            CheckWin();
        }
    }


    private void CheckWin()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i].neededAmount > 0)
            {
                return;
            }
        }

        ShowWin();
    }

    #endregion


    #region Gate

    public void Close()
    {
        if (GatePanel != null)
        {
            GatePanel.SetActive(false);
        }

        if (SwordMan.instance != null)
        {
            SwordMan.instance.isGateOpen = false;
        }

        Time.timeScale = 1f;
    }


    private void ResetGate()
    {
        
        for (int i = 0; i < Pool.Count; i++)
        {
            Pool[i].neededAmount = initialNeededAmounts[i];
        }

        if (donePanel != null)
        {
            for (int i = 0; i < donePanel.Length; i++)
            {
                if (donePanel[i] != null)
                {
                    donePanel[i].SetActive(false);
                }
            }
        }

        if (submitButtons != null)
        {
            for (int i = 0; i < submitButtons.Length; i++)
            {
                if (submitButtons[i] != null)
                {
                    submitButtons[i].gameObject.SetActive(true);
                }
            }
        }

        if (GatePanel != null)
        {
            GatePanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (SwordMan.instance != null)
        {
            SwordMan.instance.isGateOpen = false;
        }


        Time.timeScale = 1f;
    }

    #endregion


    #region Find UI

    private void FindUI()
    {
        GameObject gateUIObject = null;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();


        foreach (GameObject obj in allObjects)
        {
            if (!obj.scene.IsValid())
            {
                continue;
            }

            if (obj.name == "Gate UI")
            {
                gateUIObject = obj;
                break;
            }
        }


        if (gateUIObject == null)
        {
            Debug.LogError("ไม่พบ Gate UI");
            GatePanel = null;
            submitButtons = null;
            donePanel = null;
            return;
        }

        Transform gateUI = gateUIObject.transform;

        GatePanel = gateUIObject;

        submitButtons = gateUI.GetComponentsInChildren<Button>(true);


        Transform[] allChildren = gateUI.GetComponentsInChildren<Transform>(true);

        List<GameObject> foundDonePanels = new();


        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == "Done Image")
            {
                foundDonePanels.Add(child.gameObject);
            }
        }

        donePanel = foundDonePanels.ToArray();
    }


    private void FindWinPanel()
    {
        winPanel = null;

        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include);

        foreach (Canvas canvas in canvases)
        {
            Transform[] children =
                canvas.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.name == "WinPanel")
                {
                    winPanel = child.gameObject;

                    Debug.Log("พบ WinPanel: " + winPanel.name);

                    return;
                }
            }
        }

        Debug.LogError("ไม่พบ WinPanel ใน Canvas");
    }

    #endregion


    #region Win

    private void ShowWin()
    {
        FindWinPanel();

        if (winPanel == null)
        {
            Debug.LogError("ShowWin() : หา WinPanel ไม่เจอ");
            return;
        }

        winPanel.SetActive(true);

        if (GatePanel != null)
        {
            GatePanel.SetActive(false);
        }

        if (SwordMan.instance != null)
        {
            SwordMan.instance.isGateOpen = true;
        }

        AudioManager.Instance.StopWalk();

        Time.timeScale = 0f;
    }


    public void ReturnToStart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("StartSence");
    }

    #endregion
}