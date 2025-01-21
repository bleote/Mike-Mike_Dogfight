using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseCover;
    [SerializeField] private TMP_Text gamePausedText;
    [SerializeField] private GameObject quitButtonOne;
    [SerializeField] private GameObject quitButtonTwo;

    Rigidbody2D player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().GetComponent<Rigidbody2D>();
    }

    public void PauseGameSwitch()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            if (SFXController.musicOn)
            {
                SFXController.StopMusic("BGMusic");
            }

            if (SFXController.sfxOn)
            {
                SFXController.StopSound("EngineSound");
                SFXController.StopSound("Gogogo");
                SFXController.StopSound("SpeedUp");
                SFXController.StopSound("SpeedDown");
            }

            quitButtonOne.SetActive(true);
            quitButtonTwo.SetActive(false);
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            if (SFXController.musicOn && PlayerController.gameOn)
            {
                SFXController.PlayMusic("BGMusic");
            }

            if (SFXController.sfxOn)
            {
                SFXController.PlaySound("EngineSound");

                if (!PlayerController.gameOn)
                {
                    SFXController.PlaySound("Gogogo");
                }
            }
        }

        gamePausedText.text = "Game Paused";
    }

    public void QuitButtonOne()
    {
        quitButtonOne.SetActive(false);
        gamePausedText.text = "Quit the game?";
        quitButtonTwo.SetActive(true);
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuWaitForMouseClickSound());
    }

    private IEnumerator MainMenuWaitForMouseClickSound()
    {
        SFXController.PlaySound("MouseClick");
        
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        yield return new WaitForSeconds(0.05f);

        SceneManager.LoadScene(0);
    }

    public void PausePanelSwitch()
    {
        if (player != null)
        {
            if (pausePanel.activeInHierarchy == true)
            {
                pausePanel.SetActive(false);
                pauseCover.SetActive(false);
                gamePausedText.text = "Game Paused";
            }
            else
            {
                pausePanel.SetActive(true);
                pauseCover.SetActive(true);
            }
        }
    }
}
