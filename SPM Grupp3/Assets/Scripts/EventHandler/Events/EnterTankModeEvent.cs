using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTankModeEvent : Event
{
    public GameObject Player;

    public EnterTankModeEvent(string description, GameObject playerContainer) : base(description)
    {
        Player = playerContainer;
    }
}
