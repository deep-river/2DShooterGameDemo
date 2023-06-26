using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public int health;
    public int damage;
    public int experience;
    public GameObject player;
    private Rigidbody2D rb;

    public GameObject bloodSplatPrefab;

    SpriteRenderer spriteRenderer;
    Color originalColor;

    // Vector2 originalPosition;
    float shakeAmplitude = 0.1f;  // 被击中抖动幅度
    int shakeTimes = 3;           // 被击中抖动次数 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        Vector2 target = player.transform.position;
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashCoroutine());
        StartCoroutine(ShakeCoroutine());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        spriteRenderer.color = originalColor;
    }

    IEnumerator ShakeCoroutine()
    {
        Vector2 originalPosition = rb.position;
        // 循环抖动次数
        for (int i = 0; i < shakeTimes; i++)
        {
            // 随机生成-shakeAmplitude到shakeAmplitude之间的偏移量
            float xOffset = Random.Range(-shakeAmplitude, shakeAmplitude);
            float yOffset = Random.Range(-shakeAmplitude, shakeAmplitude);

            // 应用偏移量移动敌人
            rb.transform.position += new Vector3(xOffset, yOffset, 0);

            // 等待0.05秒
            yield return new WaitForSeconds(0.05f);
        }

        // 恢复敌人位置
        rb.transform.position = originalPosition;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerController player = hitInfo.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void Die()
    {
        // 喷血效果
        Instantiate(bloodSplatPrefab, rb.position, Quaternion.identity);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.AddExperience(experience);

        Destroy(gameObject, 0.1f);
    }
}