using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnTimer;
    public static float spawnCD = 1.3f;
    private bool dropSquad = true;
    public static int destroyedPlanes = 0;
    public static int enemyFighter = 1;
    public static int enemyLevel = 1;
    public static int enemyWave = 1;

    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private GameObject[] squadPrefabs;

    private void Update()
    {
        if (PlayerController.gameOn)
        {
            if (destroyedPlanes > 10)
            {
                if (enemyFighter == 9)
                {
                    enemyWave++;
                    enemyFighter = 1;

                    if (enemyLevel < 5)
                    {
                        enemyLevel++;
                    }
                }
                else
                {
                    enemyFighter++;
                }

                destroyedPlanes = 0;
                dropSquad = true;
            }

            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnCD)
            {
                if (destroyedPlanes >= 8 && dropSquad == true)
                {
                    SpawnSquad(enemyFighter);
                    dropSquad = false;
                }
                else
                {
                    SpawnEnemy(enemyFighter);
                }
            }
        }
    }

    private void SpawnEnemy(int enemyFighter)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.75f, 5.75f), 5.5f, 0);
        Instantiate(enemyPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);
        spawnTimer = 0;
    }

    private void SpawnSquad(int enemyFighter)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.25f, 5.25f), 5.5f, 0);
        Instantiate(squadPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);
        spawnTimer = 0;
    }
}
