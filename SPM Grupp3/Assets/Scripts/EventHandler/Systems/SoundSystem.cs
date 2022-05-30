using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// When this script is attached to an object, an AudioSource is automatically also added, and cannot be removed.
[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    [SerializeField] private int maxBufferSize = 2;

    private AudioSource audioSource;
    private List<AudioSource> soundBuffer = new List<AudioSource>();

    private Slider effectSlider;


    void Start()
    {       
        effectSlider = GameObject.Find("EffectVolume").GetComponent<Slider>();
        effectSlider.onValueChanged.AddListener(delegate { SetVolume(); });
        // If any DieEvent is invoked, call the OnObjectExploded-method
        EventHandler.RegisterListener<PlaySoundEvent>(PlaySound);

        audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume()
    {
        audioSource.volume = effectSlider.value;
    }

    void PlaySound(PlaySoundEvent eventInfo)
    {
        AudioClip clip = eventInfo.sound;
        if (soundBuffer.Count < maxBufferSize)
        {
            audioSource.PlayOneShot(clip);
            soundBuffer.Add(audioSource);
        }
    }
    
    void Update()
    {
        // Clear sound buffer every half-second
        // This means that if more than maxBufferSize-amount of sounds are added to the buffer within half a second,
        // only the maxBufferSize-amount of them are played
        InvokeRepeating(nameof(ClearBuffer), 0, 0.5f);
    }

    void ClearBuffer()
    {
        soundBuffer.Clear();
    }
}
