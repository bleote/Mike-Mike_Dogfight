using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemy;
    [SerializeField] private float speed;
    Vector3 newPos;
    private float enemyHp;

    [SerializeField] private GameObject[] enemyBulletPrefabs;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTimer;
    [SerializeField] private float receiveDmgMultiplier;
    private float shootTimer = 0;

    private SpriteRenderer enemySpriteRenderer;
    private Color originalColor = Color.white;
    [SerializeField] private GameObject explosionPrefab;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI enemiesDownText;

    private void Awake()
    {
        enemyHp = (2 * EnemySpawner.enemyFighter - 10) + (EnemySpawner.enemyLevel * 10);
        enemy = GetComponent<Rigidbody2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        enemiesDownText = GameObject.FindGameObjectWithTag("EnemiesDownText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > reloadTimer) 
        {
            Vector2 bulletSpawnPos = enemy.position + new Vector2(0.516f, -0.3f);
            GameObject newBullet = Instantiate(enemyBulletPrefabs[EnemySpawner.enemyLevel - 1], bulletSpawnPos, Quaternion.identity);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(new Vector2(0, bulletSpeed * -1), ForceMode2D.Impulse);
            shootTimer = 0;
        }

        if (enemyHp < 1)
        {
            EnemyDeath();
        }

        if (!PlayerController.gameOn)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        CheckEnemySpeed();

        enemy.MovePosition(newPos);

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    private void CheckEnemySpeed()
    {
        if (PlayerController.extraHard >= 2)
        {
            newPos = transform.position + (speed + (PlayerController.extraHard/2.5f)) * Time.deltaTime * Vector3.down;
        }
        else
        {
            newPos = transform.position + speed * Time.deltaTime * Vector3.down;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.CompareTag("PlayerBullet"))
        {
            ReceiveDmg(receiveDmgMultiplier * GameManager.playerArtillery);
            GameManager.hitShots++;
        }

        SFXController.PlaySound("EnemyDMG");
        Destroy(otherGO);
    }

    private void ReceiveDmg(float bulletDmg)
    {
        StartCoroutine(FlashSpriteToRed(0.05f));

        enemyHp -= bulletDmg;
    }

    private IEnumerator FlashSpriteToRed(float duration)
    {
        enemySpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        enemySpriteRenderer.color = originalColor;
    }

    private void EnemyDeath()
    {
        int randomNumber = Random.Range(1, 4);
        SFXController.ExplosionSound(randomNumber);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        EnemySpawner.destroyedPlanes++;
        GameManager.totalEnemiesDown++;
        GameManager.totalScore += (25 * EnemySpawner.enemyFighter) * EnemySpawner.enemyLevel;
        scoreText.text = GameManager.totalScore.ToString();
        enemiesDownText.text = GameManager.totalEnemiesDown.ToString();
    }
}