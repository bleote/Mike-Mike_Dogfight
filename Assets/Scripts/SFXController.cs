using UnityEngine;

public class SFXController : MonoBehaviour
{
    public static bool musicOn;
    public static bool sfxOn;
    public static int scene = 1;

    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject objectMusicOn;
    [SerializeField] private GameObject objectMusicOff;
    [SerializeField] private GameObject objectSfxOn;
    [SerializeField] private GameObject objectSfxOff;

    Rigidbody2D player;

    private void Start()
    {
        if (scene == 2)
        {
            player = FindAnyObjectByType<PlayerController>().GetComponent<Rigidbody2D>();
        }
    }

    public static void PlaySound(string sfx)
    {
        if (sfxOn)
        {
            GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
            AudioSource source = soundObject.GetComponent<AudioSource>();
            source.Play();
        }
    }

    public static void StopSound(string sfx)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag(sfx);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Stop();
    }

    public static void PlayMusic(string music)
    {
        if (musicOn)
        {
            GameObject soundObject = GameObject.FindGameObjectWithTag(music);
            AudioSource source = soundObject.GetComponent<AudioSource>();
            source.Play();
        }
    }

    public static void StopMusic(string music)
    {
        GameObject soundObject = GameObject.FindGameObjectWithTag(music);
        AudioSource source = soundObject.GetComponent<AudioSource>();
        source.Stop();
    }

    public static void ExplosionSound(int number)
    {
        if (sfxOn)
        {
            GameObject soundObject = GameObject.FindGameObjectWithTag("Explosion" + number);
            AudioSource source = soundObject.GetComponent<AudioSource>();
            source.Play();
        }
    }

    public void MusicSwitch()
    {
        if (musicOn)
        {
            musicOn = false;
            PlayerPrefs.SetInt(GameManager.MusicKey, 1);
            StopMusic("BGMusic");
            objectMusicOn.SetActive(false);
            objectMusicOff.SetActive(true);
        }
        else
        {
            musicOn = true;
            PlayerPrefs.SetInt(GameManager.MusicKey, 0);
            PlayMusic("BGMusic");
            objectMusicOff.SetActive(false);
            objectMusicOn.SetActive(true);
        }
    }

    public void SfxSwitch()
    {
        if (sfxOn)
        {
            sfxOn = false;
            PlayerPrefs.SetInt(GameManager.SFXKey, 1);

            if (scene == 2)
            {
                StopSound("EngineSound");
                StopSound("Gogogo");
            }

            objectSfxOn.SetActive(false);
            objectSfxOff.SetActive(true);
        }
        else
        {
            sfxOn = true;
            PlayerPrefs.SetInt(GameManager.SFXKey, 0);

            if (scene == 2)
            {
                PlaySound("EngineSound");
            }

            objectSfxOff.SetActive(false);
            objectSfxOn.SetActive(true);
        }
    }

    public void SoundPanelSwitch()
    {
        if (player != null)
        {
            if (soundPanel.activeInHierarchy == true)
            {
                soundPanel.SetActive(false);
            }
            else
            {
                soundPanel.SetActive(true);
            }
        }
    }
}
