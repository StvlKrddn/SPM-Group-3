using System;

// Kan fyllas med fler achievements om vi hinner
[Serializable]
public struct AchievementData
{
    public bool CompletedStageThree;

    public AchievementData(bool completedStageThree)
    {
        CompletedStageThree = completedStageThree;
    }
}
