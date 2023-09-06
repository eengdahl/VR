using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private GameObject timeTrialPanel;
    [SerializeField] private Toggle timeTrialButton;
    
    public GameController gameController;

    private bool countDown;
    private bool playing;

    public bool timeTrialEnabled;
    
    private void Update()
    {
        if (playing)
        {
            
        }
    }

    public void SelectDifficulty(int selection)
    {
        
        switch (selection)
        {
            case 0:
                gameController.InitializeGame(Difficulty.Easy, timeTrialEnabled);
                break;
            case 1:
                gameController.InitializeGame(Difficulty.Normal, timeTrialEnabled);
                break;
            case 2:
                gameController.InitializeGame(Difficulty.Hard, timeTrialEnabled);
                break;
        }
        
        difficultyPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void ChangeDifficulty()
    {
        
    }
    
    private void StartGame()
    {
        gameController.playing = true;
        gameController.scoreController.StartNewGame(timeTrialEnabled);
    }

    public void ToggleTimeTrial()
    {
        timeTrialEnabled = timeTrialButton.isOn;
    }
    
    public void SelectNewDifficulty()
    {
        gameController.playing = false;
        restartPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void RestartGame()
    {
        gameController.RestartGame();
    }

    public void StartCountDown()
    {
        //Add function to move Canvas
        startPanel.SetActive(false);
        restartPanel.SetActive(true);
        Invoke(nameof(StartGame), 3f);
    }
}
