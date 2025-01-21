using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string iOSGameId;
    [SerializeField] private string androidGameId;
    [SerializeField] private bool testMode;
    [SerializeField] private string iOSAdUnitId;
    [SerializeField] private string androidAdUnitId;

    public static RewardedAdsManager Instance;

    private string gameId;
    private string adUnitId;

    private GameOverHandler gameOverHandler;

    private void Awake()
    {
        if (GameManager.hasNetwork)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
#if !UNITY_WEBGL
                InitializeAds();
#endif

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        gameId = iOSGameId;
        adUnitId = iOSAdUnitId;
#elif UNITY_ANDROID
        gameId = androidGameId;
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        gameId = androidGameId;
        adUnitId = androidAdUnitId;
#endif

        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            gameOverHandler.ContinueGame();
        }
    }

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        if (GameManager.hasNetwork)
        {
#if !UNITY_WEBGL
            this.gameOverHandler = gameOverHandler;

            Advertisement.Load(adUnitId, this);
#endif
        }
    }
}
