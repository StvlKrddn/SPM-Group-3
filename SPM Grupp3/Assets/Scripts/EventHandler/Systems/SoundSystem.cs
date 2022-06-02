using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// When this script is attached to an object, an AudioSource is automatically also added, and cannot be removed.
[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;
    [SerializeField] private int maxBufferSize = 2;

    public AudioSource audioSource;
    private List<AudioSource> soundBuffer = new List<AudioSource>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        EventHandler.RegisterListener<PlaySoundEvent>(PlaySound);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume");
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        audioSource.volume = volume;
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

    public void StopAudio()
    {
        audioSource.Stop();
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
