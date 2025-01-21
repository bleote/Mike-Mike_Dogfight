using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] squadPrefabs;

    [Header("Wave Alert")]
    [SerializeField] private TMP_Text waveAlertText;

    [Header("Spawn Ranges")]
    [SerializeField] private RectTransform enemy1SpawnLeft;
    [SerializeField] private RectTransform enemy1SpawnRight;
    [SerializeField] private RectTransform enemy2SpawnLeft;
    [SerializeField] private RectTransform enemy2SpawnRight;
    [SerializeField] private RectTransform squad1SpawnLeft;
    [SerializeField] private RectTransform squad1SpawnRight;
    [SerializeField] private RectTransform squad2SpawnLeft;
    [SerializeField] private RectTransform squad2SpawnRight;
    private float enemy1Left;
    private float enemy1Right;
    private float enemy2Left;
    private float enemy2Right;
    private float squad1Left;
    private float squad1Right;
    private float squad2Left;
    private float squad2Right;

    [Header("Interstitial Ads")]
    [SerializeField] private Image blackOut;

    private float spawnTimer;
    public static float spawnCD = 1.3f;
    private bool dropSquad = true;
    private bool holdEnemySpawn = false;
    public static int destroyedPlanes = 0;
    public static int enemyFighter = 1;
    public static int enemyLevel = 1;
    public static int enemyWave = 1;

    private bool waveInterstitialAd = false;

    private bool waveSwitchTracker = true;

    InterstitialAdsManager interstitialAdsManager;
    UnityAnalyticsManager unityAnalyticsManager;

    void Start()
    {
        AdjustSpawnRange();
        interstitialAdsManager = FindAnyObjectByType<InterstitialAdsManager>();
        unityAnalyticsManager = FindObjectOfType<UnityAnalyticsManager>();
    }

    private void Update()
    {
        if (PlayerController.gameOn)
        {
            if (!holdEnemySpawn)
            {
                if (destroyedPlanes > 19)
                {
                    if (enemyFighter == 3)
                    {
                        holdEnemySpawn = true;
                        StartCoroutine(WaveConclusionAlert());
                    }
                    else if (enemyFighter == 6)
                    {
                        holdEnemySpawn = true;
                        StartCoroutine(WaveConclusionAlert());
                    }
                    else if (enemyFighter == 9)
                    {
                        holdEnemySpawn = true;
                        StartCoroutine(WaveConclusionAlert());
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
                    if (destroyedPlanes >= 16 && dropSquad == true)
                    {
                        SpawnSquad1(enemyFighter);
                        SpawnSquad2(enemyFighter);
                        dropSquad = false;
                    }
                    else
                    {
                        SpawnEnemy1(enemyFighter);
                        StartCoroutine(SpawnEnemy2(enemyFighter));
                    }
                }
            }
        }
    }

    private void AdjustSpawnRange()
    {
        enemy1Left = enemy1SpawnLeft.position.x;
        enemy1Right = enemy1SpawnRight.position.x;
        enemy2Left = enemy2SpawnLeft.position.x;
        enemy2Right = enemy2SpawnRight.position.x;
        squad1Left = squad1SpawnLeft.position.x;
        squad1Right = squad1SpawnRight.position.x;
        squad2Left = squad2SpawnLeft.position.x;
        squad2Right = squad2SpawnRight.position.x;
    }

    private IEnumerator WaveConclusionAlert()
    {
        yield return new WaitForSeconds(6.5f);

        waveAlertText.text = $"You Survived Enemy Wave {enemyWave}";
        SFXController.PlaySound("SurvivedWaveSound");

        enemyWave++;
        GameManager.wavesSurvived++;
        spawnCD -= 0.05f;
        
        SendTrackSurvivedWave();

        yield return new WaitForSeconds(3);

        waveAlertText.text = string.Empty;

        if (enemyFighter == 3)
        {
            enemyFighter++;
        }
        else if (enemyFighter == 6) 
        {
            enemyFighter++;
        }
        else
        {
            enemyFighter = 1;

            if (enemyLevel < 5)
            {
                enemyLevel++;
            }
        }

        yield return new WaitForSeconds(1);

        CheckForWaveInsterstitialAd();
    }

    private void CheckForWaveInsterstitialAd()
    {
        if (GameManager.hasNetwork)
        {
            if (enemyWave == 3 || enemyWave == 5 || enemyWave == 7 || enemyWave == 9 || enemyWave == 11 || enemyWave == 13)
            {
                waveInterstitialAd = true;
            }
            else
            {
                waveInterstitialAd = false;
            }

            if (waveInterstitialAd)
            {
                StartCoroutine(InterstitialAdBreakStart());
            }
            else
            {
                holdEnemySpawn = false;
            }
        }
        else
        {
            holdEnemySpawn = false;
        }
    }

    private IEnumerator InterstitialAdBreakStart()
    {
        blackOut.gameObject.SetActive(true);
        blackOut.color = new Color(0, 0, 0, 0);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < 0.25f)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / 0.25f);
            blackOut.color = new Color(0, 0, 0, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        blackOut.color = new Color(0, 0, 0, 1);


        interstitialAdsManager.interstitialAdBreak = true;
        interstitialAdsManager.ShowAd();
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public IEnumerator InterstitialAdBreakEnd()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
        
        blackOut.color = new Color(0, 0, 0, 1);
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < 0.25f)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / 0.25f);
            blackOut.color = new Color(0, 0, 0, alpha);

            yield return null;

            elapsedTime = Time.time - startTime;
        }

        blackOut.color = new Color(0, 0, 0, 0);
        blackOut.gameObject.SetActive(false);

        interstitialAdsManager.interstitialAdBreak = false;
        holdEnemySpawn = false;
    }

    private void SpawnEnemy1(int enemyFighter)
    {
        float spawnPositionX = Random.Range(enemy1Left, enemy1Right);
        Vector3 spawnPos = new(spawnPositionX, 5.5f, 0);
        Instantiate(enemyPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);

        spawnTimer = 0;
    }

    private IEnumerator SpawnEnemy2(int enemyFighter)
    {
        if (enemyWave <= 5)
        {
            yield return new WaitForSeconds(0.65f);
        }
        else if (enemyWave > 5 && enemyWave <= 10)
        {
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        float spawnPositionX = Random.Range(enemy2Left, enemy2Right);
        Vector3 spawnPos = new(spawnPositionX, 5.5f, 0);
        Instantiate(enemyPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);

        spawnTimer = 0;
    }

    private void SpawnSquad1(int enemyFighter)
    {
        Vector3 spawnPos = new(Random.Range(squad1Left, squad1Right), 5.5f, 0);
        Instantiate(squadPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);
        spawnTimer = 0;
    }

    private void SpawnSquad2(int enemyFighter)
    {
        Vector3 spawnPos = new(Random.Range(squad2Left, squad2Right), 5.5f, 0);
        Instantiate(squadPrefabs[enemyFighter - 1], spawnPos, Quaternion.identity);
        spawnTimer = 0;
    }

    private void SendTrackSurvivedWave()
    {
        if (enemyWave == 2 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 3 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 4 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 5 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 6 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 7 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 8 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 9 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 10 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 11 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 12 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 13 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 14 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 15 && !waveSwitchTracker)
        {
            waveSwitchTracker = true;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }
        else if (enemyWave == 16 && waveSwitchTracker)
        {
            waveSwitchTracker = false;
            unityAnalyticsManager.TrackSurvivedWave(enemyWave - 1);
        }

    }
}
