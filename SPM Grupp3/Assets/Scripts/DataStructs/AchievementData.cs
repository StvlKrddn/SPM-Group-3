using System;

// Kan fyllas med fler achievements om vi hinner
[Serializable]
public struct AchievementData
{
    public Achievement CompletedStageThree;

    public AchievementData(Achievement completedStageThree)
    {
        CompletedStageThree = completedStageThree;
    }
}
