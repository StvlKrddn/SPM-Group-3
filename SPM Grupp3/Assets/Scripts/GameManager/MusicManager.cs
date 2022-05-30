using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    private Slider musicSlider;

    public Slider MusicSlider { get { return musicSlider; } set { musicSlider = value; } }
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        audioSource = transform.GetComponent<AudioSource>();
    }

    public void SetMusicVolume()
    {
        audioSource.volume = musicSlider.value;
    }
}
