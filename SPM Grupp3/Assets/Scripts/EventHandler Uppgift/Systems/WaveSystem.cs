using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        // If any DebugEvent is invoked, call the PrintMessage-method
        EventHandler.Instance.RegisterListener<NewWaveEvent>(NextWave);
    }

    void NextWave(NewWaveEvent newWaveEvent)
    {
        waveManager.StartWave(newWaveEvent.CurrentWave);
    }
}
