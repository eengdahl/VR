using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAchievementGiver //should be on things that are used to unlock achievements
{
    void AchievementReached();
}

[System.Serializable]
public class Achievement //the name of the achievement and how many it takes to unlock it
{
    [Tooltip("The Name of the Achievement. Make sure this matches the name in allAchievements list!")]
    public string name;
    [Tooltip("If several things needs to be hit, otherwise, keep this at 1, NOT 0!")]
    public int requirement = 1;
    [Tooltip("Tracks progress, don't touch this")]
    public int progress;
}

public class AchievementHandler : MonoBehaviour
{
    [SerializeField] List<Achievement> allAchievements = new(); //all possible achievements
    List<Achievement> unlockedAchievements = new(); //achievements we've unlocked

    public void ReceiveAchievement(string achName)
    {
        foreach (var achievement in allAchievements) //loop through all achievements
        {
            if (achName == achievement.name) //find the relevant achievement
            {
                achievement.progress++;
                if (achievement.progress == achievement.requirement) //if progress has reached requirement, we unlock it
                    UnlockAchievement(achievement);
            }
        }
    }

    void UnlockAchievement(Achievement achievement)
    {
        unlockedAchievements.Add(achievement);
        print(achievement.name);
    }

    public void SendAchievements()
    {
        List<string> unlocked = new();

        foreach (var item in unlockedAchievements)
            unlocked.Add(item.name);

        //GameObject.FindObjectOfType<ScoreController>().DisplayAchievements(unlocked, allAchievements.Count);
    }

    public void ResetAchievements()
    {
        foreach (var item in allAchievements)
        {
            item.progress = 0;
        }

        unlockedAchievements.Clear();
    }
}
