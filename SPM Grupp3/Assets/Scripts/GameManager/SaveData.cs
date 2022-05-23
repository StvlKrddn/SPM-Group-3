using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public float money, material, currentBaseHealth;
    public int currentWave, enemiesKilled, moneyCollected, materialCollected, currentScene;

    public SaveData(int currentWave, int enemiesKilled, int moneyCollected, int materialCollected, float money, float material, float baseHealth, int currentScene)
    {
        this.money = money;
        this.material = material;
        this.currentBaseHealth = baseHealth;
        this.currentWave = currentWave;
        this.enemiesKilled = enemiesKilled;
        this.moneyCollected = moneyCollected;
        this.materialCollected = materialCollected;
        this.currentScene = currentScene;
    }
}
