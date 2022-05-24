using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public float money, material, currentBaseHealth;
    public int currentWave, enemiesKilled, moneyCollected, materialCollected, currentScene;
    public SerializableColor player1Color, player2Color;
    public PlayerMode startingMode;
    public List<TowerData> towerData;
    public SaveData(int currentWave, int enemiesKilled, int moneyCollected, int materialCollected, float money, float material, float baseHealth, int currentScene, Color player1Color, Color player2Color, PlayerMode startingMode, List<GameObject> towersPlaced)
    {
        this.money = money;
        this.material = material;
        this.currentBaseHealth = baseHealth;
        this.currentWave = currentWave;

        this.enemiesKilled = enemiesKilled;
        this.moneyCollected = moneyCollected;
        this.materialCollected = materialCollected;

        this.currentScene = currentScene;

        this.player1Color = player1Color;
        this.player2Color = player2Color;

        this.startingMode = startingMode;

        towerData = new List<TowerData>();
        foreach (GameObject tower in towersPlaced)
        {
            towerData.Add(new TowerData());
        }
    }
}
