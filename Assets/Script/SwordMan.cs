using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SwordMan : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5;
    public int facingDirection = 1;

    [Header("HP")]
    public float maxHP = 100f;
    public float currentHP = 0f;

    [Header("Attack")]

    public Transform attackPoint;

    public float attackRange = 1.2f;

    public float attackDamage = 20f;

    public LayerMask enemyLayer;

    public float attackCooldown = 0.5f;

    private bool canAttack = true;

    [Header("Dash")]

    public float dashSpeed = 12f;

    public float dashDuration = 0.15f;

    public float dashCooldown = 0.5f;

    private bool isDashing = false;

    private bool canDash = true;

    [Header("Skill")]

    public float skillRadius = 2.5f;

    public float skillDamage = 40f;

    public float skillCooldown = 5f;

    private bool canSkill = true;

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            Dash();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Sikll();
        }
        

    }

    void FixedUpdate()
    {
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

        if (horizontal > 0 &&
            transform.localScale.x < 0)
        {
            Flip();
        }

        else if (horizontal < 0 &&
                 transform.localScale.x > 0)
        {
            Flip();
        }

        rb.linearVelocity = movement * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,transform.localScale.z);
    }


    public void Attack()
    {
        if (!canAttack)
            return;

        if (isDashing)
            return;

        canAttack = false;

        Debug.Log("Player Attack!");

        if (attackPoint != null)
        {
            Collider2D[] enemies =
                Physics2D.OverlapCircleAll(
                    attackPoint.position,
                    attackRange,
                    enemyLayer
                );


            foreach (Collider2D enemy in enemies)
            {
                enemy.SendMessage(
                    "TakeDamage",
                    attackDamage,
                    SendMessageOptions.DontRequireReceiver
                );

                Debug.Log("Hit Enemy!");
            }
        }

        StartCoroutine(AttackCooldown());
    }



    IEnumerator AttackCooldown()
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

        rb.linearVelocity = new Vector2(facingDirection * dashSpeed,0f);


        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity =Vector2.zero;


        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);


        canDash = true;
    }

    public void Sikll()
    {
        if (!canSkill)
            return;

        if (isDashing)
            return;


        canSkill = false;

        Debug.Log("Player Skill!");

        Collider2D[] enemies =
    Physics2D.OverlapCircleAll(
        transform.position,
        skillRadius,
        enemyLayer
    );


        foreach (Collider2D enemy in enemies)
        {
            enemy.SendMessage("TakeDamage",skillDamage,SendMessageOptions.DontRequireReceiver);


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
        if(attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        }

        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }

}