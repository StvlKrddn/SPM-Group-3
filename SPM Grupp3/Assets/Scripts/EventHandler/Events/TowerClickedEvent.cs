using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClickedEvent : Event
{
    public GameObject towerClicked;

    public TowerClickedEvent(string description, GameObject towerClicked) : base(description)
    {
        this.towerClicked = towerClicked;
    }
}
