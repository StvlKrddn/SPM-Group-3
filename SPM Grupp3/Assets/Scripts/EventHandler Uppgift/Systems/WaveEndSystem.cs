using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private WaveManager waveManager;
    private TutorialUI tutorial;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        if(FindObjectOfType<TutorialUI>())
        {
            tutorial = FindObjectOfType<TutorialUI>();
        }
        // If any DebugEvent is invoked, call the PrintMessage-method
        EventHandler.Instance.RegisterListener<WaveEndEvent>(waveDone);
    }

    void waveDone(WaveEndEvent waveEndEvent)
    {
       // waveManager.StartWave(waveEndEvent.CurrentWave);

        tutorial.waveEnded();
    }
}
