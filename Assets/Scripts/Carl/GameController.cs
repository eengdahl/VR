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
    
    public UIController uiController;
    public ScoreController scoreController;
    public TargetPlacer targetPlacer;
    
    public bool playing;
    
    public Difficulty chosenDifficulty;
    public bool timeTrialEnabled;

    public float gameTime = 90f;
    
    private void Start()
    {
        uiController = FindObjectOfType<UIController>();
        scoreController = FindObjectOfType<ScoreController>();
        targetPlacer = FindObjectOfType<TargetPlacer>();
        
        uiController.gameController = this;
        scoreController.gameController = this;
    }

    private void Update()
    {
        if (playing && timeTrialEnabled)
        {
            gameTime -= 1 * Time.deltaTime;

            if (gameTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void InitializeGame(Difficulty difficulty, bool timeTrial)
    {
        chosenDifficulty = difficulty;
        timeTrialEnabled = timeTrial;
        targetPlacer.PlaceTargets(chosenDifficulty);
    }
    
    public void RestartGame()
    {
        playing = false;
        targetPlacer.RemoveTargets();
        targetPlacer.PlaceTargets(chosenDifficulty);
        uiController.MoveUpCountDownSigns();
        uiController.StartCountDown();
    }
    
    public void EndGame()
    {
        playing = false;
        scoreController.SaveHighscore(chosenDifficulty);
        //Code to end the round and save score
        targetPlacer.RemoveTargets();
        uiController.EndGame();
    }
}