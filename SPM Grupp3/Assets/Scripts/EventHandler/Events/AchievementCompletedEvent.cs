using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCompletedEvent : Event
{
    public Achievement AchievementType;
    public bool Completed;

    public AchievementCompletedEvent(string description, Achievement achievementType, bool completed) : base(description)
    {
        AchievementType = achievementType;
        Completed = completed;
    }
}
