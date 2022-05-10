using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveEvent : Event
{
    public GameObject Invoker;
    
    public StartWaveEvent(string description, GameObject invoker) : base(description)
    {
        Invoker = invoker;
    }
}
