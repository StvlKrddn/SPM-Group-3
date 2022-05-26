using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        EventHandler.RegisterListener<NewWaveEvent>(NextWave);
    }

    void NextWave(NewWaveEvent newWaveEvent)
    {
        waveManager.StartWave(newWaveEvent.CurrentWave);
    }
}
