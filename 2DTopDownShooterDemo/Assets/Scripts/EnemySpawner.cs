using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float basicSpawnInterval = 5f;
    private float spawnInterval = 5f;

    public GameObject player;
    public PlayerController pc;

    private float enemyBasicSpeed = 1.8f;
    private int enemyBasicHealth = 10;
    private int enemyBasicDamage = 15;
    private int enemyBasicExperience = 25;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            // 敌人数值随玩家等级提升
            ec.speed = enemyBasicSpeed + 0.2f * pc.level;
            ec.health = enemyBasicHealth + 20 * pc.level;
            ec.damage = enemyBasicDamage + 2 * pc.level;
            ec.experience = enemyBasicExperience;

            spawnInterval = Mathf.Clamp(basicSpawnInterval - 0.25f * pc.level, 0.5f, 5f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}