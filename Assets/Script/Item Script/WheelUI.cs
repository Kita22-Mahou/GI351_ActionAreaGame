using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WheelUI : MonoBehaviour
{
    [System.Serializable]
    public class WheelSlot
    {
        public RectTransform slot;
        public GameObject highlight;
        public TMP_Text text;
        public string command;
    }

    [Header("Wheel")]
    [SerializeField] private GameObject wheel;

    [Header("Slots")]
    [SerializeField] private WheelSlot[] slots;

    [Header("Selection")]
    [SerializeField] private float selectDistance = 15f;

    [Header("Hold")]
    [SerializeField] private float holdTime = 0.2f;

    private bool isWheelOpen = false;
    private bool isHoldingMouse = false;

    private float holdTimer = 0f;

    private int selectedSlot = -1;

    private RectTransform wheelRect;
    private RectTransform canvasRect;

    private void Awake()
    {
        wheelRect = wheel.GetComponent<RectTransform>();

        canvasRect =
            wheel.transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        wheel.SetActive(false);

        ClearHighlight();
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isHoldingMouse = true;
            holdTimer = 0f;
        }

        if (isHoldingMouse &&
            Mouse.current.leftButton.isPressed)
        {
            holdTimer += Time.unscaledDeltaTime;


            if (!isWheelOpen &&
                holdTimer >= holdTime)
            {
                OpenWheel();
            }

            if (isWheelOpen)
            {
                UpdateSelection();
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (isWheelOpen)
            {
                ConfirmSelection();
            }
            else if (isHoldingMouse)
            {
                SwordMan.instance.AttackDirection();
            }

            isHoldingMouse = false;
            holdTimer = 0f;
        }
    }

    private void OpenWheel()
    {
        isWheelOpen = true;

        selectedSlot = -1;

        ClearHighlight();

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            null,
            out localPosition
        );

        wheelRect.localPosition = localPosition;

        wheel.SetActive(true);

        //SwordMan.instance.isInWheel = true;

        Debug.Log("Wheel Open");
    }

    private void UpdateSelection()
    {
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector2 center =
            RectTransformUtility.WorldToScreenPoint(
                null,
                wheelRect.position
            );

        Vector2 direction =
            mousePosition - center;

        // Mouse ยังอยู่ใกล้จุดกลาง
        if (direction.magnitude < selectDistance)
        {
            ClearHighlight();

            selectedSlot = -1;

            return;
        }

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        int newSlot =
            GetSlotFromAngle(angle);

        if (newSlot != selectedSlot)
        {
            selectedSlot = newSlot;

            UpdateHighlight();

            Debug.Log(
                "Selected : " +
                slots[selectedSlot].command
            );
        }
    }

    private int GetSlotFromAngle(float angle)
    {
        // ขวาบน
        if (angle >= 0f &&
            angle < 90f)
        {
            return 1;
        }

        // ซ้ายบน
        if (angle >= 90f &&
            angle < 180f)
        {
            return 0;
        }

        // ซ้ายล่าง
        if (angle >= 180f &&
            angle < 270f)
        {
            return 2;
        }

        // ขวาล่าง
        return 3;
    }

    private void UpdateHighlight()
    {
        ClearHighlight();

        if (selectedSlot < 0)
            return;

        slots[selectedSlot]
            .highlight
            .SetActive(true);
    }

    private void ClearHighlight()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].highlight != null)
            {
                slots[i]
                    .highlight
                    .SetActive(false);
            }
        }
    }

    private void ConfirmSelection()
    {
        if (selectedSlot >= 0)
        {
            string command =
                slots[selectedSlot].command;

            Debug.Log(
                "Selected Command = " +
                command
            );

            ExecuteCommand(command);
        }

        CloseWheel();
    }

    private void CloseWheel()
    {
        isWheelOpen = false;

        ClearHighlight();

        wheel.SetActive(false);

        //SwordMan.instance.isInWheel = false;

        selectedSlot = -1;

        Debug.Log("Wheel Close");
    }

    private void ExecuteCommand(string command)
    {
        switch (command)
        {
            case "Attack":

                Debug.Log("COMMAND : ATTACK");

                break;

            case "Follow":

                Debug.Log("COMMAND : FOLLOW");

                break;

            case "Move":

                Debug.Log("COMMAND : MOVE");

                break;

            case "Stop":

                Debug.Log("COMMAND : STOP");

                break;
        }
    }
}