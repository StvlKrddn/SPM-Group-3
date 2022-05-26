using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveNotice : MonoBehaviour
{

    private void OnEnable()
    {
        EventHandler.RegisterListener<StartWaveEvent>(NewWave);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterListener<StartWaveEvent>(NewWave);
    }

    void NewWave(StartWaveEvent eventInfo)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Invoke(nameof(Duration), 3f);
    }

    void Duration()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
