using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UIElements;

public class SwordMan : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private GameObject characterSprite;
    [SerializeField] private float speed = 5;
    [SerializeField] private int faceDirection = 1;
    private Rigidbody2D rb;

    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    [Header("Attack")]
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool isAttacking = false;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool canDash = true;

    [Header("Skill")]
    [SerializeField] private GameObject skillHitbox;

    [SerializeField] private float skillRadius = 1.5f;
    [SerializeField] private float skillDamage = 40f;
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillDuration = 0.1f;

    [SerializeField] private bool canSkill = true;
    [SerializeField] private bool isSkilling = false;

    [Header("Referent")]
    public static SwordMan instance;
    public GameObject sword;

    #region Event System
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
        sword.SetActive(false);

        skillRadius = skillHitbox.GetComponent<CircleCollider2D>().radius;

    }

    private void Update()
    {
        //Debug.Log(faceDirection);
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed)  // make FaceDetect not return 0
        {
            faceDirection = FaceDetect();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AttackDirection();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            Dash();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Skill();
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Call();
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            CallToMouse();
        }
    }

    void FixedUpdate()
    {
        Move();
        Flip();
    }
    #endregion

    #region Movement

    void Move()
    {
        if(isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isDashing)
            return;


        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }

        Vector2 movement = new Vector2(horizontal, vertical);
        movement = movement.normalized;

        rb.linearVelocity = movement * speed;
    }

    void Flip()
    {
        if (isAttacking)
            return;

        if (isDashing)
            return;

        float horizontal = this.rb.linearVelocityX;
        Transform spriteTransform = characterSprite.transform;

        if (horizontal > 0 &&
            spriteTransform.localScale.x < 0)
        {
            spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
        }

        else if (horizontal < 0 &&
                 spriteTransform.localScale.x > 0)
        {
            spriteTransform.localScale = new Vector3(spriteTransform.localScale.x * -1, spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }

    public int FaceDetect()
    {
        float horizontal = rb.linearVelocityX;
        float vertical = rb.linearVelocityY;

        if (horizontal == 0 && vertical > 0) // Face Up
        {
            return 1;
        }
        if (horizontal > 0 && vertical > 0) // Face Right Up
        {
            return 2;
        }
        if (horizontal > 0 && vertical == 0) // Face Right
        {
            return 3;
        }
        if (horizontal > 0 && vertical < 0) // Face Right Down
        {
            return 4;
        }
        if (horizontal == 0 && vertical < 0) // Face Down
        {
            return 5;
        }
        if (horizontal < 0 && vertical < 0) // Face Left Down
        {
            return 6;
        }
        if (horizontal < 0 && vertical == 0) // Face Left 
        {
            return 7;
        }
        if (horizontal < 0 && vertical > 0) // Face Left Up
        {
            return 8;
        }

        return 0;
    }
    #endregion

    #region Abilities
    void AttackDirection()
    {
        if (isDashing)
            return;

        if (isSkilling)
            return;

        int direction = faceDirection;

        if (direction == 1) // Up
        {
            Attack(
                new Vector2(0, 1.25f),
                Quaternion.Euler(0, 0, 90));
        }
        else if (direction == 2) // Right Up
        {
            Attack(
                new Vector2(0.85f, 0.85f),
                Quaternion.Euler(0, 0, 45));
        }
        else if (direction == 3) // Right
        {
            Attack(
                new Vector2(1.25f, 0),
                Quaternion.Euler(0, 0, 0));
        }
        else if (direction == 4) // Right Down
        {
            Attack(
                new Vector2(0.85f, -0.85f),
                Quaternion.Euler(0, 0, -45));
        }
        else if (direction == 5) // Down
        {
            Attack(
                new Vector2(0, -1.25f),
                Quaternion.Euler(0, 0, 90));
        }
        else if (direction == 6) // Left Down
        {
            Attack(
                new Vector2(-0.85f, -0.85f),
                Quaternion.Euler(0, 0, 45));
        }
        else if (direction == 7) // Left
        {
            Attack(
                new Vector2(-1.25f, 0),
                Quaternion.Euler(0, 0, 0));
        }
        else if (direction == 8) // Left Up
        {
            Attack(
                new Vector2(-0.85f, 0.85f),
                Quaternion.Euler(0, 0, -45));
        }

    }

    void Attack( Vector2 pos, Quaternion rot) // ให้ Hitbox ย้ายจุดไปรอบๆ
    {
        isAttacking = true;
        swordHitbox.transform.position = (Vector2)transform.position + pos;
        swordHitbox.transform.rotation = rot;
        swordHitbox.SetActive(true);
        StartCoroutine(DisableHitbox(0));
    }

    IEnumerator DisableHitbox(int hitboxNumber)
    {
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        swordHitbox.SetActive(false);
    }

    public void Dash()
    {
        if (isAttacking)
            return;

        if (!canDash)
            return;

        float horizontal = rb.linearVelocityX;
        float vertical = rb.linearVelocityY;

        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }

        Vector2 movement = new Vector2(horizontal, vertical);
        movement = movement.normalized;

        rb.linearVelocity = movement * dashSpeed;
        isDashing = true;
        canDash = false;

        StartCoroutine(DashCoroutine());

        Debug.Log("Player Dash!");
    }

    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    public void Skill()
    {
        if (!canSkill)
            return;

        if (isAttacking)
            return;

        if (isDashing)
            return;


        canSkill = false;

        Debug.Log("Player Skill!");

        skillHitbox.SetActive(true);

        Collider2D[] enemies =
    Physics2D.OverlapCircleAll(
        transform.position,
        skillRadius,
        enemyLayer );

        foreach (Collider2D enemy in enemies)
        {
            enemy.SendMessage("TakeDamage", skillDamage, SendMessageOptions.DontRequireReceiver);

            Debug.Log("Skill Hit Enemy!");
        }
        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillCooldown()
    {
        yield return new WaitForSeconds(skillDuration);

        skillHitbox.SetActive(true);

        yield return new WaitForSeconds(skillCooldown);

        canSkill = true;

        Debug.Log("Skill Ready!");
    }

    void Call() // Call cart
    {
        Debug.Log("Call");
        Car.Instance.MakeCall(this.transform);
    }

    void CallToMouse()
    {
        Debug.Log("Call to mouse");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Car.Instance.MakeCallToMouse((Vector2)mousePosition);

        Debug.Log(mousePosition);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }

}