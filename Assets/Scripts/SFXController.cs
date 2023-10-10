using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static void PlaySound(string sfx)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Play();
    }

    public static void StopSound(string sfx)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Stop();
    }

    public static void ExplosionSound(int number)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag("Explosion" + number);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Play();
    }
}
