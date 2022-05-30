using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Slider musicSlider;
    private void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        musicSlider = GameObject.Find("MusicVolume").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });

    }

    public void SetMusicVolume()
    {
        audioSource.volume = musicSlider.value;
    }
}
