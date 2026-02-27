using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 3f;
    private Vector3 moveDir;

    public void Setup(Vector3 direction)
    {
        moveDir = direction;
        
        // Face the flight direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for IDamageable 
        IDamageable target = other.GetComponentInParent<IDamageable>();
        
        if (target != null && !other.CompareTag("Player"))
        {
            // Random 3-5 damage per hit
            float damage = Random.Range(3f, 6f); 
            target.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
}