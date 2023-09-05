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
    
    private void Start()
    {
        //Create reference to scorecontroller
        
    }

    void StartGame(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                //Set up the game
                break;
            case Difficulty.Normal:
                break;
            case Difficulty.Hard:
                break;
        }
    }

    public void RestartGame()
    {
        playing ^= playing;
    }
    
    
}
