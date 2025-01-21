using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Username")]
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_Text usernameText;

    [Header("Player Records and Status")]
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text bestStrikeText;
    [SerializeField] private TMP_Text awardsText;
    [SerializeField] private TMP_Text playerStatusText;

    [Header("Insignias and Awards")]
    [SerializeField] private GameObject[] insigniasArray;
    [SerializeField] private Image[] awardSlots;
    [SerializeField] private Image[] awardImages;

    [Header("Full History")]
    [SerializeField] private TMP_Text missionsText;
    [SerializeField] private TMP_Text enemiesDownText;
    [SerializeField] private TMP_Text averageStrikeText;
    [SerializeField] private TMP_Text shootingAccuracyText;
    [SerializeField] private TMP_Text totalAwardsText;
    [SerializeField] private TMP_Text awardsPerMissionText;
    [SerializeField] private TMP_Text collectedSuppliesText;
    [SerializeField] private TMP_Text fullyUpgradedAircraftsText;
    [SerializeField] private TMP_Text kamikazeHitsText;
    [SerializeField] private TMP_Text phoenixAwardsText;
    [SerializeField] private TMP_Text wavesSurvivedText;
    [SerializeField] private TMP_Text allAwardsInOneMissionText;


    [Header("Sound Canvas")]
    [SerializeField] private GameObject objectMusicOn;
    [SerializeField] private GameObject objectMusicOff;
    [SerializeField] private GameObject objectSfxOn;
    [SerializeField] private GameObject objectSfxOff;

    string username;
    int playerHighScore;
    int playerBestStrike;
    int playerLeaderboardScore;

    [Header("Leadereboard")]
    [SerializeField] private MainMenuUploadToLeaderboard mainMenuUploadToLeaderboard;

    private void Awake()
    {
        //CleanRecords();

        ResetGameplaySetup();
        LoadSoundSettings();
        CheckUserName();
        CheckRecords();
        CheckPlayerStatus();
        CheckInsignia(PlayerPrefs.GetInt(GameManager.BestRankKey, 1));
        UpdateAwardsDisplay();
        UpdateFullHistory();
    }

    private void Start()
    {
        CheckLeaderboardNewEntry();
    }

    private void ResetGameplaySetup()
    {
        SFXController.scene = 1;
        //Time.timeScale = 1;
        //AudioListener.pause = false;
        PlayerController.gameOn = false;
    }

    private void LoadSoundSettings()
    {
        int currentMusicOption = PlayerPrefs.GetInt(GameManager.MusicKey, 0);
        int currentSFXOption = PlayerPrefs.GetInt(GameManager.SFXKey, 0);

        if (currentMusicOption == 0)
        {
            SFXController.musicOn = true;
        }
        else
        {
            SFXController.musicOn = false;
        }

        if (currentSFXOption == 0)
        {
            SFXController.sfxOn = true;
        }
        else
        {
            SFXController.sfxOn = false;
        }

        MusicAndSoundButtonsCheck();
        CheckBGMusicPlay();
    }

    public void MusicAndSoundButtonsCheck()
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

    private void CheckBGMusicPlay()
    {
        if (SFXController.musicOn)
        {
            AudioSource bgMusic = GameObject.FindGameObjectWithTag("BGMusic").GetComponent<AudioSource>();
            bgMusic.Play();
        }
    }

    private void CheckUserName()
    {
        string currentUserName = PlayerPrefs.GetString(GameManager.UserNameKey, string.Empty);

        if (currentUserName == string.Empty)
        {
            usernamePanel.SetActive(true);
        }
        else
        {
            usernameText.text = currentUserName;
        }
    }

    private void CheckRecords()
    {
        string bestRankString = PlayerPrefs.GetString(GameManager.RankStringKey, "Second Lieutenant");
        int highScore = PlayerPrefs.GetInt(GameManager.HighScoreKey, 0);
        int bestStrike = PlayerPrefs.GetInt(GameManager.BestStrikeKey, 0);
        int awards = PlayerPrefs.GetInt(GameManager.AwardsKey, 0);

        rankText.text = $"Rank: {bestRankString}";
        highScoreText.text = $"High Score: {highScore}";
        bestStrikeText.text = $"Best Strike: {bestStrike}";
        awardsText.text = $"Unique Awards: {awards}/15";
    }

    private void CheckPlayerStatus()
    {
        int awardAceKeyInt = PlayerPrefs.GetInt(GameManager.Award13, 0);
        int awardEliteFighterKeyInt = PlayerPrefs.GetInt(GameManager.Award14, 0);
        int awardAirForceHeroKeyInt = PlayerPrefs.GetInt(GameManager.Award15, 0);

        if (awardAirForceHeroKeyInt == 1)
        {
            playerStatusText.text = "Air Force Hero";
        }
        else if (awardEliteFighterKeyInt == 1)
        {
            playerStatusText.text = "Elite Fighter";
        }
        else if (awardAceKeyInt == 1)
        {
            playerStatusText.text = "Ace";
        }
        else
        {
            playerStatusText.text = "Officer";
        }
    }

    private void CheckInsignia(int BestRankKey)
    {
        for (int i = 0; i < insigniasArray.Length; i++)
        {
            insigniasArray[BestRankKey - 1].SetActive(true);
        }
    }

    private void UpdateAwardsDisplay()
    {
        // Step 1: Retrieve all 15 PlayerPrefs integers
        int awardUpComerKeyInt = PlayerPrefs.GetInt(GameManager.Award01, 0);
        int awardMGunExpertKeyInt = PlayerPrefs.GetInt(GameManager.Award02, 0);
        int awardSmartWingKeyInt = PlayerPrefs.GetInt(GameManager.Award03, 0);
        int awardSharpShooterKeyInt = PlayerPrefs.GetInt(GameManager.Award04, 0);
        int awardEnduranceKeyInt = PlayerPrefs.GetInt(GameManager.Award05, 0);
        int awardMadDogKeyInt = PlayerPrefs.GetInt(GameManager.Award06, 0);
        int awardBacklinerKeyInt = PlayerPrefs.GetInt(GameManager.Award07, 0);
        int awardThirstyJoeKeyInt = PlayerPrefs.GetInt(GameManager.Award08, 0);
        int awardFlyEmAllKeyInt = PlayerPrefs.GetInt(GameManager.Award09, 0);
        int awardIndestructibleKeyInt = PlayerPrefs.GetInt(GameManager.Award10, 0);
        int awardLastDropKeyInt = PlayerPrefs.GetInt(GameManager.Award11, 0);
        int awardPhoenixKeyInt = PlayerPrefs.GetInt(GameManager.Award12, 0);
        int awardAceKeyInt = PlayerPrefs.GetInt(GameManager.Award13, 0);
        int awardEliteFighterKeyInt = PlayerPrefs.GetInt(GameManager.Award14, 0);
        int awardAirForceHeroKeyInt = PlayerPrefs.GetInt(GameManager.Award15, 0);

        // Step 2: Form a list with PlayerPrefs integers equal to 1, keeping "ascending" order
        List<Image> earnedAwardsIndexes = new();

        if (awardUpComerKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[0]);
        }

        if (awardMGunExpertKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[1]);
        }

        if (awardSmartWingKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[2]);
        }

        if (awardSharpShooterKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[3]);
        }

        if (awardEnduranceKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[4]);
        }

        if (awardMadDogKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[5]);
        }

        if (awardBacklinerKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[6]);
        }

        if (awardThirstyJoeKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[7]);
        }

        if (awardFlyEmAllKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[8]);
        }

        if (awardIndestructibleKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[9]);
        }

        if (awardLastDropKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[10]);
        }

        if (awardPhoenixKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[11]);
        }

        if (awardAceKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[12]);
        }

        if (awardEliteFighterKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[13]);
        }

        if (awardAirForceHeroKeyInt == 1)
        {
            earnedAwardsIndexes.Add(awardImages[14]);
        }


        // Step 3: Swap the images in awardSlots based on the list
        for (int i = 0; i < earnedAwardsIndexes.Count && i < awardSlots.Length; i++)
        {
            Image earnedAwardImage = earnedAwardsIndexes[i];
            awardSlots[i].sprite = earnedAwardImage.sprite;
            awardSlots[i].color = Color.white;
        }
    }

    private void UpdateFullHistory()
    {
        int totalMissions = PlayerPrefs.GetInt(GameManager.TotalMissionsKey, 0);
        int totalEnemiesDown = PlayerPrefs.GetInt(GameManager.TotalEnemiesDownKey, 0);
        float averageStrike;

        if (totalEnemiesDown > 0)
        {
            averageStrike = Mathf.Floor((float)totalEnemiesDown / totalMissions * 10) / 10.0f;
        }
        else
        {
            averageStrike = 0;
        }

        int shootingAccuracy = PlayerPrefs.GetInt(GameManager.ShootingAccuracyKey, 0);
        int totalAwards = PlayerPrefs.GetInt(GameManager.TotalAwardsKey, 0);
        float awardsPerMission;

        if (totalAwards > 0)
        {
            awardsPerMission = Mathf.Floor((float)totalAwards / totalMissions * 10) / 10.0f;
        }
        else
        {
            awardsPerMission = 0;
        }

        int collectedSupplies = PlayerPrefs.GetInt(GameManager.CollectedSuppliesKey, 0);
        int fullyUpgradedAircrafts = PlayerPrefs.GetInt(GameManager.FullyUpgradedAircraftsKey, 0);
        int kamikazeHits = PlayerPrefs.GetInt(GameManager.KamikazeHitsKey, 0);
        int phoenixAwards = PlayerPrefs.GetInt(GameManager.PhoenixAwardsKey, 0);
        int wavesSurvived = PlayerPrefs.GetInt(GameManager.WavesSurvivedKey, 0);
        int allAwardsInOneMission = PlayerPrefs.GetInt(GameManager.AllAwardsInOneMissionKey, 0);


        missionsText.text = $"Missions: {totalMissions}";
        enemiesDownText.text = $"Enemies Down: {totalEnemiesDown}";
        averageStrikeText.text = $"Average Strike: {averageStrike}";
        shootingAccuracyText.text = $"Shooting Accuracy: {shootingAccuracy}%";
        totalAwardsText.text = $"Total Awards: {totalAwards}";
        awardsPerMissionText.text = $"Awards Per Mission: {awardsPerMission}";
        collectedSuppliesText.text = $"Collected Supplies: {collectedSupplies}";
        fullyUpgradedAircraftsText.text = $"Fully Upgraded Aircrafts: {fullyUpgradedAircrafts}";
        kamikazeHitsText.text = $"Kamikaze Hits: {kamikazeHits}";
        phoenixAwardsText.text = $"Phoenix Awards: {phoenixAwards}";
        wavesSurvivedText.text = $"Waves Survived: {wavesSurvived}";
        allAwardsInOneMissionText.text = $"All Awards In One Mission: {allAwardsInOneMission}";

    }

    private void CheckLeaderboardNewEntry()
    {
        username = PlayerPrefs.GetString(GameManager.UserNameKey, string.Empty);
        playerHighScore = PlayerPrefs.GetInt(GameManager.HighScoreKey, 0);
        playerBestStrike = PlayerPrefs.GetInt(GameManager.BestStrikeKey, 0);
        playerLeaderboardScore = PlayerPrefs.GetInt(GameManager.LeaderboardScoreKey, 0);

        if (username != string.Empty && playerHighScore > 0 && playerHighScore > playerLeaderboardScore)
        {
            mainMenuUploadToLeaderboard.SetLeaderboardEntry(username, playerHighScore, playerBestStrike.ToString());
            PlayerPrefs.SetInt(GameManager.LeaderboardScoreKey, playerHighScore);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Leaderboard()
    {
        StartCoroutine(LeaderboardWaitForMouseClickSound());
    }

    private IEnumerator LeaderboardWaitForMouseClickSound()
    {
        SFXController.PlaySound("MouseClick");

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(2);
    }

    private void CleanRecords()
    {
        PlayerPrefs.SetString(GameManager.UserNameKey, string.Empty);

        PlayerPrefs.SetInt(GameManager.LeaderboardScoreKey, 0);

        PlayerPrefs.SetInt(GameManager.BestRankKey, 1);
        PlayerPrefs.SetInt(GameManager.HighScoreKey, 0);
        PlayerPrefs.SetInt(GameManager.BestStrikeKey, 0);
        PlayerPrefs.SetInt(GameManager.AwardsKey, 0);
        PlayerPrefs.SetString(GameManager.RankStringKey, "Second Lieutenant");

        PlayerPrefs.SetInt(GameManager.Award01, 0);
        PlayerPrefs.SetInt(GameManager.Award02, 0);
        PlayerPrefs.SetInt(GameManager.Award03, 0);
        PlayerPrefs.SetInt(GameManager.Award04, 0);
        PlayerPrefs.SetInt(GameManager.Award05, 0);
        PlayerPrefs.SetInt(GameManager.Award06, 0);
        PlayerPrefs.SetInt(GameManager.Award07, 0);
        PlayerPrefs.SetInt(GameManager.Award08, 0);
        PlayerPrefs.SetInt(GameManager.Award09, 0);
        PlayerPrefs.SetInt(GameManager.Award10, 0);
        PlayerPrefs.SetInt(GameManager.Award11, 0);
        PlayerPrefs.SetInt(GameManager.Award12, 0);
        PlayerPrefs.SetInt(GameManager.Award13, 0);
        PlayerPrefs.SetInt(GameManager.Award14, 0);
        PlayerPrefs.SetInt(GameManager.Award15, 0);

        PlayerPrefs.SetInt(GameManager.TotalMissionsKey, 0);
        PlayerPrefs.SetInt(GameManager.TotalEnemiesDownKey, 0);
        PlayerPrefs.SetInt(GameManager.HitShotsKey, 0);
        PlayerPrefs.SetInt(GameManager.MissedShotsKey, 0);
        PlayerPrefs.SetInt(GameManager.TotalShotsKey, 0);
        PlayerPrefs.SetInt(GameManager.ShootingAccuracyKey, 0);
        PlayerPrefs.SetInt(GameManager.TotalAwardsKey, 0);
        PlayerPrefs.SetInt(GameManager.CollectedSuppliesKey, 0);
        PlayerPrefs.SetInt(GameManager.KamikazeHitsKey, 0);
        PlayerPrefs.SetInt(GameManager.FullyUpgradedAircraftsKey, 0);
        PlayerPrefs.SetInt(GameManager.PhoenixAwardsKey, 0);
        PlayerPrefs.SetInt(GameManager.WavesSurvivedKey, 0);
        PlayerPrefs.SetInt(GameManager.AllAwardsInOneMissionKey, 0);

        PlayerPrefs.SetInt(GameManager.TotalMissionsForInterstitialAdsKey, 1);

        Debug.Log("Cleaned all the records");
    }
}
