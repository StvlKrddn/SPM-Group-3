using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickedEvent : Event
{
    public GameObject Clicker;
    
    public UIClickedEvent(string description, GameObject clicker) : base(description)
    {
        Clicker = clicker;
    }
}
