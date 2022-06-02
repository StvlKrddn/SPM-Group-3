using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        if (gameObject.name.Equals("MusicVolume"))
        {
            MusicManager.instance.MusicSlider = slider;
            slider.onValueChanged.AddListener(delegate { MusicManager.instance.SetMusicVolume(); });
        }
        else if (gameObject.name.Equals("EffectVolume"))
        {
            SoundSystem.instance.EffectSlider = slider;
            slider.onValueChanged.AddListener(delegate { SoundSystem.instance.SetVolume(); });
        }
    }
}
