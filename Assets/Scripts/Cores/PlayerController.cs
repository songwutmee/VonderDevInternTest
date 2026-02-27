using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float apCostPerShot = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float horizontal;
    private bool facingRight = true;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        // Movement & Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
        
        // Shoots towards mouse position
        if (Input.GetMouseButtonDown(0)) HandleAttack();

        UpdateAnimator();
        HandleFlipping();
    }

    private void HandleAttack()
{
    if (PlayerStatus.Instance != null && PlayerStatus.Instance.UseAP(apCostPerShot))
    {
        anim.SetTrigger("Attack");

        // Use the distance between camera and world plane (usually -10 to 0)
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; 
        
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0; // Flatten to 2D plane

        // Calculate direction from shooting point to mouse
        Vector3 shootDir = (worldMousePos - shootPoint.position).normalized;

        // Instantiate and initialize the projectile
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile projectileComponent = proj.GetComponent<Projectile>();
        
        if (projectileComponent != null)
        {
            projectileComponent.Setup(shootDir);
        }
    }
}

    private void HandleFlipping()
    {
        if (horizontal > 0 && !facingRight) Flip();
        else if (horizontal < 0 && facingRight) Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("Speed", Mathf.Abs(horizontal));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("AirSpeedY", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (!isDead) rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    public void PlayHurtAnimation() => anim.SetTrigger("Hurt");

    public void PlayDeathAnimation()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("Death", true); 
    }

    public void Revive()
    {
        isDead = false;
        anim.SetBool("Death", false);
    }

    private void OnCollisionStay2D(Collision2D c) { if (c.gameObject.CompareTag("Platform")) isGrounded = true; }
    private void OnCollisionExit2D(Collision2D c) { if (c.gameObject.CompareTag("Platform")) isGrounded = false; }
}