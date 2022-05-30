using System.Collections.Generic;
using UnityEngine;

// When this script is attached to an object, an AudioSource is automatically also added, and cannot be removed.
[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    [SerializeField] private int maxBufferSize = 2;

    private AudioSource audioSource;
    private List<AudioSource> soundBuffer = new List<AudioSource>();


    void Start()
    {
        EventHandler.RegisterListener<DieEvent>(PlaySound);

        audioSource = GetComponent<AudioSource>();
    }


    void PlaySound(DieEvent eventInfo)
    {
        AudioClip clip = eventInfo.DeathSounds[Random.Range(0, eventInfo.DeathSounds.Length)];

        // If buffer is smaller than max size, play another sound
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
