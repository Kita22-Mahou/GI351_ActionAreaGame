using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WheelUI : MonoBehaviour
{
    [System.Serializable]
    public class WheelSlot
    {
        public RectTransform slot;
        public Image image;
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
    [SerializeField] private float selectDistance = 15f;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color selectedColor = Color.green;

    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform wheelRect;

    private bool wheelOpen;
    private int selectedSlot = -1;

    private Vector2 callPosition;


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
            canvasRect = canvas.GetComponent<RectTransform>();

        if (wheel != null)
            wheelRect = wheel.GetComponent<RectTransform>();
    }


    private void Start()
    {
        if (wheel == null)
            return;

        wheel.SetActive(false);

        ClearHighlight();
        UpdateSlotColors();
    }

    private void Update()
    {
        if (Keyboard.current == null ||
            Mouse.current == null ||
            wheel == null)
        {
            return;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            OpenWheel();
        }

        if (wheelOpen)
        {
            UpdateSelection();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            ConfirmSelection();
        }
    }

    // Wheel
    private void OpenWheel()
    {
        if (canvasRect == null || wheelRect == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            null,
            out localPosition
        );

        wheelRect.localPosition = localPosition;

        callPosition = GetMouseWorldPosition();

        selectedSlot = -1;
        wheelOpen = true;

        ClearHighlight();
        UpdateSlotColors();

        wheel.SetActive(true);

        if (SwordMan.instance != null)
            SwordMan.instance.isInWheel = true;
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
            UpdateSlotColors();

            return;
        }

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg;

        if (angle < 0f)
            angle += 360f;

        int slot = GetSlot(angle);

        if (!IsSlotAvailable(slot))
        {
            selectedSlot = -1;

            ClearHighlight();
            UpdateSlotColors();

            return;
        }

        if (slot != selectedSlot)
        {
            selectedSlot = slot;

            ClearHighlight();

            if (slots[selectedSlot].highlight != null)
            {
                slots[selectedSlot]
                    .highlight
                    .SetActive(true);
            }

            UpdateSlotColors();
        }
    }

    private int GetSlot(float angle)
    {
        if (angle >= 0f && angle < 90f)
            return 1;

        if (angle >= 90f && angle < 180f)
            return 0;

        if (angle >= 180f && angle < 270f)
            return 3;

        return 2;
    }

    // Slot
    private bool IsSlotAvailable(int index)
    {
        if (index < 0 || index >= slots.Length)
            return false;

        return slots[index].command != "None";
    }

    private void UpdateSlotColors()
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].image == null)
                continue;

            if (!IsSlotAvailable(i))
            {
                slots[i].image.color = lockedColor;
                continue;
            }

            if (i == selectedSlot)
            {
                slots[i].image.color = selectedColor;
            }
            else
            {
                slots[i].image.color = normalColor;
            }
        }
    }

    private void ClearHighlight()
    {
        if (slots == null)
            return;

        foreach (WheelSlot slot in slots)
        {
            if (slot.highlight != null)
                slot.highlight.SetActive(false);
        }
    }

    // Command
    private void ConfirmSelection()
    {
        if (!wheelOpen)
            return;

        if (selectedSlot >= 0 &&
            selectedSlot < slots.Length)
        {
            if (IsSlotAvailable(selectedSlot))
            {
                ExecuteCommand(
                    slots[selectedSlot].command
                );
            }
        }

        CloseWheel();
    }

    private void ExecuteCommand(string command)
    {
        if (poko == null)
            return;

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
        }
    }

    // Position
    private Vector2 GetMouseWorldPosition()
    {
        if (Camera.main == null)
            return Vector2.zero;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

        return new Vector2(
            worldPosition.x,
            worldPosition.y
        );
    }

    // Close
    private void CloseWheel()
    {
        wheelOpen = false;
        selectedSlot = -1;

        ClearHighlight();
        UpdateSlotColors();

        wheel.SetActive(false);

        if (SwordMan.instance != null)
            SwordMan.instance.isInWheel = false;
    }
}