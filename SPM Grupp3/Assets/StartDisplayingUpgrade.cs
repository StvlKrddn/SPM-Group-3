using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStartDisplayingUpgrade : MonoBehaviour
{
    public TutorialUI theTutorial;
    // Start is called before the first frame update
    void Start()
    {
        theTutorial.stopDisablingUpgradeButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
