using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> usernames;
    [SerializeField] private List<TextMeshProUGUI> scores;
    [SerializeField] private List<TextMeshProUGUI> strikes;

    string username;
    int playerHighScore;
    int playerBestStrike;
    int playerLeaderboardScore;

    private string publicLeaderboardKey = "4aee29268c5d1bc973f1c78f2421f4df9dc0d52cef8ab441303da1543f5133cc";

    UnityAnalyticsManager unityAnalyticsManager;

    private void Awake()
    {
        unityAnalyticsManager = FindObjectOfType<UnityAnalyticsManager>();
    }

    private void Start()
    {
        unityAnalyticsManager.TrackLeaderboardView();

        CheckForMissingLeaderboardEntry();
        CheckMusicSettings();

        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < usernames.Count) ? msg.Length : usernames.Count;
            for (int i = 0; i < loopLength; i++)
            {
                usernames[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                strikes[i].text = msg[i].Extra;
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score, string extra)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, extra, ((msg) => 
        {
            GetLeaderboard();
        }));

        //LeaderboardCreator.ResetPlayer();
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuWaitForMouseClickSound());
    }

    private IEnumerator MainMenuWaitForMouseClickSound()
    {
        SFXController.PlaySound("MouseClick");

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(0);
    }

    private void CheckForMissingLeaderboardEntry()
    {
        username = PlayerPrefs.GetString(GameManager.UserNameKey, string.Empty);
        playerHighScore = PlayerPrefs.GetInt(GameManager.HighScoreKey, 0);
        playerBestStrike = PlayerPrefs.GetInt(GameManager.BestStrikeKey, 0);
        playerLeaderboardScore = PlayerPrefs.GetInt(GameManager.LeaderboardScoreKey, 0);

        if (playerHighScore > playerLeaderboardScore)
        {
            SetLeaderboardEntry(username, playerHighScore, playerBestStrike.ToString());
            PlayerPrefs.SetInt(GameManager.LeaderboardScoreKey, playerHighScore);
        }
    }

    private void CheckMusicSettings()
    {
        AudioSource bgMusic = GameObject.FindGameObjectWithTag("BGMusic").GetComponent<AudioSource>();
        if (SFXController.musicOn)
        {
            bgMusic.Play();
        }
    }
}
