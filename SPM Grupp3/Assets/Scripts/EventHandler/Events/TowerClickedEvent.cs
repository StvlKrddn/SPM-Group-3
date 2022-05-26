using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClickedEvent : Event
{
    public GameObject towerClicked;
    public GameObject placementClicked;

    public TowerClickedEvent(string description, GameObject towerClicked, GameObject placementClicked) : base(description)
    {
        this.towerClicked = towerClicked;
        this.placementClicked = placementClicked;
    }
}
