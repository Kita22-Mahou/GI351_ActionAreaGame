using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordMan : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5;
    public int facingDirection = 1;
    public float maxHP = 100f;
    public float currentHP;

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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal > 0 && transform.transform.localScale.x < 0 
            || horizontal < 0 && transform.localScale.x > 0)
        {

            Flip();
        }

        rb.linearVelocity = new Vector2(horizontal , vertical) * speed ;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,transform.localScale.z);
    }


    public void Attack()
    {
        Debug.Log("Player Attack!");
    }
    public void Dash()
    {
        Debug.Log("Player Dash!");
    }
    public void Sikll()
    {
        Debug.Log("Player Skill!");
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        Debug.Log("PlayerHP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;

            PlayerDead();
        }


        if (currentHP <= 0)
        {
            PlayerDead();
        }
    }

    void PlayerDead()
    {
        Debug.Log("Player Dead");

    }

}