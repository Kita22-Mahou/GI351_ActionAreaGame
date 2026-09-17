using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SwordMan : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5;
    [SerializeField] private int facingDirection = 1;
    private Rigidbody2D rb;

    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    [Header("Attack")]
    [SerializeField] private bool isAttacking;

    [SerializeField] private int faceDirection;

    [SerializeField] private GameObject[] swordHitbox;

    [SerializeField] private float attackCooldown = 0.5f;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private bool canAttack = true;

    private float attackTimer = 0f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;

    [SerializeField] private float dashDuration = 0.15f;

    [SerializeField] private float dashCooldown = 0.5f;

    [SerializeField] private bool isDashing = false;

    [SerializeField] private bool canDash = true;

    [Header("Skill")]
    [SerializeField] private float skillRadius = 2.5f;

    [SerializeField] private float skillDamage = 40f;

    [SerializeField] private float skillCooldown = 5f;

    [SerializeField] private bool canSkill = true;

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

    }

    private void Update()
    {
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed)  // if walking -> detect
        {
            faceDirection = FaceDetect();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack2();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
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

        //if (attackTimer > 0)
        //{
        //    attackTimer -= Time.deltaTime;

        //    if (attackTimer <= 0)
        //    {
        //        HideSword();
        //    }
        //}
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            Move();
            Flip();
        }
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    #endregion

    #region Movement

    void Move()
    {
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
        float horizontal = this.rb.linearVelocityX;

        if (horizontal > 0 &&
            transform.localScale.x < 0)
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        else if (horizontal < 0 &&
                 transform.localScale.x > 0)
        {
            facingDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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

        return 0;
    }
    #endregion

    #region Abilities
    void Attack2()
    {
        int direction = faceDirection;

        if (direction == 1)
        {
            isAttacking = true;
            swordHitbox[0].SetActive(true);
            StartCoroutine(DisableHitbox(0));
        }
        else if (direction == 2)
        {
            isAttacking = true;
            swordHitbox[1].SetActive(true);
            StartCoroutine(DisableHitbox(1));
        }
        else if (direction == 3)
        {
            isAttacking = true;
            swordHitbox[2].SetActive(true);
            StartCoroutine(DisableHitbox(2));
        }
        else if (direction == 4)
        {
            isAttacking = true;
            swordHitbox[3].SetActive(true);
            StartCoroutine(DisableHitbox(3));
        }
        else if (direction == 5)
        {
            isAttacking = true;
            swordHitbox[4].SetActive(true);
            StartCoroutine(DisableHitbox(4));
        }

    }

    IEnumerator DisableHitbox(int hitboxNumber)
    {
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        swordHitbox[hitboxNumber].SetActive(false);
    }

    //public void Attack()
    //{
    //    if (isDashing)
    //        return;

    //    if (attackTimer > 0)
    //        return;

    //    sword.SetActive(true);

    //    attackTimer = 0.25f;
    //    attackcooldown();
    //}

    IEnumerator attackcooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }


    public void Dash()
    {

        if (!canDash)
            return;

        if (isDashing)
            return;


        StartCoroutine(DashCoroutine());

        Debug.Log("Player Dash!");
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;

        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);


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

        if (isDashing)
            return;


        canSkill = false;

        Debug.Log("Player Skill!");

        sword.SetActive(true);

        attackTimer = attackCooldown;

        Collider2D[] enemies =
    Physics2D.OverlapCircleAll(
        transform.position,
        skillRadius,
        enemyLayer
    );


        foreach (Collider2D enemy in enemies)
        {
            enemy.SendMessage("TakeDamage", skillDamage, SendMessageOptions.DontRequireReceiver);


            Debug.Log("Skill Hit Enemy!");
        }
        StartCoroutine(SkillCooldown());
    }

    IEnumerator SkillCooldown()
    {
        yield return new WaitForSeconds(skillCooldown);

        canSkill = true;

        Debug.Log("Skill Ready!");
    }

    void Call()
    {
        Debug.Log("Call");
        Car.Instance.MakeCall(GetCallPosition());
    }

    public Transform GetCallPosition()
    {
        return this.transform;
    }

    void HideSword()
    {
        if (sword != null)
        {
            sword.SetActive(false);
        }
    }

    #endregion

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        Debug.Log("PlayerHP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;

            PlayerDead();
        }
    }

    void PlayerDead()
    {
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Player Dead");

        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }

}