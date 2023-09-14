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
    [SerializeField] public Toggle timeTrialButton;
    public GameController gameController;

    [SerializeField] private CountdownSign signThree;
    [SerializeField] private CountdownSign signTwo;
    [SerializeField] private CountdownSign signOne;

    public bool timeTrialEnabled;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            MoveUpCountDownSigns();
        if (Input.GetKeyDown(KeyCode.K))
            StartCountDown();
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
        MoveUpCountDownSigns();
    }

    public void MoveUpCountDownSigns()
    {
        signOne.MoveUpSigns();
        signTwo.MoveUpSigns();
        signThree.MoveUpSigns();
    }

    public void ToggleTimeTrial(bool timeTrial)
    {
        timeTrialEnabled = timeTrial;
        //print("Enabled Time Trial");
    }

    public void SelectNewDifficulty()
    {
        StopAllCoroutines();
        gameController.TogglePlayState();
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
        StartCoroutine(nameof(CountDown));
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        signThree.CountdownSignFlip();
        yield return new WaitForSeconds(1);
        signTwo.CountdownSignFlip();
        yield return new WaitForSeconds(1);
        signOne.CountdownSignFlip();

        yield return new WaitForSeconds(0.5f);
        //Add something to show that the game has started
        gameController.TogglePlayState();
    }

    public void EndGame()
    {
        restartPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }
}
