using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //private ScoreController scoreController;
    private GameObject easyTargetConfig;
    private GameObject normalTargetConfig;
    private GameObject hardTargetConfig;

    public bool playing;

    private DifficultyController difficultyController;
    private UIController uiController;
    
    private void Start()
    {
        difficultyController = FindObjectOfType<DifficultyController>();
        uiController = FindObjectOfType<UIController>();
        //Create reference to scorecontroller

    }

    public void StartGame(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                difficultyController.PlaceTargets(Difficulty.Easy);
                break;
            case Difficulty.Normal:
                difficultyController.PlaceTargets(Difficulty.Normal);
                break;
            case Difficulty.Hard:
                difficultyController.PlaceTargets(Difficulty.Hard);
                break;
        }
        
        uiController.StartGame();
    }

    public void RestartGame()
    {
        playing ^= playing;
        difficultyController.RemoveTargets();
    }
    
    
}
