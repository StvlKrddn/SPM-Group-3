using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSystem : MonoBehaviour
{
    private WaveManager waveManager;
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        // If any DieEvent is invoked, call the OnObjectExploded-method
        EventHandler.Instance.RegisterListener<DieEvent>(OnObjectExploded);
    }
    
    private void OnObjectExploded(DieEvent eventInfo)
    {
        waveManager.WaveUpdate();
    }
}
