using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    AudioSource mouseClick;

    private void Start()
    {
        mouseClick = GetComponent<AudioSource>();
    }
   
    public void MouseClickSound()
    {
        if (SFXController.sfxOn)
        {
            mouseClick.Play();
        }
    }
}
