using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveNotice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.RegisterListener<StartWaveEvent>(NewWave);
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
