using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private ScriptableObject easyDifficulty;
    [SerializeField] private ScriptableObject mediumDifficulty;
    [SerializeField] private ScriptableObject hardDifficulty;

    public GameController gameController;
    
    public void PlaceTargets(Difficulty difficulty)
    {
        //Add code to set up the targets depending on difficulty
    }

    public void RemoveTargets()
    {
        //Add code to remove all the targets
    }
}
