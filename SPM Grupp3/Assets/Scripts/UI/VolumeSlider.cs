using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        if (gameObject.name.Equals("MusicVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
            slider.onValueChanged.AddListener(delegate { MusicManager.instance.SetMusicVolume(slider.value); });
        }
        else if (gameObject.name.Equals("EffectVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("EffectsVolume");
            slider.onValueChanged.AddListener(delegate { SoundSystem.instance.SetVolume(slider.value); });
        }
    }
}
