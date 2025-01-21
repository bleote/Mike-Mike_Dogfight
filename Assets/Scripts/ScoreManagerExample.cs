using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEditor;

public class ScoreManagerExample : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private TMP_InputField inputNewScore;
    [SerializeField] private TMP_InputField inputNewStrike;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text bestStrikeText;

    int playerHighScore;
    int playerBestStrike;

    public UnityEvent<string, int, string> submitScoreEvent;

    private void Start()
    {
        playerHighScore = PlayerPrefs.GetInt(GameManager.HighScoreKey, 0);
        playerBestStrike = PlayerPrefs.GetInt(GameManager.BestStrikeKey, 0);
        highScoreText.text = $"High Score: {playerHighScore}";
        bestStrikeText.text = $"Best Strike: {playerBestStrike}";
    }

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputUsername.text, int.Parse(inputNewScore.text), inputNewStrike.text);
    }
}
