using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroFade : MonoBehaviour
{
    public static bool gameIntro = false;

    private void Awake()
    {
        if (!gameIntro)
        {
            return;
        }
        else
        {
            TurnThisOff();
        }
    }

    public void TurnThisOff()
    {
        this.gameObject.SetActive(false);
        gameIntro = true;
    }
}
