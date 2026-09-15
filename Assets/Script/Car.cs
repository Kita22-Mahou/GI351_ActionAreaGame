using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

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
    [SerializeField] private Vector2 destination;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float callStopDistance = 1.5f;

    [SerializeField] private bool isCalling = false;

    [SerializeField] private Transform player;

    [Header("Referent")]
    public static Car Instance;

    #region Event System
    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
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
        }
    }
    #endregion

    #region Movement
    void HandleBoost()
    {
        isBoosting = Keyboard.current.fKey.isPressed;

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

            currentOverheat = Mathf.Clamp(currentOverheat, 0f ,maxOverheat);

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

    public void MakeCall(Transform target)
    {
        isCalling = true;
        destination = target.position;
    }

    void CallMove()
    {
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

        Vector2 direction =
           ((Vector2)destination -
            (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * normalSpeed;
    }
    #endregion


    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        Debug.Log("HP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;

            Destroyed();
        }
    }

    void Destroyed()
    {
        Debug.Log("DESTROYED!");

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, destination);
    }
}