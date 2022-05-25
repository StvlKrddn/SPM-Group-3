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

    public TowerData(PlacedTower tower)
    {
        towerType = tower.tower.name;
        level = tower.upgradesPurchased;
        position = tower.tower.transform.position;
        Switch();
        Debug.Log(towerType);
    }

    private void Switch()
    {
        if (towerType.Contains("CanonTower"))
        {
            towerType = "CanonTower";
        }
        else if (towerType.Contains("MissileTower"))
        {
            towerType = "MissileTower";
        }
        else if (towerType.Contains("SlowTower"))
        {
            towerType = "SlowTower";
        }
        else if (towerType.Contains("PoisonTower"))
        {
            towerType = "PoisonTower";
        }
    }
}
