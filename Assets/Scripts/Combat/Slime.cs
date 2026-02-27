using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IDamageable
{
    [Header("Movement & Range")]
    public float health = 20f;
    public float moveSpeed = 1.8f;
    public float chaseRange = 8f;
    public float attackRange = 1.1f; 
    public bool isSmallSlime = false;

    [Header("Combat Settings")]
    public float damage = 10f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime;

    [Header("References")]
    public GameObject smallSlimePrefab;
    public Collider2D combatArea;

    private Transform player;
    private Animator anim;
    private Vector3 spawnPoint;
    private bool isDead = false;
    private Vector3 initialScale;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        initialScale = transform.localScale;
        spawnPoint = transform.position;
    }

    private void Start()
    {
        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        bool playerInZone = (combatArea != null) ? combatArea.OverlapPoint(player.position) : true;
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (playerInZone && distToPlayer <= chaseRange)
        {
            if (distToPlayer <= attackRange)
            {
                PerformAttack();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            GoBackHome();
        }
    }

    private void MoveTowardsPlayer()
    {
        anim.SetBool("IsMoving", true);
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Flip sprite based on movement direction
        float flipX = dir.x > 0 ? -initialScale.x : initialScale.x;
        transform.localScale = new Vector3(flipX, initialScale.y, initialScale.z);
    }

    private void PerformAttack()
    {
        anim.SetBool("IsMoving", false);
        
        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            
            // Apply damage to player
            if (PlayerStatus.Instance != null)
            {
                PlayerStatus.Instance.TakeDamage(damage);
            }
            
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void GoBackHome()
    {
        float distToHome = Vector2.Distance(transform.position, spawnPoint);
        if (distToHome > 0.1f)
        {
            anim.SetBool("IsMoving", true);
            transform.position = Vector2.MoveTowards(transform.position, spawnPoint, moveSpeed * Time.deltaTime);
            
            float flipX = (spawnPoint.x > transform.position.x) ? -initialScale.x : initialScale.x;
            transform.localScale = new Vector3(flipX, initialScale.y, initialScale.z);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        GetComponent<SpriteFlash>()?.Flash();
        anim.SetTrigger("Hurt");

        if (health <= 0) StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        isDead = true;
        anim.SetTrigger("Die");
        
        // Wait for the animation end before splitting
        yield return new WaitForSeconds(0.8f);

        if (!isSmallSlime && smallSlimePrefab != null)
        {
            SpawnMinions();
        }
        Destroy(gameObject);
    }

    private void SpawnMinions()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject minion = Instantiate(smallSlimePrefab, transform.position + (Vector3)Random.insideUnitCircle * 0.5f, Quaternion.identity);
            Slime sScript = minion.GetComponent<Slime>();
            if (sScript != null)
            {
                sScript.health = 5f;
                sScript.isSmallSlime = true;
                sScript.combatArea = this.combatArea;
                minion.transform.localScale = initialScale * 0.5f;
            }
        }
    }
}