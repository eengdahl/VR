using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] public GameObject difficultyPanel;
    [SerializeField] public GameObject startPanel;
    [SerializeField] public GameObject restartPanel;
    [SerializeField] public GameObject timeTrialPanel;
    [SerializeField] public GameObject backPanel;
    public GameController gameController;

    [SerializeField] private CountdownSign signThree;
    [SerializeField] private CountdownSign signTwo;
    [SerializeField] private CountdownSign signOne;

    public void SelectDifficulty(int selection) //Choose difficulty and SetupGame in Gamecontroller
    {
        switch (selection)
        {
            case 0:
                gameController.SetupGame(Difficulty.Easy);
                break;
            case 1:
                gameController.SetupGame(Difficulty.Normal);
                break;
            case 2:
                gameController.SetupGame(Difficulty.Hard);
                break;
        }

        difficultyPanel.SetActive(false);
        timeTrialPanel.SetActive(false);
        startPanel.SetActive(true);
        gameController.scoreController.ResetScore();
    }

    public void ToggleTimeTrial(bool timeTrial) //change timetrial bool in GameController
    {
        gameController.timeTrialEnabled = timeTrial;
    }

    public void BackToDifficultySelection() //Calls ReturnToMenu in GameController
    {
        if (gameController.currentGameState != GameController.GameState.inGame) return;

        backPanel.SetActive(false);
        startPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        timeTrialPanel.SetActive(true);
        gameController.ReturnToMenu();
    }

    public void StartCountDown() //Removes menu in preparation for game
    {
        //Add function to move Canvas
        startPanel.SetActive(false);
        restartPanel.SetActive(true);
        gameController.StartCountdown();
    }

    public void ShowMenu() //A ReturnToMenu Function
    {
        restartPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        timeTrialPanel.SetActive(true);
        gameController.ReturnToMenu();
    }
}
