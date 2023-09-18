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
    [Header("UI Components")]
    [SerializeField] public GameObject difficultyPanel;
    [SerializeField] public GameObject startPanel;
    [SerializeField] public GameObject restartPanel;
    [SerializeField] public GameObject timeTrialPanel;
    [SerializeField] public GameObject backPanel;
    public GameController gameController;

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
        backPanel.SetActive(true);
    }

    public void ToggleTimeTrial(bool timeTrial) //change timetrial bool in GameController
    {
        gameController.timeTrialEnabled = timeTrial;
    }

    public void ReturnToMenu() //Calls ReturnToMenu in GameController
    {
        if (gameController.currentGameState != GameController.GameState.inGame) return;

        restartPanel.SetActive(false);
        backPanel.SetActive(false);
        startPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        timeTrialPanel.SetActive(true);
        gameController.ReturnToMenu();
    }

    public void StartCountDown() //Removes menu in preparation for game
    {
        //Add function to move Canvas (?)
        startPanel.SetActive(false);
        restartPanel.SetActive(true);
        gameController.StartCountdown();
    }

    //public void ShowMenu() //A ReturnToMenu Function
    //{
    //    difficultyPanel.SetActive(true);
    //    timeTrialPanel.SetActive(true);
    //    gameController.ReturnToMenu();
    //}

    public void BackButton() //used by the backbutton in main menu
    {
        difficultyPanel.SetActive(true);
        timeTrialPanel.SetActive(true);
        startPanel.SetActive(false);
        backPanel.SetActive(false);
        gameController.ResetCountdown();
    }
}
