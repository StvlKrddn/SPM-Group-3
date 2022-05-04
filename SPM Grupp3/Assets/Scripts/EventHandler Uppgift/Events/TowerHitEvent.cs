using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHitEvent : Event
{
    public GameObject towerGO;
    public GameObject hitEffect;
    public GameObject enemyHit;

    public TowerHitEvent(string description, GameObject towerGO, GameObject hitEffect, GameObject enemyHit) : base(description)
    {
        this.towerGO = towerGO;
        this.hitEffect = hitEffect;
        this.enemyHit = enemyHit;
    }
}
