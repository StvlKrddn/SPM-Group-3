using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinedEvent : Event
{
    public GameObject NewPlayer;

    public PlayerJoinedEvent(string description, GameObject newPlayerGO) : base(description)
    {
        NewPlayer = newPlayerGO;
    }
}
