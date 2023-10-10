using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player;
    private SpriteRenderer playerSpriteRenderer;
    private Color originalColor = Color.white;
    private float speed = 7.5f + (0.5f * UIManager.playerAircraft);

    [SerializeField]
    private Sprite[] playerSprites;

    [SerializeField]
    private ArmorBar armorBar;

    private float fuelDemand;
    [SerializeField]
    private FuelBar fuelBar;

    [SerializeField]
    private GameObject[] bulletPrefabs;
    private float bulletSpeed = 10;
    private float shootTimer = 0;
    private float reloadTimer;

    private GameObject planeSmoke;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject rpmDestroyed;

    [SerializeField]
    private AudioSource speedUpSound;
    private bool speedDownSoundPlaying = false;
    [SerializeField]
    private AudioSource speedDownSound;
    private bool speedUpSoundPlaying = false;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI aircraftText;
    [SerializeField]
    private TextMeshProUGUI artilleryText;
    [SerializeField]
    private TextMeshProUGUI enemiesDownText;
    [SerializeField]
    private TextMeshProUGUI rankText;
    [SerializeField]
    private TextMeshProUGUI alertText;

    private int playerAircraft = UIManager.playerAircraft;
    private int playerArtillery = UIManager.playerArtillery;

    private int extraHard = 1;

    [SerializeField]
    private GameObject[] ranks;
    private int rankUpSound = 1;

    [SerializeField]
    private GameObject[] awardSlotObjects;
    [SerializeField]
    private SpriteRenderer[] awardSlotSpriteRenderer;
    [SerializeField]
    private Sprite[] awardSprites;

    private float enduranceTimer = 0;
    private float madDogTimer = 0;
    private float backlinerTimer = 0;

    public static bool gameOn = false;
    private bool goSoundPlay = false;
    [SerializeField]
    private GameObject wallBottom;
    [SerializeField]
    private GameObject getReadyText;

    public static bool pause = false;
    [SerializeField]
    private GameObject pauseText;

    [SerializeField]
    private GameObject finalPanel;
    [SerializeField]
    private TextMeshProUGUI[] finalTexts;

    void Awake()
    {
        UIManager.totalArmor = 1f;
        UIManager.totalFuel = 1f;
        UIManager.playerAircraft = 1;
        UIManager.playerArtillery = 1;
        UIManager.totalScore = 0;
        UIManager.totalEnemiesDown = 0;
        UIManager.playerRank = 1;
        UIManager.totalAwards = 0;
        UIManager.currentAwardIndex = 0;
        UIManager.planeCrashes = 0;
        UIManager.totalSupplies = 0;
        UIManager.totalRefuels = 0;
        UIManager.awardIndestructible = false;
        UIManager.awardEndurance = false;
        UIManager.awardUpComer = false;
        UIManager.awardSmartWing = false;
        UIManager.awardMadDog = false;
        UIManager.awardBackliner = false;
        UIManager.awardLastDrop = false;
        UIManager.awardSharpshooter = false;
        UIManager.awardThirstyJoe = false;
        UIManager.awardFlyEmAll = false;
        UIManager.awardArtilleryExpert = false;
        UIManager.criticalArmor = false;
        UIManager.awardPhoenix = false;
        UIManager.awardAce = false;
        UIManager.awardEliteFighter = false;
        UIManager.awardAirForceHero = false;

        EnemySpawner.spawnCD = 1.3f;
        EnemySpawner.destroyedPlanes = 0;
        EnemySpawner.enemyFighter = 1;
        EnemySpawner.enemyLevel = 1;
        EnemySpawner.enemyWave = 1;

        player = GetComponent<Rigidbody2D>();
        planeSmoke = GameObject.FindGameObjectWithTag("PlaneSmoke");
        planeSmoke.SetActive(false);
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        scoreText.text = UIManager.totalScore.ToString();
        rankText.text = "Second Lieutenant";
        CheckAircraft(playerAircraft);
        CheckArtillery(playerArtillery);
        SetReloadTimer();
        pauseText.SetActive(false);
    }

    private void Update()
    {
        if (gameOn)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > reloadTimer)
            {
                if (Input.GetButton("Fire1") && Time.timeScale != 0)
                {
                    Vector2 bulletSpawnPos = player.position + new Vector2(0.516f, 0.25f);
                    GameObject newBullet = Instantiate(bulletPrefabs[playerArtillery - 1], bulletSpawnPos, Quaternion.identity);
                    Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
                    bulletBody.AddForce(new Vector2(0, bulletSpeed), ForceMode2D.Impulse);
                    shootTimer = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (pause == false)
                    PauseGame();
                else
                    ContinueGame();
            }

            if (UIManager.totalScore >= 50000 && extraHard == 1)
            {
                EnemySpawner.spawnCD -= 0.1f;
                extraHard++;
            }
            else if (UIManager.totalScore >= 60000 && extraHard == 2)
            {
                EnemySpawner.spawnCD -= 0.1f;
                extraHard++;
            }
            else if (UIManager.totalScore >= 70000 && extraHard == 3)
            {
                EnemySpawner.spawnCD -= 0.1f;
                extraHard++;
            }
            else if (UIManager.totalScore >= 80000 && extraHard == 4)
            {
                EnemySpawner.spawnCD -= 0.1f;
                extraHard++;
            }
            else if (UIManager.totalScore >= 90000 && extraHard == 4)
            {
                EnemySpawner.spawnCD -= 0.1f;
                extraHard++;
            }

            if (UIManager.totalEnemiesDown >= 40 && rankUpSound == 1)
            {
                ranks[0].SetActive(false);
                ranks[1].SetActive(true);
                RankUpdate("First Lieutenant");
            }
            else if (UIManager.totalEnemiesDown >= 80 && rankUpSound == 2)
            {
                ranks[1].SetActive(false);
                ranks[2].SetActive(true);
                RankUpdate("Captain");
            }
            else if (UIManager.totalEnemiesDown >= 130 && rankUpSound == 3)
            {
                ranks[2].SetActive(false);
                ranks[3].SetActive(true);
                RankUpdate("Major");
            }
            else if (UIManager.totalEnemiesDown >= 180 && rankUpSound == 4)
            {
                ranks[3].SetActive(false);
                ranks[4].SetActive(true);
                RankUpdate("Lieutenant Colonel");
            }
            else if (UIManager.totalEnemiesDown >= 250 && rankUpSound == 5)
            {
                ranks[4].SetActive(false);
                ranks[5].SetActive(true);
                RankUpdate("Colonel");
            }
        }

    }

    private void FixedUpdate()
    {
        if (!gameOn)
        {

            Vector3 newPos = transform.position + 2 * Time.deltaTime * Vector3.up;

            player.MovePosition(newPos);


            if (player.position.y > -8 && !goSoundPlay)
            {
                goSoundPlay = true;
                SFXController.PlaySound("Gogogo");
            }

            if (player.position.y > -2.7)
            {
                getReadyText.SetActive(false);
                wallBottom.SetActive(true);
                SFXController.PlaySound("BGMusic");
                gameOn = true;
            }
        }
        else
        {
            if (player != null)
            {
                enduranceTimer += Time.deltaTime;
                FuelSpent();
                float xDirection = Input.GetAxisRaw("Horizontal");
                float yDirection = Input.GetAxis("Vertical");
                Vector3 moveInput = new(xDirection, yDirection / 2, 0);

                Vector3 newPos = transform.position + speed * Time.deltaTime * moveInput;

                player.MovePosition(newPos);

                if (xDirection > 0 || xDirection < 0)
                    transform.localScale = new Vector3(0.8f, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);

                bool upKeyPressed = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);

                if (upKeyPressed)
                {
                    madDogTimer += Time.deltaTime;

                    if (!speedUpSoundPlaying)
                    {
                        speedUpSound.Play();
                        speedUpSoundPlaying = true;
                    }
                }
                else if (!upKeyPressed && speedUpSoundPlaying)
                {
                    speedUpSound.Stop();
                    speedUpSoundPlaying = false;
                }

                bool downKeyPressed = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

                if (downKeyPressed)
                {
                    backlinerTimer += Time.deltaTime;
                    speed = 3.75f + (0.5f * playerAircraft / 2);

                    if (!speedDownSoundPlaying)
                    {
                        speedDownSound.Play();
                        speedDownSoundPlaying = true;
                    }

                }
                else if (!downKeyPressed)
                {
                    speed = 7.5f + (0.5f * playerAircraft);

                    if (speedDownSoundPlaying)
                    {
                        speedDownSound.Stop();
                        speedDownSoundPlaying = false;
                    }

                }

                if (UIManager.totalFuel <= 0)
                {
                    PlayerDeath();
                }

                //AWARDS
                if (UIManager.planeCrashes == 10 && UIManager.awardIndestructible == false)
                {
                    UIManager.awardIndestructible = true;
                    GetAward(1);
                    StartCoroutine(AlertBlink("AWARD: INDESTRUCTIBLE", 3, 0.25f));
                }

                if (enduranceTimer > 420 && UIManager.awardEndurance == false)
                {
                    UIManager.awardEndurance = true;
                    GetAward(2);
                    StartCoroutine(AlertBlink("AWARD: ENDURANCE", 3, 0.25f));
                }

                if (UIManager.totalEnemiesDown >= 25 && UIManager.awardUpComer == false)
                {
                    UIManager.awardUpComer = true;
                    GetAward(3);
                    StartCoroutine(AlertBlink("AWARD: UPCOMER", 3, 0.25f));
                }

                if (UIManager.totalSupplies >= 20 && UIManager.awardSmartWing == false)
                {
                    UIManager.awardSmartWing = true;
                    GetAward(4);
                    StartCoroutine(AlertBlink("AWARD: SMART WING", 3, 0.25f));
                }

                if (madDogTimer > 45 && UIManager.awardMadDog == false)
                {
                    UIManager.awardMadDog = true;
                    GetAward(5);
                    StartCoroutine(AlertBlink("AWARD: MAD DOG", 3, 0.25f));
                }

                if (backlinerTimer > 90 && UIManager.awardBackliner == false)
                {
                    UIManager.awardBackliner = true;
                    GetAward(6);
                    StartCoroutine(AlertBlink("AWARD: BACKLINER", 3, 0.25f));
                }

                if (UIManager.totalEnemiesDown >= 65 && UIManager.awardSharpshooter == false)
                {
                    UIManager.awardSharpshooter = true;
                    GetAward(8);
                    StartCoroutine(AlertBlink("AWARD: SHARPSHOOTER", 3, 0.25f));
                }

                if (EnemySpawner.enemyWave == 2 && UIManager.awardAce == false)
                {
                    UIManager.awardAce = true;
                    GetAward(13);
                    StartCoroutine(AlertBlink("AWARD: ACE", 3, 0.25f));
                }
                else if (EnemySpawner.enemyWave == 3 && UIManager.awardEliteFighter == false)
                {
                    UIManager.awardEliteFighter = true;
                    GetAward(14);
                    StartCoroutine(AlertBlink("AWARD: ELITE FIGHTER", 3, 0.25f));
                }
                else if (EnemySpawner.enemyWave == 4 && UIManager.awardAirForceHero == false)
                {
                    UIManager.awardAirForceHero = true;
                    GetAward(15);
                    StartCoroutine(AlertBlink("AWARD: AIR FORCE HERO", 3, 0.25f));
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.CompareTag("Enemy"))
        {
            int randomNumber = Random.Range(1, 3);
            SFXController.ExplosionSound(randomNumber);
            Instantiate(explosionPrefab, otherGO.transform.position, Quaternion.identity);
            Destroy(otherGO);
            EnemySpawner.destroyedPlanes++;
            UIManager.totalEnemiesDown++;
            UIManager.planeCrashes++;
            CrashDmg();
            enemiesDownText.text = UIManager.totalEnemiesDown.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.CompareTag("EnemyBullet"))
        {
            SFXController.PlaySound("BulletHit");
            BulletDmg();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyArmor"))
        {
            armorBar.DamageBar(0.5f * -1);
            StartCoroutine(AlertBlink("ARMOR UP", 3, 0.25f));

            if (UIManager.totalArmor >= 1 && UIManager.criticalArmor == true && UIManager.awardPhoenix == false)
            {
                UIManager.awardPhoenix = true;
                GetAward(12);
                StartCoroutine(AlertBlink("AWARD: PHOENIX", 3, 0.25f));
            }

            planeSmoke.SetActive(false);
            SFXController.PlaySound("ArmorUp");
            UIManager.totalScore += 200;
            UIManager.totalSupplies++;
            scoreText.text = UIManager.totalScore.ToString();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyFuel"))
        {
            StartCoroutine(AlertBlink("FUEL UP", 3, 0.25f));

            if (UIManager.totalFuel < 0.05f && UIManager.awardLastDrop == false)
            {
                UIManager.awardLastDrop = true;
                GetAward(7);
                StartCoroutine(AlertBlink("AWARD: UNTILL THE LAST DROP", 3, 0.25f));
            }

            fuelBar.FuelLoadBar(1f * -1);
            SFXController.PlaySound("FuelUp");
            UIManager.totalScore += 400;
            UIManager.totalSupplies++;
            UIManager.totalRefuels++;

            if (UIManager.totalRefuels == 6 && UIManager.awardThirstyJoe == false)
            {
                UIManager.awardThirstyJoe = true;
                GetAward(9);
                StartCoroutine(AlertBlink("AWARD: THIRSTY JOE", 3, 0.25f));
            }

            scoreText.text = UIManager.totalScore.ToString();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyAircraft"))
        {
            SFXController.PlaySound("AircraftUp");

            if (playerAircraft < 5)
            {
                playerAircraft++;
                EnemySpawner.spawnCD -= 0.1f;
                StartCoroutine(AlertBlink("AIRCRAFT UPGRADE", 3, 0.25f));

                if (playerAircraft == 5 && UIManager.awardFlyEmAll == false)
                {
                    UIManager.awardFlyEmAll = true;
                    GetAward(10);
                    StartCoroutine(AlertBlink("AWARD: FLY 'EM ALL", 3, 0.25f));
                }
            }

            CheckAircraft(playerAircraft);
            SetReloadTimer();
            UIManager.totalScore += 1000;
            UIManager.totalSupplies++;
            scoreText.text = UIManager.totalScore.ToString();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyArtillery"))
        {
            SFXController.PlaySound("ArtilleryUp");

            if (playerArtillery < 5)
            {
                playerArtillery++;
                StartCoroutine(AlertBlink("ARTILLERY UPGRADE", 3, 0.25f));

                if (playerArtillery == 3 && UIManager.awardArtilleryExpert == false)
                {
                    UIManager.awardArtilleryExpert = true;
                    GetAward(11);
                    StartCoroutine(AlertBlink("AWARD: MACHINE GUN EXPERT", 3, 0.25f));
                }
            }

            CheckArtillery(playerArtillery);
            UIManager.totalScore += 700;
            UIManager.totalSupplies++;
            scoreText.text = UIManager.totalScore.ToString();
            Destroy(otherGO);
        }
    }

    private void CheckAircraft(int playerAircraft)
    {
        if (playerAircraft == 1)
        {
            aircraftText.text = "WM - Bronco";
            playerSpriteRenderer.sprite = playerSprites[0];
        }
        else if (playerAircraft == 2)
        {
            aircraftText.text = "358 Airwolf";
            playerSpriteRenderer.sprite = playerSprites[1];
        }
        else if (playerAircraft == 3)
        {
            aircraftText.text = "JJ - Hawkfire";
            playerSpriteRenderer.sprite = playerSprites[2];
        }
        else if (playerAircraft == 4)
        {
            aircraftText.text = "Thunderstrike 20";
            playerSpriteRenderer.sprite = playerSprites[3];
        }
        else
        {
            aircraftText.text = "T-01 Flyflort";
            playerSpriteRenderer.sprite = playerSprites[4];
        }
    }

    private void CheckArtillery(int playerArtillery)
    {
        if (playerArtillery == 1)
        {
            artilleryText.text = "7.7mm M-Gun";
        }
        else if (playerArtillery == 2)
        {
            artilleryText.text = "12.7mm M-Gun";
        }
        else if (playerArtillery == 3)
        {
            artilleryText.text = "13.2mm M-Gun";
        }
        else if (playerArtillery == 4)
        {
            artilleryText.text = "20mm Cannon";
        }
        else
        {
            artilleryText.text = "30mm Cannon";
        }
    }

    private void RankUpdate(string rank)
    {
        rankText.text = rank;
        StartCoroutine(AlertBlink("RANK UP", 3, 0.25f));
        SFXController.PlaySound("RankUp");
        rankUpSound++;
        UIManager.playerRank++;
        UIManager.totalScore += (UIManager.playerRank * 500);
    }

    private IEnumerator AlertBlink(string textToBlink, int blinkCount, float blinkInterval)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            alertText.text = textToBlink;
            if (i == 2)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(blinkInterval);
            }

            alertText.text = "";
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void GetAward(int awardNumber)
    {
        SFXController.PlaySound("AwardSound");
        awardSlotObjects[UIManager.currentAwardIndex].SetActive(true);
        SpriteRenderer awardSpriteRenderer = awardSlotSpriteRenderer[UIManager.currentAwardIndex].GetComponent<SpriteRenderer>();
        awardSpriteRenderer.sprite = awardSprites[awardNumber - 1];
        UIManager.currentAwardIndex++;
        UIManager.totalAwards++;
    }

    private void SetReloadTimer()
    {
        reloadTimer = 0.6f - (playerAircraft * 0.1f);
    }

    private void FuelSpent()
    {
        bool upKeyPressed = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        bool downKeyPressed = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        fuelDemand = 0.00010f + (playerAircraft * 0.00002f);

        if (upKeyPressed)
        {
            fuelDemand *= 2;
        }


        if (downKeyPressed)
        {
            fuelDemand /= 1.4f;
        }

        fuelBar.FuelLoadBar(fuelDemand);
    }

    private void BulletDmg()
    {
        StartCoroutine(FlashSpriteToRed(0.05f));

        armorBar.DamageBar((0.1f * EnemySpawner.enemyLevel) / (playerAircraft * 1));

        if (UIManager.totalArmor <= 0.3f)
        {
            planeSmoke.SetActive(true);
        }

        if (UIManager.totalArmor <= 0.05f)
        {
            UIManager.criticalArmor = true;
        }

        if (UIManager.totalArmor <= 0)
        {
            PlayerDeath();
        }
    }

    private void CrashDmg()
    {
        StartCoroutine(FlashSpriteToRed(0.05f));

        armorBar.DamageBar(0.20f + (EnemySpawner.enemyFighter * 0.05f) / (playerAircraft * 1));

        if (UIManager.totalArmor <= 0.3f)
        {
            planeSmoke.SetActive(true);
        }

        if (UIManager.totalArmor <= 0.05f)
        {
            UIManager.criticalArmor = true;
        }

        if (UIManager.totalArmor <= 0)
        {
            PlayerDeath();
        }
    }

    private IEnumerator FlashSpriteToRed(float duration)
    {
        playerSpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        playerSpriteRenderer.color = originalColor;
    }

    private void PauseGame()
    {
        pause = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseText.SetActive(true);
    }

    private void ContinueGame()
    {
        pause = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseText.SetActive(false);
    }

    private void CheckRecords()
    {
        if (UIManager.totalScore > UIManager.recordScore)
        {
            UIManager.recordScore = UIManager.totalScore;
            finalTexts[1].text = "Final Score: " + UIManager.totalScore + " (Personal Record)";
        }
        else
        {
            finalTexts[1].text = "Final Score: " + UIManager.totalScore;
        }

        if (UIManager.totalEnemiesDown > UIManager.recordEnemiesDown)
        {
            UIManager.recordEnemiesDown = UIManager.totalEnemiesDown;
            finalTexts[2].text = "Enemies Down: " + UIManager.totalEnemiesDown + " (Personal Record)";
        }
        else
        {
            finalTexts[2].text = "Enemies Down: " + UIManager.totalEnemiesDown;
        }

        if (UIManager.playerRank > UIManager.recordRank)
        {
            UIManager.recordRank = UIManager.playerRank;
            finalTexts[3].text = "Rank: " + rankText.text + " (Personal Record)";
        }
        else
        {
            finalTexts[3].text = "Rank: " + rankText.text;
        }

        if (UIManager.totalAwards > UIManager.recordAwards)
        {
            UIManager.recordAwards = UIManager.totalAwards;
            finalTexts[4].text = "Awards: " + UIManager.totalAwards + "/15 (Personal Record)";
        }
        else
        {
            finalTexts[4].text = "Awards: " + UIManager.totalAwards + "/15";
        }
    }

    private void PlayerDeath()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SFXController.PlaySound("Explosion1");
        Destroy(gameObject);
        speedUpSound.Stop();
        speedDownSound.Stop();
        SFXController.StopSound("BGMusic");
        SFXController.PlaySound("GameOver");
        rpmDestroyed.SetActive(true);
        gameOn = false;
        if (UIManager.totalFuel <= 0)
        {
            finalTexts[0].text = "Out Of Fuel";
        }
        else
        {
            finalTexts[0].text = "Aircraft Destroyed";
        }

        CheckRecords();
        finalPanel.SetActive(true);
    }
}
