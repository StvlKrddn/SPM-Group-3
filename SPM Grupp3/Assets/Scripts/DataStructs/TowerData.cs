using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TowerData
{
    public string towerType;
    public int level;
    public SerializableVector3 position;

    public TowerData(GameObject tower)
    {
        Tower towerComponent = tower.GetComponent<Tower>();
        towerType = towerComponent.tower.name;
        level = towerComponent.level;
        position = tower.transform.position;
    }
}
