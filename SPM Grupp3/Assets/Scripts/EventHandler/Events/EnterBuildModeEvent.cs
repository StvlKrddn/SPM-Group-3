using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuildModeEvent : Event
{
    public GameObject Player;

    public EnterBuildModeEvent(string description, GameObject player) : base(description)
    {
        Player = player;
    }
}
