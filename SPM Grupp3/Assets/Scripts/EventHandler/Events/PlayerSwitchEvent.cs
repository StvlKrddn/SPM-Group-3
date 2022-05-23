using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchEvent : Event
{
    public GameObject PlayerContainer;
    
    public PlayerSwitchEvent(string description, GameObject playerContainer) : base(description)
    {
        PlayerContainer = playerContainer;
    }
}
