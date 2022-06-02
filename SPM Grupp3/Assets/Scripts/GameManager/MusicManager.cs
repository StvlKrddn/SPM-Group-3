using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetMusicPLay(bool state)
    {
        if(state)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        audioSource.volume = volume;
    }
}
