using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // User Consent Key
    public const string UserConsentKey = "UserConsent";

    // Username Key
    public const string UserNameKey = "UserName";

    // Sound Options Keys
    public const string MusicKey = "MusicKey";
    public const string SFXKey = "SFXKey";

    // Leaderboard Score Key
    public const string LeaderboardScoreKey = "LeaderboardScore";

    // Internet Connection Status
    public static bool hasNetwork = true;

    //On Game Score and Levels
    public static float totalArmor = 1f;
    public static float totalFuel = 1f;
    public static int playerAircraft = 1;
    public static int playerArtillery = 1;
    public static int totalScore = 0;
    public static int totalEnemiesDown = 0;
    public static int playerRank;
    public static int totalAwards = 0;
    public static int wavesSurvived = 0;

    //Shooting Accuracy
    public static int hitShots = 0;
    public static int missedShots = 0;

    //On Game Awards Boolean Status
    public static int currentAwardIndex = 0;

    public static int kamikazeHits = 0;
    public static int collectedSupplies = 0;
    public static int totalRefuels = 0;

    public static bool awardUpComer = false;
    public static bool awardMGunExpert = false;
    public static bool awardSmartWing = false;
    public static bool awardSharpshooter = false;
    public static bool awardEndurance = false;
    public static bool awardMadDog = false;
    public static bool awardBackliner = false;
    public static bool awardThirstyJoe = false;
    public static bool awardFlyEmAll = false;
    public static bool awardIndestructible = false;
    public static bool awardLastDrop = false;
    public static bool criticalArmor = false; //triggers Phoenix Award possibiity
    public static bool awardPhoenix = false;
    public static bool awardAce = false;
    public static bool awardEliteFighter = false;
    public static bool awardAirForceHero = false;

    //Survived Waves Tracker Helper
    public static bool currentWave = false;

    //Record Key Names
    public const string BestRankKey = "BestRank";
    public const string RankStringKey = "RankString";
    public const string HighScoreKey = "HighScore";
    public const string BestStrikeKey = "BestStrike";
    public const string AwardsKey = "Awards";

    //Award Key Names
    public const string Award01 = "AwardUpComer";
    public const string Award02 = "AwardMGunExpert";
    public const string Award03 = "AwardSmartWing";
    public const string Award04 = "AwardSharpShooter";
    public const string Award05 = "AwardEndurance";
    public const string Award06 = "AwardMadDog";
    public const string Award07 = "AwardBackliner";
    public const string Award08 = "AwardThirstyJoe";
    public const string Award09 = "AwardFlyEmAll";
    public const string Award10 = "AwardIndestructible";
    public const string Award11 = "AwardLastDrop";
    public const string Award12 = "AwardPhoenix";
    public const string Award13 = "AwardAce";
    public const string Award14 = "AwardEliteFighter";
    public const string Award15 = "AwardAirForceHero";

    //Full History Keys
    public const string TotalMissionsKey = "TotalMissions";
    public const string TotalEnemiesDownKey = "TotalEnemiesDown";
    public const string HitShotsKey = "HitShots";
    public const string MissedShotsKey = "MissedShots";
    public const string TotalShotsKey = "TotalShots";
    public const string ShootingAccuracyKey = "ShootingAccuracy";
    public const string TotalAwardsKey = "TotalAwards";
    public const string TotalRefuelsKey = "TotalRefuels";
    public const string CollectedSuppliesKey = "CollectedSupplies";
    public const string FullyUpgradedAircraftsKey = "FullyUpgradedAircrafts";
    public const string KamikazeHitsKey = "KamikazeHits";
    public const string PhoenixAwardsKey = "PhoenixAwards";
    public const string WavesSurvivedKey = "WavesSurvived";
    public const string AllAwardsInOneMissionKey = "AllAwardsInOneMission";

    //Interstitial Ads Key
    public const string TotalMissionsForInterstitialAdsKey = "TotalMissionsForInterstitialAds";

    //Update Check
    private bool isCleanedUp = false;

    //Player Controller Reference
    PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnDestroy()
    {
        if (!isCleanedUp)
        {
            isCleanedUp = true;

            CheckRecordUpdates();
            CheckAwardStatusUpdate();
            TotalMissionsUpdate();
            TotalEnemiesDownUpdate();
            ShootingAccuracyUpdate();
            TotalAwardsUpdate();
            CollectedSuppliesUpdate();
            FullyUpgradedAircraftsUpdate();
            KamikazesHitsUpdate();
            PhoenixAwardsUpdate();
            WavesSurvivedUpdate();
            AllAwardsInOneMissionUpdate();
        }
    }

    private void CheckRecordUpdates()
    {
        int currentBestRank = PlayerPrefs.GetInt(BestRankKey, 1);
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        int currentBestStrike = PlayerPrefs.GetInt(BestStrikeKey, 0);
        int currentAwards = PlayerPrefs.GetInt(AwardsKey, 0);

        if (playerRank > currentBestRank)
        {
            PlayerPrefs.SetInt(BestRankKey, playerRank);
            UpdateRank(playerRank);
        }

        if (totalScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, totalScore);
        }

        if (totalEnemiesDown > currentBestStrike)
        {
            PlayerPrefs.SetInt(BestStrikeKey, totalEnemiesDown);
        }

        if (totalAwards > currentAwards)
        {
            PlayerPrefs.SetInt(AwardsKey, totalAwards);
        }
    }

    private void UpdateRank(int endGameRank)
    {
        switch (endGameRank)
        {
            case 2:
                PlayerPrefs.SetString(RankStringKey, "First Lieutenant");
                break;

            case 3:
                PlayerPrefs.SetString(RankStringKey, "Captain");
                break;

            case 4:
                PlayerPrefs.SetString(RankStringKey, "Major");
                break;

            case 5:
                PlayerPrefs.SetString(RankStringKey, "Lieutenant Colonel");
                break;

            case 6:
                PlayerPrefs.SetString(RankStringKey, "Colonel");
                break;

            default:
                PlayerPrefs.SetString(RankStringKey, "Second Lieutenant");
                break;
        }
    }

    private void CheckAwardStatusUpdate()
    {
        int awardUpComerKeyInt = PlayerPrefs.GetInt(Award01, 0);
        int awardMGunExpertKeyInt = PlayerPrefs.GetInt(Award02, 0);
        int awardSmartWingKeyInt = PlayerPrefs.GetInt(Award03, 0);
        int awardSharpShooterKeyInt = PlayerPrefs.GetInt(Award04, 0);
        int awardEnduranceKeyInt = PlayerPrefs.GetInt(Award05, 0);
        int awardMadDogKeyInt = PlayerPrefs.GetInt(Award06, 0);
        int awardBacklinerKeyInt = PlayerPrefs.GetInt(Award07, 0);
        int awardThirstyJoeKeyInt = PlayerPrefs.GetInt(Award08, 0);
        int awardFlyEmAllKeyInt = PlayerPrefs.GetInt(Award09, 0);
        int awardIndestructibleKeyInt = PlayerPrefs.GetInt(Award10, 0);
        int awardLastDropKeyInt = PlayerPrefs.GetInt(Award11, 0);
        int awardPhoenixKeyInt = PlayerPrefs.GetInt(Award12, 0);
        int awardAceKeyInt = PlayerPrefs.GetInt(Award13, 0);
        int awardEliteFighterKeyInt = PlayerPrefs.GetInt(Award14, 0);
        int awardAirForceHeroKeyInt = PlayerPrefs.GetInt(Award15, 0);

        AwardKeyValueSet(awardUpComer, awardUpComerKeyInt, Award01);
        AwardKeyValueSet(awardMGunExpert, awardMGunExpertKeyInt, Award02);
        AwardKeyValueSet(awardSmartWing, awardSmartWingKeyInt, Award03);
        AwardKeyValueSet(awardSharpshooter, awardSharpShooterKeyInt, Award04);
        AwardKeyValueSet(awardEndurance, awardEnduranceKeyInt, Award05);
        AwardKeyValueSet(awardMadDog, awardMadDogKeyInt, Award06);
        AwardKeyValueSet(awardBackliner, awardBacklinerKeyInt, Award07);
        AwardKeyValueSet(awardThirstyJoe, awardThirstyJoeKeyInt, Award08);
        AwardKeyValueSet(awardFlyEmAll, awardFlyEmAllKeyInt, Award09);
        AwardKeyValueSet(awardIndestructible, awardIndestructibleKeyInt, Award10);
        AwardKeyValueSet(awardLastDrop, awardLastDropKeyInt, Award11);
        AwardKeyValueSet(awardPhoenix, awardPhoenixKeyInt, Award12);
        AwardKeyValueSet(awardAce, awardAceKeyInt, Award13);
        AwardKeyValueSet(awardEliteFighter, awardEliteFighterKeyInt, Award14);
        AwardKeyValueSet(awardAirForceHero, awardAirForceHeroKeyInt, Award15);
    }

    private void AwardKeyValueSet(bool awardBool, int awardKeyInt, string awardKeyName)
    {
        if (awardBool && awardKeyInt != 1)
        {
            PlayerPrefs.SetInt(awardKeyName, 1);
        }
    }

    private void TotalMissionsUpdate()
    {
        int currentTotalMissions = PlayerPrefs.GetInt(TotalMissionsKey, 0);
        PlayerPrefs.SetInt(TotalMissionsKey, currentTotalMissions + 1);
    }

    private void TotalEnemiesDownUpdate()
    {
        int currentTotalEnemiesDown = PlayerPrefs.GetInt(TotalEnemiesDownKey, 0);
        PlayerPrefs.SetInt(TotalEnemiesDownKey, currentTotalEnemiesDown + totalEnemiesDown);
    }

    private void ShootingAccuracyUpdate()
    {
        //Step 1: List the Current Accuracy Numbers (Hit, Missed, and Total Shots).
        int currentHitShots = PlayerPrefs.GetInt(HitShotsKey, 0);
        int currentMissedShots = PlayerPrefs.GetInt(MissedShotsKey, 0);
        int currentTotalShots = PlayerPrefs.GetInt(TotalShotsKey, 0);

        //Step 2: Update PlayerPrefs Hit, Missed, and Total Shots
        if (hitShots > 0)
        {
            PlayerPrefs.SetInt(HitShotsKey, currentHitShots + hitShots);
        }

        if (missedShots > 0)
        {
            PlayerPrefs.SetInt(MissedShotsKey, currentMissedShots + missedShots);
        }
        
        int totalShots = hitShots + missedShots;

        if (totalShots > 0)
        {
            PlayerPrefs.SetInt(TotalShotsKey, currentTotalShots + totalShots);
        }

        //Step 3: Calculate Accuracy
        currentHitShots = PlayerPrefs.GetInt(HitShotsKey, 0);
        currentTotalShots = PlayerPrefs.GetInt(TotalShotsKey, 0);
        
        if (currentTotalShots > 0)
        {
            float shootingAccuracy = (float)currentHitShots / currentTotalShots * 100;

            PlayerPrefs.SetInt(ShootingAccuracyKey, Mathf.RoundToInt(shootingAccuracy));
        }
        else
        {
            return;
        }
    }

    private void TotalAwardsUpdate()
    {
        int currentTotalAwards = PlayerPrefs.GetInt(TotalAwardsKey, 0);
        PlayerPrefs.SetInt(TotalAwardsKey, currentTotalAwards + totalAwards);
    }

    private void CollectedSuppliesUpdate()
    {
        int currentCollectedSupplies = PlayerPrefs.GetInt(CollectedSuppliesKey, 0);
        PlayerPrefs.SetInt(CollectedSuppliesKey, currentCollectedSupplies + collectedSupplies);
    }

    private void FullyUpgradedAircraftsUpdate()
    {
        if (playerController.playerAircraft > 4 && playerController.playerArtillery > 4)
        {
            int currentFullyUpgradedAircrafts = PlayerPrefs.GetInt(FullyUpgradedAircraftsKey, 0);
            PlayerPrefs.SetInt(FullyUpgradedAircraftsKey, currentFullyUpgradedAircrafts + 1);
        }
    }

    private void KamikazesHitsUpdate()
    {
        int currentKamikazesHits = PlayerPrefs.GetInt(KamikazeHitsKey, 0);
        PlayerPrefs.SetInt(KamikazeHitsKey, currentKamikazesHits + kamikazeHits);
    }


    private void PhoenixAwardsUpdate()
    {
        if (awardPhoenix)
        {
            int currentPhoenixAwards = PlayerPrefs.GetInt(PhoenixAwardsKey, 0);
            PlayerPrefs.SetInt(PhoenixAwardsKey, currentPhoenixAwards + 1);
        }
    }

    private void WavesSurvivedUpdate()
    {
        if (wavesSurvived > 0)
        {
            int currentWavesSurvived = PlayerPrefs.GetInt(WavesSurvivedKey, 0);
            PlayerPrefs.SetInt(WavesSurvivedKey, currentWavesSurvived + wavesSurvived);
        }
    }

    private void AllAwardsInOneMissionUpdate()
    {
        if (totalAwards == 15)
        {
            int currentAllAwardsInOneMission = PlayerPrefs.GetInt(AllAwardsInOneMissionKey, 0);
            PlayerPrefs.SetInt(AllAwardsInOneMissionKey, currentAllAwardsInOneMission + 1);
        }
    } 
}