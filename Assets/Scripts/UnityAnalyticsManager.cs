using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Analytics.Internal;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityAnalyticsManager : MonoBehaviour
{

    [SerializeField] private string termsOfServiceURL;
    [SerializeField] private string privacyPolicyURL;

    private int userConsent;
    private int currentSceneIndex;

    UserConsentManager userConsentManager;

    void Awake()
    {
        userConsent = PlayerPrefs.GetInt(GameManager.UserConsentKey, 0);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 0)
        {
            userConsentManager = FindObjectOfType<UserConsentManager>();
        }
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (userConsent == 0)
        {
            AskForConsent();
        }
        else
        {
            AnalyticsService.Instance.StartDataCollection();
        }
    }

    private void AskForConsent()
    {
        userConsentManager.userConsentPanel.SetActive(true);
    }

    public void OpenTermsOfService()
    {
        Application.OpenURL(termsOfServiceURL);
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyURL);
    }

    public void ConsentGiven()
    {
        PlayerPrefs.SetInt(GameManager.UserConsentKey, 1);
        AnalyticsService.Instance.StartDataCollection();
        userConsentManager.userConsentPanel.SetActive(false);
    }

    //Main Menu trackers below
    public void TrackNewGame()
    {
        AnalyticsService.Instance.RecordEvent("newGame");
    }

    public void TrackProfileView()
    {
        AnalyticsService.Instance.RecordEvent("profileView");
    }

    public void TrackControlsView()
    {
        AnalyticsService.Instance.RecordEvent("controlsView");
    }

    public void TrackAwardsListView()
    {
        AnalyticsService.Instance.RecordEvent("awardsListView");
    }

    //Leaderboard trackers below
    public void TrackLeaderboardView()
    {
        AnalyticsService.Instance.RecordEvent("leaderboardView");
    }

    public void TrackMainMenuFromLeaderboard()
    {
        AnalyticsService.Instance.RecordEvent("mainMenuFromLeaderboard");
    }

    //Mission trackers below
    public void TrackContinueGame()
    {
        AnalyticsService.Instance.RecordEvent("continueGame");
    }

    public void TrackNewGameFromMission()
    {
        AnalyticsService.Instance.RecordEvent("newGameFromMission");
    }

    public void TrackMainMenuFromMission()
    {
        AnalyticsService.Instance.RecordEvent("mainMenuFromMission");
    }

    public void TrackMissionAborted()
    {
        AnalyticsService.Instance.RecordEvent("missionAborted");
    }

    public void TrackAwardReceived(string awardName)
    {
        EventAwardReceived awardReceived = new EventAwardReceived
        {
            AwardName = awardName
        };

        AnalyticsService.Instance.RecordEvent(awardReceived);
    }

    public void TrackRankPromotion(string rankName)
    {
        EventRankPromotion rankPromotion = new EventRankPromotion
        {
            RankName = rankName
        };

        AnalyticsService.Instance.RecordEvent(rankPromotion);
    }

    public void TrackAircraftProgress(int aircraftLevel)
    {
        EventAircraftProgress aircraftProgress = new EventAircraftProgress
        {
            AircraftLevelString = $"Level {aircraftLevel}"
        };

        AnalyticsService.Instance.RecordEvent(aircraftProgress);
    }

    public void TrackArtilleryProgress(int artilleryLevel)
    {
        EventArtilleryProgress artilleryProgress = new EventArtilleryProgress
        {
            ArtilleryLevelString = $"Level {artilleryLevel}"
        };

        AnalyticsService.Instance.RecordEvent(artilleryProgress);
    }

    public void TrackSurvivedWave(int waveNumber)
    {
        EventSurvivedWave survivedWave = new EventSurvivedWave
        {
            WaveNumber = $"Wave {waveNumber}"
        };

        AnalyticsService.Instance.RecordEvent(survivedWave);
    }
}

class InitWithEnvironment : MonoBehaviour
{
    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
}
