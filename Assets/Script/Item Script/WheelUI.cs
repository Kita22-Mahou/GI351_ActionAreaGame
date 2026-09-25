using UnityEngine;
using UnityEngine.InputSystem;

public class WheelUI : MonoBehaviour
{
    [System.Serializable]
    public class WheelSlot
    {
        public RectTransform slot;
        public GameObject highlight;
        public string command;
    }

    [Header("Wheel")]
    [SerializeField] private GameObject wheel;

    [Header("Poko")]
    [SerializeField] private Poko poko;

    [Header("Slots")]
    [SerializeField] private WheelSlot[] slots;

    [Header("Settings")]
    [SerializeField] private float holdTime = 0.2f;
    [SerializeField] private float selectDistance = 15f;

    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform wheelRect;

    private bool holdingMouse;
    private bool wheelOpen;

    private float holdTimer;
    private int selectedSlot = -1;
    private Vector2 callPosition;


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
        }

        if (wheel != null)
        {
            wheelRect = wheel.GetComponent<RectTransform>();
        }
    }


    private void Start()
    {
        if (wheel != null)
        {
            wheel.SetActive(false);
        }

        ClearHighlight();
    }


    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            holdingMouse = true;
            holdTimer = 0f;
        }


        if (holdingMouse &&
            Mouse.current.leftButton.isPressed)
        {
            holdTimer += Time.unscaledDeltaTime;

            if (!wheelOpen &&
                holdTimer >= holdTime)
            {
                OpenWheel();
            }

            if (wheelOpen)
            {
                UpdateSelection();
            }
        }


        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (wheelOpen)
            {
                ConfirmSelection();
            }
            else
            {
                Attack();
            }

            holdingMouse = false;
            holdTimer = 0f;
        }
    }

    private void Attack()
    {
        if (SwordMan.instance == null)
            return;

        SwordMan.instance.AttackDirection();
    }

    private void OpenWheel()
    {
        if (wheel == null ||
            wheelRect == null ||
            canvasRect == null)
        {
            return;
        }


        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        callPosition = GetMouseWorldPosition();

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            null,
            out localPosition
        );

        wheelRect.localPosition = localPosition;


        wheelOpen = true;
        selectedSlot = -1;

        ClearHighlight();

        wheel.SetActive(true);


        if (SwordMan.instance != null)
        {
            SwordMan.instance.isInWheel = true;
        }
    }

    private void UpdateSelection()
    {
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();


        Vector2 wheelCenter =
            RectTransformUtility.WorldToScreenPoint(
                null,
                wheelRect.position
            );


        Vector2 direction =
            mousePosition - wheelCenter;

        if (direction.magnitude < selectDistance)
        {
            selectedSlot = -1;

            ClearHighlight();

            return;
        }


        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;


        if (angle < 0f)
        {
            angle += 360f;
        }


        int slot = GetSlot(angle);


        if (slot != selectedSlot)
        {
            selectedSlot = slot;

            ShowHighlight();
        }
    }

    private int GetSlot(float angle)
    {
        // Top Right
        if (angle >= 0f &&
            angle < 90f)
        {
            return 1;
        }


        // Top Left
        if (angle >= 90f &&
            angle < 180f)
        {
            return 0;
        }


        // Bottom Left
        if (angle >= 180f &&
            angle < 270f)
        {
            return 3;
        }


        // Bottom Right
        return 2;
    }
    private void ShowHighlight()
    {
        ClearHighlight();

        if (selectedSlot < 0 ||
            selectedSlot >= slots.Length)
        {
            return;
        }


        if (slots[selectedSlot].highlight != null)
        {
            slots[selectedSlot]
                .highlight
                .SetActive(true);
        }
    }


    private void ClearHighlight()
    {
        if (slots == null)
            return;


        foreach (WheelSlot slot in slots)
        {
            if (slot.highlight != null)
            {
                slot.highlight.SetActive(false);
            }
        }
    }

    private void ConfirmSelection()
    {
        if (selectedSlot >= 0 &&
            selectedSlot < slots.Length)
        {
            ExecuteCommand(
                slots[selectedSlot].command
            );
        }

        CloseWheel();
    }

    private void ExecuteCommand(string command)
    {
        if (poko == null)
        {
            return;
        }


        switch (command)
        {
            case "Call":
                poko.CallToPosition(callPosition);
                break;


            case "Follow":
                poko.ToggleFollow();
                break;


            case "Attack":
                poko.ToggleAttackMonster();
                break;


            case "None":

                break;
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        if (Camera.main == null)
            return Vector2.zero;


        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );
        return new Vector2(mouseWorld.x, mouseWorld.y);
    }

    private void CloseWheel()
    {
        wheelOpen = false;
        selectedSlot = -1;

        ClearHighlight();


        if (wheel != null)
        {
            wheel.SetActive(false);
        }


        if (SwordMan.instance != null)
        {
            SwordMan.instance.isInWheel = false;
        }
    }
}