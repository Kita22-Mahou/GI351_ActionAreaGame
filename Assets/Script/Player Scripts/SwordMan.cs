using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UIElements;

public class SwordMan : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private GameObject characterSprite;
    [SerializeField] private float speed = 5;
    [SerializeField] private int faceDirection = 2;
    private Rigidbody2D rb;

    [Header("Attack")]
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float baseAttackDamage = 20f;
    [SerializeField] private float damageBonus = 0f;
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
    [HideInInspector] public bool isInShop = false;

    [Header("Referent")]
    public static SwordMan instance;
    public GameObject sword;
    private PlayerFaceDetector playerfaceDetector;

    #region Event System
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        
        sword.SetActive(false);

        skillRadius = skillHitbox.GetComponent<CircleCollider2D>().radius;
        playerfaceDetector = GetComponent<PlayerFaceDetector>();
    }

    private void Update()
    {
        //Debug.Log(faceDirection);
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed)  // make FaceDetect not return 0
        {
            faceDirection = playerfaceDetector.faceDetectDirection;
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
    #endregion

    #region Abilities
    public void AttackDirection()
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
        swordHitbox.GetComponent<CapsuleCollider2D>().enabled = true;
        StartCoroutine(DisableHitbox());
    }

    IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        swordHitbox.SetActive(false);
        swordHitbox.GetComponent<CapsuleCollider2D>().enabled = false;
        swordHitbox.GetComponent<SwordHitBox>().HashSetClear();
    }

    public void Dash()
    {
        if (isAttacking)
            return;

        if (!canDash)
            return;

        float horizontal = rb.linearVelocityX;
        float vertical = rb.linearVelocityY;

        switch (faceDirection)
        {
            case 1: // up
                horizontal = 0;
                vertical = 1;
                break;

            case 2: // right up
                horizontal = 1;
                vertical = 1;
                break;

            case 3: // right
                horizontal = 1;
                vertical = 0;
                break;

            case 4: // right down
                horizontal = 1;
                vertical = -1;
                break;

            case 5: // down
                horizontal = 0;
                vertical = -1;
                break;

            case 6: // left down
                horizontal = -1;
                vertical = -1;
                break;

            case 7: // left
                horizontal = -1;
                vertical = 0;
                break;

            case 8: // left up
                horizontal = -1;
                vertical = 1;
                break;
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


    #region Buff
    public float GetAttackDamage()
    {
        return baseAttackDamage + damageBonus;
    }

    //public void UpgradeDamage(float amount)
    //{
        //damageBonus += amount;
        //Debug.Log("Attack Damage: " + GetAttackDamage());
    //}

    public void UpgradeSpeed(float amount)
    {
        speed += amount;
        Debug.Log("Speed: " + speed);
    }

    //public void UpgradeMaxHP(float amount)
    //{
    //    maxHP += amount;
    //    currentHP += amount;

    //    currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

    //    Debug.Log("MaxHP: " + maxHP);
    //}

    //public void Heal(float amount)
    //{
    //    currentHP += amount;
    //    currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

    //    Debug.Log("Player HP: " + currentHP);
    //}
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }

}