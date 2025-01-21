using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoInternetLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject noInternetPanel;

    private void Start()
    {
        if (!GameManager.hasNetwork)
        {
            noInternetPanel.SetActive(true);
        }
    }

    public void TryInternetAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
