using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSystem : MonoBehaviour
{
    void Start()
    {
        // If any DieEvent is invoked, call the OnObjectExploded-method
        EventHandler.Instance.RegisterListener<DieEvent>(OnObjectExploded);
    }
    
    private void OnObjectExploded(DieEvent eventInfo)
    {
        Destroy(eventInfo.Invoker);
    }
}
