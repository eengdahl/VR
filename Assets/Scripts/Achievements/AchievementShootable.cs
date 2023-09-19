using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementShootable : MonoBehaviour, IAchievementGiver
{
    [Tooltip("Make sure this matches with a name in allAchievements !")]
    [SerializeField] string achName = "";

    public void AchievementReached()
    {
        //print("sending achievement: " + achName);
        GameObject.FindObjectOfType<AchievementHandler>().ReceiveAchievement(achName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            AchievementReached();
        }
    }
}
