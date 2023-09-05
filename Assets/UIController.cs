using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEditor;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;

    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject restartPanel;

    public void SelectDifficulty(int selection)
    {
        switch (selection)
        {
            case 0:
                gameController.StartGame(Difficulty.Easy);
                break;
            case 1:
                gameController.StartGame(Difficulty.Normal);
                break;
            case 2:
                gameController.StartGame(Difficulty.Hard);
                break;
        }
        
        difficultyPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        //Add function to move Canvas
        startPanel.SetActive(false);
        restartPanel.SetActive(true);
        StartCountDown();
    }

    public void ChangeDifficulty()
    {
        restartPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void RestartGame()
    {
        gameController.RestartGame();
    }

    public void StartCountDown()
    {
        
    }
}
