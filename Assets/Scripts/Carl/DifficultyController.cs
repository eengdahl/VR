using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    // THIS SCRIPT IS NOT USED ANYWHERE
    // IT REMAINS AS A TESTAMENT TO OVERDESIGN
    
    [SerializeField] private ScriptableObject easyDifficulty;
    [SerializeField] private ScriptableObject mediumDifficulty;
    [SerializeField] private ScriptableObject hardDifficulty;
    
    public GameController gameController;
    private TargetPlacer targetPlacer;

    private void Start()
    {
        targetPlacer = FindObjectOfType<TargetPlacer>();
    }

    public void PlaceTargets(Difficulty difficulty)
    {
        gameController.targetPlacer.RemoveTargets();
        targetPlacer.PlaceTargets(difficulty);
    }
}
