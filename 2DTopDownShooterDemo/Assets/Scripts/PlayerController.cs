using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int health;
    public int experience;
    public Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 mousePosition;
    private Camera cam;

    // Bullet parameters
    public float bulletScaleX = 1f;
    public float bulletScaleY = 1f;
    public float bulletSpeed;

    public int bulletDamage;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate;
    public float bulletSpreadFactor;
    private float bulletSpread;
    private float nextFireTime;
    float recoilForce = 5f;

    // level and exp
    public int level;
    public int experienceToNextLevel;

    // Ѫ��prefab
    public GameObject bloodSplatPrefab;
    // ����prefab
    public GameObject gunSmokePrefab;

    SpriteRenderer spriteRenderer;
    Color originalColor;

    public LevelManager levelManager;
    public GameEffectManager effectManager;

    void Start()
    {
        cam = Camera.main;
        level = 1;
        health = 100;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        bulletDamage = 10;
        bulletSpeed = 20f;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movement = new Vector2(horizontal, vertical).normalized;
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Shoot()
    {
        StartCoroutine(effectManager.ShakeCamera());

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ProjectileController bc = bullet.GetComponent<ProjectileController>();

        GameObject smoke = Instantiate(gunSmokePrefab, firePoint.position, firePoint.rotation);

        // �������������25%�ļ������ɱ�ը�ӵ�
        float randomValue = Random.value;
        if (randomValue < 0.3f)
        {
            bc.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            bc.isExplosiveBullet = true;
            bc.damage = 3 * bulletDamage;
        }
        else
        {
            bc.damage = bulletDamage;
        }

        // �ӵ��ߴ���ȼ�����
        ApplyBulletScale(bullet, bc);
        // ����ӵ�ɢ��Ч��
        ApplyBulletSpread(bullet);
        // ��Ӻ�����Ч��
        ApplyRecoil();
    }

    void ApplyBulletScale(GameObject bullet, ProjectileController bc)
    {
        float scaleX = Mathf.Clamp(bulletScaleX, 1f, 5f);
        float scaleY = Mathf.Clamp(bulletScaleY, 1f, 5f);
        bullet.transform.localScale = new Vector3(scaleX, scaleY, 1);
        bc.speed = bulletSpeed;
    }

    void ApplyBulletSpread(GameObject bullet)
    {
        bulletSpread = Mathf.Clamp(bulletSpreadFactor * fireRate, 0, 50f);

        float randomOffsetX = Random.Range(-bulletSpread, bulletSpread);
        float randomOffsetY = Random.Range(-bulletSpread, bulletSpread);
        bullet.transform.Rotate(randomOffsetX, randomOffsetY, 0);
    }

    void ApplyRecoil()
    {
        Vector2 recoilDirection = (rb.position - (Vector2)firePoint.position).normalized;
        rb.AddForce(recoilDirection * 0.1f * bulletDamage * recoilForce, ForceMode2D.Impulse);
    }

    public void AddExperience(int exp)
    {
        experience += exp;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        health += 10;
        experience = 0;
        experienceToNextLevel += level * 10;
        bulletDamage += 2 * level;
        fireRate += 1f;
        speed += 1f;
        // �ӵ��ߴ���ȼ�����
        bulletScaleX = 1.2f * bulletScaleX;
        bulletScaleY = 1.2f * bulletScaleY;
        bulletSpeed = 1.1f * bulletSpeed;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashCoroutine());
        // ��ѪЧ��
        GameObject bloodSplat = Instantiate(bloodSplatPrefab, gameObject.transform.position, Quaternion.identity);

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

    void Die()
    {
        // TODO:����������������Ч��

        levelManager.GameOver();
    }
}