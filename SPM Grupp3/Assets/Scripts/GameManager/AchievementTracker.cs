using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Achievement
{
    CompleteStageThree
}

public class AchievementTracker : MonoBehaviour
{
    private static AchievementTracker instance;
    
    // Keeps track of completed achievements
    private Dictionary<Achievement, bool> achievementDictionary;

    public Dictionary<Achievement, bool> AchievementDictionary { get { return achievementDictionary; } }
    public static AchievementTracker Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AchievementTracker>();
            }
            return instance;
        }
    }

    void Awake()
    {
        EventHandler.RegisterListener<AchievementCompletedEvent>(SaveAchievements);

        if (DataManager.FileExists(DataManager.AchievementData))
        {
            Dictionary<Achievement, bool> data = (Dictionary<Achievement, bool>) DataManager.ReadFromFile(DataManager.AchievementData);

            achievementDictionary = new Dictionary<Achievement, bool>(data);
        }
        else 
        {
            achievementDictionary = new Dictionary<Achievement, bool>();
        }
    }

    void SaveAchievements(AchievementCompletedEvent eventInfo)
    {
        Achievement achievementType = eventInfo.AchievementType;
        bool completed = eventInfo.Completed;
        if (achievementDictionary.ContainsKey(achievementType))
        {
            achievementDictionary[achievementType] = completed;
        }
        else
        {
            achievementDictionary.Add(achievementType, completed);
        }

        DataManager.WriteToFile(achievementDictionary, DataManager.AchievementData);
    }

    public bool IsAchievementCompleted(Achievement achievement)
    {
        return achievementDictionary.ContainsKey(achievement);
    }
}
