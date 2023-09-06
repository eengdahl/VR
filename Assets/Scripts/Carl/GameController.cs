using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject easyTargetConfig;
    private GameObject normalTargetConfig;
    private GameObject hardTargetConfig;

    private DifficultyController difficultyController;
    public UIController uiController;
    public ScoreController scoreController;

    public bool playing;
    
    public Difficulty chosenDifficulty;
    public bool timeTrialEnabled;
    private void Start()
    {
        difficultyController = FindObjectOfType<DifficultyController>();
        uiController = FindObjectOfType<UIController>();
        scoreController = FindObjectOfType<ScoreController>();
        
        difficultyController.gameController = this;
        uiController.gameController = this;
        scoreController.gameController = this;
    }

    private void Update()
    {
        if (playing)
        {
            
        }
    }

    public void InitializeGame(Difficulty difficulty, bool timeTrial)
    {
        if (!playing)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    chosenDifficulty = difficulty;
                    timeTrialEnabled = timeTrial;
                    difficultyController.PlaceTargets(chosenDifficulty);
                    break;
                case Difficulty.Normal:
                    chosenDifficulty = difficulty;
                    timeTrialEnabled = timeTrial;
                    difficultyController.PlaceTargets(chosenDifficulty);
                    break;
                case Difficulty.Hard:
                    chosenDifficulty = difficulty;
                    timeTrialEnabled = timeTrial;
                    difficultyController.PlaceTargets(chosenDifficulty);
                    break;
            }
        }
    }
    
    public void RestartGame()
    {
        playing = false;
        difficultyController.RemoveTargets();
        difficultyController.PlaceTargets(chosenDifficulty);
        uiController.StartCountDown();
    }
    
}
