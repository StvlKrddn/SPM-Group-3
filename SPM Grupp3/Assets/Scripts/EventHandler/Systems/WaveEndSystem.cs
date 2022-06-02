using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndSystem : MonoBehaviour
{
    private WaveManager waveManager;

    //private SniperTank sniperTank; 
    private TutorialUI tutorial;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        if(FindObjectOfType<TutorialUI>())
        {
            tutorial = FindObjectOfType<TutorialUI>();
        }

        // If any DebugEvent is invoked, call the PrintMessage-method
        EventHandler.RegisterListener<WaveEndEvent>(WaveDone);
    }




    void WaveDone(WaveEndEvent waveEndEvent)
    {
        // waveManager.StartWave(waveEndEvent.CurrentWave);
        if (tutorial != null)
        {
            tutorial.waveEnded();
        }

        EventHandler.InvokeEvent(new SaveGameEvent(
            description: "Game is saving"
        ));
    }

}
