using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float boostSpeed = 5f;
    private Rigidbody2D rb;

    [Header("Overheat")]
    public float maxOverheat = 100f;
    public float currentOverheat = 0f;

    [SerializeField] private float overheatRate = 30f;
    [SerializeField] private float coolDownRate = 20f;

    [SerializeField] private bool isBoosting = false;
    [SerializeField] private bool isOverheated = false;

    [Header("Call")]
    [SerializeField] private float callStopDistance = 1.5f;
    private Vector2 destination;
    [SerializeField] private bool isCalling = false;

    [Header("Call to mouse")]
    private Vector2 destinationToMouse;
    [SerializeField] private bool isCallingToMouse = false;

    [Header("Item Detect")]
    [SerializeField] private float itemDetectRange = 3f;
    [SerializeField] private float itemCollectDistance = 0.7f;
    [SerializeField] private LayerMask itemLayer;

    private Transform targetItem;

    [Header("Referent")]
    public static Car Instance;

    #region Event System
    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        HandleBoost();
        HandleOverheat();
    }


    void FixedUpdate()
    {
        if (isCalling)
        {
            CallMove();
            return;
        }
        if (isCallingToMouse)
        {
            CallToMouse();
            return;
        }
        DetectItem();
    }
    #endregion

    #region Movement
    void HandleBoost()
    {
        //isBoosting = Keyboard.current.fKey.isPressed;

        if (isOverheated)
        {
            isBoosting = false;
        }
    }

    void HandleOverheat()
    {
        if (isBoosting && !isOverheated)
        {
            currentOverheat += overheatRate * Time.deltaTime;

            currentOverheat = Mathf.Clamp(currentOverheat, 0f, maxOverheat);

            if (currentOverheat >= maxOverheat)
            {
                currentOverheat = maxOverheat;

                isOverheated = true;

                Debug.Log("OverHeat!");
            }
        }
        else
        {
            currentOverheat -= coolDownRate * Time.deltaTime;

            if (currentOverheat <= 0)
            {
                currentOverheat = 0;
            }


            if (currentOverheat <= 20f)
            {
                isOverheated = false;
            }
        }
    }

    public void MakeCall(Transform playerPos)
    {
        isCallingToMouse = false;
        isCalling = true;
        destination = playerPos.position;
    }

    void CallMove()
    {
        if (isCallingToMouse)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                destination
            );

        if (distance <= callStopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            isCalling = false;
            Debug.Log("Im here");
            return;
        }

        Vector2 direction = ((Vector2)destination - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * normalSpeed;
    }

    public void MakeCallToMouse(Vector2 mousePos)
    {
        isCalling = false;
        isCallingToMouse = true;
        destinationToMouse = mousePos;
    }

    void CallToMouse()
    {
        if (isCalling)
            return;

        float distance =
               Vector2.Distance(
                   transform.position,
                   destinationToMouse
               );

        if (distance <= callStopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            isCallingToMouse = false;
            Debug.Log("Im here");
            return;
        }

        Vector2 direction = ((Vector2)destinationToMouse - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * normalSpeed;
    }


        void DetectItem()
        {
            if (targetItem == null)
            {
                FindNearestItem();
            }

            if (targetItem == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    targetItem.position
                );

            if (distance <= itemCollectDistance)
            {
                CollectTargetItem();
                return;
            }

            Vector2 direction =
                ((Vector2)targetItem.position -
                 (Vector2)transform.position).normalized;

            rb.linearVelocity = direction * normalSpeed;
        }

        void FindNearestItem()
        {
            Collider2D[] items =
                Physics2D.OverlapCircleAll(
                    transform.position,
                    itemDetectRange,
                    itemLayer
                );

            float closestDistance = Mathf.Infinity;

            targetItem = null;

            foreach (Collider2D item in items)
            {
                float distance =
                    Vector2.Distance(
                        transform.position,
                        item.transform.position
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetItem = item.transform;
                }
            }
        }

        void CollectTargetItem()
        {
            if (targetItem == null)
                return;

            targetItem.SendMessage("Collect", SendMessageOptions.DontRequireReceiver);

            //Destroy(targetItem.gameObject);

            targetItem = null;

            rb.linearVelocity = Vector2.zero;

        }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, destination);
        Gizmos.DrawLine(transform.position, destinationToMouse);

        Gizmos.DrawWireSphere(transform.position, itemDetectRange);
        Gizmos.DrawWireSphere(transform.position, itemCollectDistance);

    }
}