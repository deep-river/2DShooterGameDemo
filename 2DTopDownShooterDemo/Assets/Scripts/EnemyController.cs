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
    float shakeAmplitude = 0.1f;  // �����ж�������
    int shakeTimes = 3;           // �����ж������� 

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
        // ѭ����������
        for (int i = 0; i < shakeTimes; i++)
        {
            // �������-shakeAmplitude��shakeAmplitude֮���ƫ����
            float xOffset = Random.Range(-shakeAmplitude, shakeAmplitude);
            float yOffset = Random.Range(-shakeAmplitude, shakeAmplitude);

            // Ӧ��ƫ�����ƶ�����
            rb.transform.position += new Vector3(xOffset, yOffset, 0);

            // �ȴ�0.05��
            yield return new WaitForSeconds(0.05f);
        }

        // �ָ�����λ��
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
        // ��ѪЧ��
        Instantiate(bloodSplatPrefab, rb.position, Quaternion.identity);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.AddExperience(experience);

        Destroy(gameObject, 0.1f);
    }
}