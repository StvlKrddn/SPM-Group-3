using System;
using System.Collections.Generic;

[Serializable]
public struct SaveData
{
    public float Money, Material, CurrentBaseHealth;
    public int CurrentWave, EnemiesKilled, MoneyCollected, MaterialCollected, CurrentScene, TankUpgradeLevel;
    public PlayerMode StartingMode;
    public List<TowerData> TowerData;
    public SaveData(int currentWave, int enemiesKilled, int moneyCollected, int materialCollected, float money, float material, float baseHealth, int currentScene, PlayerMode startingMode, List<PlacedTower> towersPlaced, int tankUpgradeLevel)
    {
        TankUpgradeLevel = tankUpgradeLevel;
        Money = money;
        Material = material;
        CurrentBaseHealth = baseHealth;
        CurrentWave = currentWave;

        EnemiesKilled = enemiesKilled;
        MoneyCollected = moneyCollected;
        MaterialCollected = materialCollected;

        CurrentScene = currentScene;

        StartingMode = startingMode;
        TowerData = new List<TowerData>();
        
        foreach (PlacedTower tower in towersPlaced)
        {
            TowerData.Add(new TowerData(tower));
        }
    }
}
