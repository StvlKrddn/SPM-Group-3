using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndSystem : MonoBehaviour
{
    private WaveManager waveManager;

    private TankUpgradeTree tank;
    //private SniperTank sniperTank; 
    private TutorialUI tutorial;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        if(FindObjectOfType<TutorialUI>())
        {
            tutorial = FindObjectOfType<TutorialUI>();
        }
        //player =
        
        if(FindObjectOfType<TankUpgradeTree>())
        {
            tank = FindObjectOfType<TankUpgradeTree>();
        }

        // If any DebugEvent is invoked, call the PrintMessage-method
        EventHandler.Instance.RegisterListener<WaveEndEvent>(waveDone);


        
    }




    void waveDone(WaveEndEvent waveEndEvent)
    {
        // waveManager.StartWave(waveEndEvent.CurrentWave);
        if (tutorial != null)
        {
            tutorial.waveEnded();
        }

        if(tank != null)
        {
            tank.ResetCooldown();
        }
        EventHandler.Instance.InvokeEvent(new SaveGameEvent(
            description: "Game is saving"
        ));
    }

}