using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 11f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float horizontal;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Attack Input
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetFloat("AirSpeedY", rb.velocity.y);
        }

        FlipLogic();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (anim != null) anim.SetTrigger("Jump"); 
    }

    private void Attack()
    {
        if (anim != null) anim.SetTrigger("Attack"); 
    }

    public void TakeDamage()
    {
        // This only plays the animation for now. 
        if (anim != null) anim.SetTrigger("Hurt");
    }

    private void FlipLogic()
    {
        if (horizontal > 0 && !facingRight) Flip();
        else if (horizontal < 0 && facingRight) Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) isGrounded = false;
    }
}