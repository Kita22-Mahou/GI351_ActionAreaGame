using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    [Header("Movement")]
    public float normalSpeed = 2f;
    public float boostSpeed = 5f;

    [Header("Overheat")]
    public float maxOverheat = 100f;
    public float currentOverheat = 0f;

    public float overheatRate = 30f;
    public float coolDownRate = 20f;

    private bool isBoosting = false;
    private bool isOverheated = false;

    [Header("Call")]

    public float callSpeed = 7f;
    public float callStopDistance = 1.5f;

    private bool isCalling = false;

    public Transform player;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHP = maxHP;
    }


    void Update()
    {
        HandleBoost();
        HandleOverheat();

        if (Keyboard.current != null &&Keyboard.current.qKey.wasPressedThisFrame)
        {
            Call();
        }
    }


    void FixedUpdate()
    {
        if (isCalling)
        {
            MoveToPlayer();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        if (currentHP <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float hpPercent = currentHP / maxHP;

        float hpSpeedMultiplier = Mathf.Lerp(0.5f, 1f ,hpPercent);


        float currentSpeed = normalSpeed;

        if (isBoosting && !isOverheated)
        {
            currentSpeed = boostSpeed * hpSpeedMultiplier;
        }

        transform.Translate(
            Vector2.right * currentSpeed * Time.fixedDeltaTime
        );
    }

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

    void Call()
    {
        isCalling = true;

        Debug.Log("Call");
    }

    void MoveToPlayer()
    {
        if (player == null)
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (distance <= callStopDistance)
        {
            rb.linearVelocity = Vector2.zero;

            isCalling = false;

            Debug.Log("Comenaaaa");

            return;
        }


        Vector2 direction =
            ((Vector2)player.position -
             (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * callSpeed;
    }


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
}