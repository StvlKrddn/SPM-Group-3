using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //transform.GetChild(0).gameObject.SetActive(true);
        transform.GetComponentInChildren<Image>().enabled = true;
        Invoke(nameof(Duration), 3f);
    }

    void Duration()
    {
        //transform.GetChild(0).gameObject.SetActive(false);
        transform.GetComponentInChildren<Image>().enabled = false;
    }
}
