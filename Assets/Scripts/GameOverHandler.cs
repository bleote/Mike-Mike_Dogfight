using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject continueButtonBlocker;
    [SerializeField] private ArmorBar armorBar;

    private void Start()
    {
        if (!GameManager.hasNetwork)
        {
            continueButtonBlocker.SetActive(true);
            continueButton.interactable = false;
        }
    }

    public void ContinueButton()
    {
        if (GameManager.hasNetwork)
        {
#if !UNITY_WEBGL
            RewardedAdsManager.Instance.ShowAd(this);
#endif
            continueButton.interactable = false;
        }
    }

    public void NewGame()
    {
        MissionsInterstitialUpdate();
        StartCoroutine(NewGameWaitForMouseClickSound());
    }

    private IEnumerator NewGameWaitForMouseClickSound()
    {
        SFXController.PlaySound("MouseClick");

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        MissionsInterstitialUpdate();
        StartCoroutine(MainMenuWaitForMouseClickSound());
    }

    private IEnumerator MainMenuWaitForMouseClickSound()
    {
        SFXController.PlaySound("MouseClick");

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(0);
    }

    //Set all the conditions for the game to continue from where it was, with a restabilished Aircraft
    public void ContinueGame()
    {
        playerGO.SetActive(true);
        GameManager.totalArmor = 1f;
        GameManager.totalFuel = 1f;
        armorBar.DamageBar(-1);
        PlayerController.gameOn = true;
        PlayerController.gameContinue = true;
        continueButtonBlocker.SetActive(true);
        finalPanel.SetActive(false);

        if (SFXController.musicOn)
        {
            SFXController.PlayMusic("BGMusic");
        }

        if (SFXController.sfxOn)
        {
            SFXController.PlaySound("EngineSound");
        }

        if (GameManager.criticalArmor == true && GameManager.awardPhoenix == false)
        {
            GameManager.criticalArmor = false;
        }
    }

    private void MissionsInterstitialUpdate()
    {
#if !UNITY_WEBGL
        if (GameManager.hasNetwork)
        {
            int currentMissionsInterstitial = PlayerPrefs.GetInt(GameManager.TotalMissionsForInterstitialAdsKey, 1);

            if (currentMissionsInterstitial == 1)
            {
                PlayerPrefs.SetInt(GameManager.TotalMissionsForInterstitialAdsKey, 2);
            }
            else
            {
                PlayerPrefs.SetInt(GameManager.TotalMissionsForInterstitialAdsKey, 1);
            }
        }
#endif
    }
}
