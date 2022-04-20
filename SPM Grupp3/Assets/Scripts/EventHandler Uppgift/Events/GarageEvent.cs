using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageEvent : Event
{
    public GameObject Player;

    public GarageEvent(string description, GameObject player) : base(description)
    {
        Player = player;
    }
}
