using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [SerializeField] private ScriptableObject easyDifficulty;
    [SerializeField] private ScriptableObject mediumDifficulty;
    [SerializeField] private ScriptableObject hardDifficulty;

    public List<GameObject> activeTargets = new List<GameObject>();
    
    public GameController gameController;
    private TargetPlacer targetPlacer;

    private void Start()
    {
        targetPlacer = FindObjectOfType<TargetPlacer>();
        targetPlacer.difficultyController = this;
    }

    public void PlaceTargets(Difficulty difficulty)
    {
        RemoveTargets();
        targetPlacer.PlaceTargets(difficulty);
    }

    public void RemoveTargets()
    {
        foreach (var target in activeTargets)
        {
            Destroy(target);
        }
    }
}
