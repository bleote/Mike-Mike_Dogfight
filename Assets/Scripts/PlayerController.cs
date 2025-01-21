using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float verticalSpeedDivider;
    [SerializeField] private GameObject playerGO;
    private Rigidbody2D player;
    private SpriteRenderer playerSpriteRenderer;
    private Color originalColor = Color.white;
    private float aircraftSpeed;
    private Vector2 moveDirection;
    private bool slowingDown = false;
    private bool speedingUp = false;
    private bool isShooting = false;

    [Header("Joystick")]
    [SerializeField] private RectTransform fireButtonRT;
    [SerializeField] private Image fireButtonImage;
    [SerializeField] private float buttonShootingSize = 0.95f;

    [Header("Sound")]
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject objectMusicOn;
    [SerializeField] private GameObject objectMusicOff;
    [SerializeField] private GameObject objectSfxOn;
    [SerializeField] private GameObject objectSfxOff;
    private bool speedDownSoundPlaying = false;
    private bool speedUpSoundPlaying = false;

    [Header("Player Aircrafts")]
    [SerializeField] private Sprite[] playerSprites;

    [Header("Bars")]
    [SerializeField] private ArmorBar armorBar;
    [SerializeField] private FuelBar fuelBar;
    private float fuelDemand;

    [Header("Bullets")]
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private float bulletSpeed;
    private float shootTimer = 0;
    private float reloadTimer;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    private GameObject planeSmoke;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI aircraftText;
    [SerializeField] private TextMeshProUGUI artilleryText;
    [SerializeField] private TextMeshProUGUI enemiesDownText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI alertText;

    public int playerAircraft;
    public int playerArtillery;

    public static int extraHard = 1;

    [Header("Ranks")]
    private string currentRankString;
    private int rankUpSoundTrigger;
    private int rankBonus;

    [Header("Awards")]
    [SerializeField] private GameObject[] awardSlotObjects;
    [SerializeField] private Sprite[] awardSprites;
    [SerializeField] private string[] awardNames;

    private float enduranceTimer = 0;
    private float madDogTimer = 0;
    private float backlinerTimer = 0;

    public static bool gameOn = false;
    public static bool gameContinue;
    private bool goSoundPlay = false;

    [Header("Other")]
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject getReadyText;
    [SerializeField] private GameObject abortPanel;

    [Header("Final Panel")]
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private TheGeneral theGeneral;
    [SerializeField] private TextMeshProUGUI causeOfDeathText;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject interstitialAdsNewGameButton;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject interstitialAdsMainMenuButton;
    public int missionsInterstitial;

    [Header("Interstitial Ads")]
    [SerializeField] InterstitialAdsManager interstitialAdsManager;

    UnityAnalyticsManager unityAnalyticsManager;

    private bool reachedMaxAircraft = false;
    private bool reachedMaxArtillery = false;

    void Awake()
    {
        NewGameSetup();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (gameOn)
        {
            CheckForRankPromotionOrBonus();

            CheckForExtraHard();
        }

    }

    private void FixedUpdate()
    {
        if (!gameOn)
        {
            if (!gameContinue)
            {
                Vector3 newPos = transform.position + 2.8f * Time.deltaTime * Vector3.up;

                player.MovePosition(newPos);


                if (player.position.y > -8 && !goSoundPlay)
                {
                    goSoundPlay = true;
                    SFXController.PlaySound("Gogogo");
                }

                else if (player.position.y > -2.5f)
                {
                    getReadyText.SetActive(false);
                    wallBottom.SetActive(true);
                    gameOn = true;
                    if (SFXController.musicOn)
                    {
                        SFXController.PlayMusic("BGMusic");
                    }
                }

            }
            else
            {
                MusicAndSoundCheck();
            }
        }
        else
        {
            if (player != null)
            {
                enduranceTimer += Time.deltaTime;

                FuelUpdate();

                MovePlane();

                FireBullets();

                CheckForAwards();
            }
        }
    }

    public void PlayerMovementInput(InputAction.CallbackContext value)
    {
        moveDirection = value.ReadValue<Vector2>();
    }

    private void MovePlane()
    {
        Vector3 playerMovement = new(moveDirection.x, moveDirection.y / verticalSpeedDivider);

        Vector3 newPos = transform.position + aircraftSpeed * Time.deltaTime * playerMovement;

        player.MovePosition(newPos);

        if (moveDirection.y > 0.2)
        {
            speedingUp = true;
        }
        else
        {
            speedingUp = false;
        }

        if (speedingUp)
        {
            madDogTimer += Time.deltaTime;

            if (!speedUpSoundPlaying)
            {
                SFXController.PlaySound("SpeedUp");
                speedUpSoundPlaying = true;
            }
        }
        else if (!speedingUp && speedUpSoundPlaying)
        {
            SFXController.StopSound("SpeedUp");
            speedUpSoundPlaying = false;
        }

        if (moveDirection.y < -0.2)
        {
            slowingDown = true;
        }
        else
        {
            slowingDown = false;
        }

        if (slowingDown)
        {
            backlinerTimer += Time.deltaTime;

            if (!speedDownSoundPlaying)
            {
                SFXController.PlaySound("SpeedDown");
                speedDownSoundPlaying = true;
            }

        }
        else if (!slowingDown)
        {

            if (speedDownSoundPlaying)
            {
                SFXController.StopSound("SpeedDown");
                speedDownSoundPlaying = false;
            }

        }
    }

    public void FireInput(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            isShooting = true;
            fireButtonRT.localScale = new Vector2(buttonShootingSize, buttonShootingSize);
            fireButtonImage.color = new Color(0.13f, 0.78f, 1, 1);
        }

        if (value.canceled)
        {
            isShooting = false;
            fireButtonRT.localScale = Vector2.one;
            fireButtonImage.color = new Color(0, 0, 0, 1);
        }
    }

    private void FireBullets()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer > reloadTimer)
        {
            if (isShooting && Time.timeScale != 0)
            {
                BulletSpawn();
            }
        }
    }

    private void BulletSpawn()
    {
        Vector2 bulletSpawnPos = player.position + new Vector2(0.516f, 0.25f);
        GameObject newBullet = Instantiate(bulletPrefabs[playerArtillery - 1], bulletSpawnPos, Quaternion.identity);
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        SFXController.PlaySound("BulletSound");
        bulletBody.AddForce(new Vector2(0, bulletSpeed), ForceMode2D.Impulse);
        shootTimer = 0;
    }

    private void FuelUpdate()
    {
        FuelConsume();

        if (GameManager.totalFuel <= 0)
        {
            PlayerDeath();
        }
    }
    private void FuelConsume()
    {
        fuelDemand = 0.00010f + (playerAircraft * 0.00002f);

        if (speedingUp)
        {
            fuelDemand *= 2;
        }


        if (slowingDown)
        {
            fuelDemand /= 1.4f;
        }

        fuelBar.FuelLoadBar(fuelDemand);
    }

    private void CheckForAwards()
    {
        if (GameManager.kamikazeHits == 10 && GameManager.awardIndestructible == false)
        {
            GameManager.awardIndestructible = true;
            GetAward(10);
            StartCoroutine(AlertBlink("AWARD: INDESTRUCTIBLE", 3, 0.25f));
        }

        if (enduranceTimer > 480 && GameManager.awardEndurance == false)
        {
            GameManager.awardEndurance = true;
            GetAward(5);
            StartCoroutine(AlertBlink("AWARD: ENDURANCE", 3, 0.25f));
        }

        if (GameManager.totalEnemiesDown >= 50 && GameManager.awardUpComer == false)
        {
            GameManager.awardUpComer = true;
            GetAward(1);
            StartCoroutine(AlertBlink("AWARD: UPCOMER", 3, 0.25f));
        }

        if (GameManager.collectedSupplies >= 30 && GameManager.awardSmartWing == false)
        {
            GameManager.awardSmartWing = true;
            GetAward(3);
            StartCoroutine(AlertBlink("AWARD: SMART WING", 3, 0.25f));
        }

        if (madDogTimer > 60 && GameManager.awardMadDog == false)
        {
            GameManager.awardMadDog = true;
            GetAward(6);
            StartCoroutine(AlertBlink("AWARD: MAD DOG", 3, 0.25f));
        }

        if (backlinerTimer > 120 && GameManager.awardBackliner == false)
        {
            GameManager.awardBackliner = true;
            GetAward(7);
            StartCoroutine(AlertBlink("AWARD: BACKLINER", 3, 0.25f));
        }

        if (GameManager.totalEnemiesDown >= 130 && GameManager.awardSharpshooter == false)
        {
            GameManager.awardSharpshooter = true;
            GetAward(4);
            StartCoroutine(AlertBlink("AWARD: SHARPSHOOTER", 3, 0.25f));
        }

        if (EnemySpawner.enemyWave == 4 && GameManager.awardAce == false)
        {
            GameManager.awardAce = true;
            GetAward(13);
            StartCoroutine(AlertBlink("AWARD: ACE", 3, 0.25f));
        }
        else if (EnemySpawner.enemyWave == 7 && GameManager.awardEliteFighter == false)
        {
            GameManager.awardEliteFighter = true;
            GetAward(14);
            StartCoroutine(AlertBlink("AWARD: ELITE FIGHTER", 3, 0.25f));
        }
        else if (EnemySpawner.enemyWave == 10 && GameManager.awardAirForceHero == false)
        {
            GameManager.awardAirForceHero = true;
            GetAward(15);
            StartCoroutine(AlertBlink("AWARD: AIR FORCE HERO", 3, 0.25f));
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
            GameManager.totalEnemiesDown++;
            GameManager.kamikazeHits++;
            CrashDmg();
            enemiesDownText.text = GameManager.totalEnemiesDown.ToString();
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

            if (GameManager.totalArmor >= 1 && GameManager.criticalArmor == true && GameManager.awardPhoenix == false)
            {
                GameManager.awardPhoenix = true;
                GetAward(12);
                StartCoroutine(AlertBlink("AWARD: PHOENIX", 3, 0.25f));
            }

            planeSmoke.SetActive(false);
            SFXController.PlaySound("ArmorUp");
            GameManager.totalScore += 200;
            GameManager.collectedSupplies++;
            scoreText.text = GameManager.totalScore.ToString();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyFuel"))
        {
            StartCoroutine(AlertBlink("FUEL UP", 3, 0.25f));

            if (GameManager.totalFuel < 0.05f && GameManager.awardLastDrop == false)
            {
                GameManager.awardLastDrop = true;
                GetAward(11);
                StartCoroutine(AlertBlink("AWARD: UNTILL THE LAST DROP", 3, 0.25f));
            }

            fuelBar.FuelLoadBar(1f * -1);
            SFXController.PlaySound("FuelUp");
            GameManager.totalScore += 400;
            GameManager.collectedSupplies++;
            GameManager.totalRefuels++;

            if (GameManager.totalRefuels == 8 && GameManager.awardThirstyJoe == false)
            {
                GameManager.awardThirstyJoe = true;
                GetAward(8);
                StartCoroutine(AlertBlink("AWARD: THIRSTY JOE", 3, 0.25f));
            }

            scoreText.text = GameManager.totalScore.ToString();
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

                if (playerAircraft == 5 && GameManager.awardFlyEmAll == false)
                {
                    GameManager.awardFlyEmAll = true;
                    GetAward(9);
                    StartCoroutine(AlertBlink("AWARD: FLY 'EM ALL", 3, 0.25f));
                }
            }

            CheckAircraft(playerAircraft);
            SetReloadTimer();
            GameManager.totalScore += 1000;
            GameManager.collectedSupplies++;
            scoreText.text = GameManager.totalScore.ToString();
            Destroy(otherGO);
        }
        else if (otherGO.CompareTag("SupplyArtillery"))
        {
            SFXController.PlaySound("ArtilleryUp");

            if (playerArtillery < 5)
            {
                playerArtillery++;
                StartCoroutine(AlertBlink("ARTILLERY UPGRADE", 3, 0.25f));

                if (playerArtillery == 3 && GameManager.awardMGunExpert == false)
                {
                    GameManager.awardMGunExpert = true;
                    GetAward(2);
                    StartCoroutine(AlertBlink("AWARD: MACHINE GUN EXPERT", 3, 0.25f));
                }
            }

            CheckArtillery(playerArtillery);
            GameManager.totalScore += 700;
            GameManager.collectedSupplies++;
            scoreText.text = GameManager.totalScore.ToString();
            Destroy(otherGO);
        }
    }

    private void CheckAircraft(int playerAircraft)
    {
        if (!reachedMaxAircraft)
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
                reachedMaxAircraft = true;
            }

            unityAnalyticsManager.TrackAircraftProgress(playerAircraft);
        }
    }

    private void CheckArtillery(int playerArtillery)
    {
        if (!reachedMaxArtillery)
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
                reachedMaxArtillery = true;
            }

            unityAnalyticsManager.TrackArtilleryProgress(playerArtillery);
        }
    }

    private void SetRankUpSoundTrigger(string currentRankString)
    {
        rankUpSoundTrigger = currentRankString switch
        {
            "First Lieutenant" => 2,
            "Captain" => 3,
            "Major" => 4,
            "Lieutenant Colonel" => 5,
            "Colonel" => 6,
            _ => 1,
        };
    }

    private void CheckForRankPromotionOrBonus()
    {
        if (GameManager.totalEnemiesDown >= 80 && rankBonus == 1)
        {
            if (rankUpSoundTrigger == 1)
            {
                InGameRankUpdate("First Lieutenant");
            }
            else
            {
                RankBonus();
            }

            rankBonus++;
        }
        else if (GameManager.totalEnemiesDown >= 160 && rankBonus == 2)
        {
            if (rankUpSoundTrigger == 2)
            {
                InGameRankUpdate("Captain");
            }
            else
            {
                RankBonus();
            }

            rankBonus++;
        }
        else if (GameManager.totalEnemiesDown >= 260 && rankBonus == 3)
        {
            if (rankUpSoundTrigger == 3)
            {
                InGameRankUpdate("Major");
            }
            else
            {
                RankBonus();
            }

            rankBonus++;
        }
        else if (GameManager.totalEnemiesDown >= 360 && rankBonus == 4)
        {
            if (rankUpSoundTrigger == 4)
            {
                InGameRankUpdate("Lieutenant Colonel");
            }
            else
            {
                RankBonus();
            }

            rankBonus++;
        }
        else if (GameManager.totalEnemiesDown >= 500 && rankBonus == 5)
        {
            if (rankUpSoundTrigger == 5)
            {
                InGameRankUpdate("Colonel");
            }
            else
            {
                RankBonus();
            }

            rankBonus++;
        }
    }

    private void InGameRankUpdate(string rank)
    {
        rankText.text = rank;
        StartCoroutine(AlertBlink("RANK PROMOTION", 3, 0.5f));
        SFXController.PlaySound("RankUp");
        rankUpSoundTrigger++;
        GameManager.playerRank++;
        GameManager.totalScore += (GameManager.playerRank * 500);
        unityAnalyticsManager.TrackRankPromotion(rank);
    }

    private void RankBonus()
    {
        StartCoroutine(AlertBlink("RANK BONUS", 3, 0.5f));
        SFXController.PlaySound("RankUp");
        GameManager.totalScore += (GameManager.playerRank * 500);
    }

    private IEnumerator AlertBlink(string textToBlink, int blinkCount, float blinkInterval)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            alertText.text = textToBlink;
            if (i == 2)
            {
                yield return new WaitForSeconds(2f);
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
        awardSlotObjects[GameManager.currentAwardIndex].SetActive(true);
        SpriteRenderer awardSpriteRenderer = awardSlotObjects[GameManager.currentAwardIndex].GetComponent<SpriteRenderer>();
        awardSpriteRenderer.sprite = awardSprites[awardNumber - 1];
        GameManager.currentAwardIndex++;
        GameManager.totalAwards++;
        string awardName = awardNames[awardNumber - 1];
        unityAnalyticsManager.TrackAwardReceived(awardName);
    }

    private void SetReloadTimer()
    {
        reloadTimer = 0.6f - (playerAircraft * 0.1f);
    }

    private void BulletDmg()
    {
        StartCoroutine(FlashSpriteToRed(0.05f));

        armorBar.DamageBar((0.1f * EnemySpawner.enemyLevel) / (playerAircraft * 1));

        if (GameManager.totalArmor <= 0.3f)
        {
            planeSmoke.SetActive(true);
        }

        if (GameManager.totalArmor <= 0.05f)
        {
            GameManager.criticalArmor = true;
        }

        if (GameManager.totalArmor <= 0)
        {
            PlayerDeath();
        }
    }

    private void CrashDmg()
    {
        StartCoroutine(FlashSpriteToRed(0.05f));

        armorBar.DamageBar(0.20f + (EnemySpawner.enemyFighter * 0.05f) / (playerAircraft * 1));

        if (GameManager.totalArmor <= 0.3f)
        {
            planeSmoke.SetActive(true);
        }

        if (GameManager.totalArmor <= 0.05f)
        {
            GameManager.criticalArmor = true;
        }

        if (GameManager.totalArmor <= 0)
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

    public void MusicAndSoundCheck()
    {
        if (SFXController.musicOn)
        {
            objectMusicOn.SetActive(true);
            objectMusicOff.SetActive(false);
        }
        else
        {
            objectMusicOff.SetActive(true);
            objectMusicOn.SetActive(false);
        }

        if (SFXController.sfxOn)
        {
            objectSfxOn.SetActive(true);
            objectSfxOff.SetActive(false);
        }
        else
        {
            objectSfxOff.SetActive(true);
            objectSfxOn.SetActive(false);
        }

    }

    private void PlayerDeath()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SFXController.PlaySound("Explosion1");
        SFXController.StopSound("EngineSound");
        SFXController.StopSound("SpeedUp");
        SFXController.StopSound("SpeedDown");
        SFXController.StopMusic("BGMusic");
        SFXController.PlaySound("GameOver");
        planeSmoke.SetActive(false);
        playerSpriteRenderer.color = originalColor;
        transform.position = new Vector2(0, -2.75f);
        playerGO.SetActive(false);
        gameOn = false;

        if (GameManager.totalFuel <= 0)
        {
            causeOfDeathText.text = "Out Of Fuel";
        }
        else
        {
            causeOfDeathText.text = "Aircraft Destroyed";
        }

        theGeneral.MessageFromTheGeneral();

        if (soundPanel.activeInHierarchy == true)
        {
            soundPanel.SetActive(false);
        }

        if (abortPanel.activeInHierarchy == true)
        {
            abortPanel.SetActive(false);
        }

        CheckForInterstitialAds();

        finalPanel.SetActive(true);

        if (GameManager.hasNetwork)
        {
            interstitialAdsManager.LoadAd();
        }
    }

    private void CheckForInterstitialAds()
    {
        if (GameManager.hasNetwork)
        {

            if (missionsInterstitial == 2)
            {

                newGameButton.SetActive(false);
                mainMenuButton.SetActive(false);
                interstitialAdsNewGameButton.SetActive(true);
                interstitialAdsMainMenuButton.SetActive(true);
            }
            else
            {
                newGameButton.SetActive(true);
                mainMenuButton.SetActive(true);
                interstitialAdsNewGameButton.SetActive(false);
                interstitialAdsMainMenuButton.SetActive(false);
            }
        }
    }

    private void NewGameSetup()
    {
        GameManager.totalArmor = 1f;
        GameManager.totalFuel = 1f;
        GameManager.playerAircraft = 1;
        GameManager.playerArtillery = 1;
        GameManager.hitShots = 0;
        GameManager.missedShots = 0;
        GameManager.totalScore = 0;
        GameManager.totalEnemiesDown = 0;
        GameManager.playerRank = PlayerPrefs.GetInt(GameManager.BestRankKey, 1);
        GameManager.totalAwards = 0;
        GameManager.currentAwardIndex = 0;
        GameManager.kamikazeHits = 0;
        GameManager.collectedSupplies = 0;
        GameManager.totalRefuels = 0;
        GameManager.awardIndestructible = false;
        GameManager.awardEndurance = false;
        GameManager.awardUpComer = false;
        GameManager.awardSmartWing = false;
        GameManager.awardMadDog = false;
        GameManager.awardBackliner = false;
        GameManager.awardLastDrop = false;
        GameManager.awardSharpshooter = false;
        GameManager.awardThirstyJoe = false;
        GameManager.awardFlyEmAll = false;
        GameManager.awardMGunExpert = false;
        GameManager.criticalArmor = false;
        GameManager.awardPhoenix = false;
        GameManager.awardAce = false;
        GameManager.awardEliteFighter = false;
        GameManager.awardAirForceHero = false;

        missionsInterstitial = PlayerPrefs.GetInt(GameManager.TotalMissionsForInterstitialAdsKey, 1);

        unityAnalyticsManager = FindObjectOfType<UnityAnalyticsManager>();

        EnemySpawner.spawnCD = 1.3f;
        EnemySpawner.destroyedPlanes = 0;
        EnemySpawner.enemyFighter = 1;
        EnemySpawner.enemyLevel = 1;
        EnemySpawner.enemyWave = 1;

        SFXController.scene = 2;

        gameContinue = false;

        currentRankString = PlayerPrefs.GetString(GameManager.RankStringKey, "Second Lieutenant");
        SetRankUpSoundTrigger(currentRankString);

        playerAircraft = GameManager.playerAircraft;
        reachedMaxAircraft = false;
        playerArtillery = GameManager.playerArtillery;
        reachedMaxArtillery = false;
        aircraftSpeed = speed + (0.5f * GameManager.playerAircraft);
        MusicAndSoundCheck();
        player = GetComponent<Rigidbody2D>();
        planeSmoke = GameObject.FindGameObjectWithTag("PlaneSmoke");
        planeSmoke.SetActive(false);
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        scoreText.text = GameManager.totalScore.ToString();
        rankText.text = currentRankString;
        rankBonus = 1;
        CheckAircraft(playerAircraft);
        CheckArtillery(playerArtillery);
        SetReloadTimer();
        SFXController.PlaySound("EngineSound");
    }

    private void CheckForExtraHard()
    {
        if (GameManager.totalScore >= 100000 && extraHard == 1)
        {
            EnemySpawner.spawnCD -= 0.1f;
            extraHard++;
        }
        else if (GameManager.totalScore >= 150000 && extraHard == 2)
        {
            EnemySpawner.spawnCD -= 0.1f;
            extraHard++;
        }
        else if (GameManager.totalScore >= 200000 && extraHard == 3)
        {
            EnemySpawner.spawnCD -= 0.1f;
            extraHard++;
        }
        else if (GameManager.totalScore >= 250000 && extraHard == 4)
        {
            EnemySpawner.spawnCD -= 0.1f;
            extraHard++;
        }
        else if (GameManager.totalScore >= 300000 && extraHard == 5)
        {
            EnemySpawner.spawnCD -= 0.1f;
            extraHard++;
        }
    }
}
