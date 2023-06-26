using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifetime;
    private Rigidbody2D rb;

    public bool isExplosiveBullet = false;
    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (isExplosiveBullet)
            {
                Instantiate(explosionPrefab, rb.position, Quaternion.identity);
            }
            // Debug.Log("hit enemy!");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}