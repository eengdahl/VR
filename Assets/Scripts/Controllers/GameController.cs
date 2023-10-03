using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEditor;
using UnityEngine;


public enum GameState { inMenu, Countdown, inGame }

public class GameController : MonoBehaviour
{
    [Header("State")]
    public GameState currentGameState;

    [Header("Components")]
    public UIController uiController;
    public ScoreController scoreController;
    public TargetPlacer targetPlacer;
    public Shoot shoot;
    AchievementHandler achievementHandler;
    private AudioSource audSource;
    [SerializeField] GameObject countdownSigns;

    [Header("Settings")]
    public Difficulty chosenDifficulty;
    public bool timeTrialEnabled;
    public float gameTime = 90f;
    private float gameTimer;

    private bool spawnedBoss;

    private void Awake()
    {
       

        audSource = GetComponent<AudioSource>();
        uiController = FindObjectOfType<UIController>();
        uiController.gameController = this;
        scoreController = FindObjectOfType<ScoreController>();
        scoreController.gameController = this;
        targetPlacer = FindObjectOfType<TargetPlacer>();
        shoot = FindObjectOfType<Shoot>(); //Dont know if this still is used
        shoot.gameController = this;
        achievementHandler = FindObjectOfType<AchievementHandler>();

        uiController.gameController = this;
        scoreController.gameController = this;
    }

    private void Update()
    {
        if (currentGameState == GameState.inGame && timeTrialEnabled)
        {
            gameTimer -= 1 * Time.deltaTime;
            scoreController.UpdateTimer(gameTimer);

            if (gameTimer <= 15 && !spawnedBoss)
            {
                targetPlacer.InitializeTargets(targetPlacer.SpawnBoss(chosenDifficulty));
                spawnedBoss = true;
            }

            if (gameTimer <= 0)
            {
                EndGame();
            }
        }
    }

    public void SetupGame(Difficulty difficulty) //spawn targets and ready the countdown
    {
        chosenDifficulty = difficulty; //set difficulty
        gameTimer = gameTime;
        scoreController.ResetScore(); //reset the score
        //scoreController.ClearAchievementsDisplay();
        targetPlacer.PlaceTargets(chosenDifficulty);
        countdownSigns.GetComponent<Animator>().SetTrigger("readyCountdown");
    }

    public void StartCountdown() //activate the countdown animation
    {
        currentGameState = GameState.Countdown;
        countdownSigns.GetComponent<Animator>().SetTrigger("startCountdown");
        scoreController.leaderboardAnim.SetBool("showLeaderboard", false);
    }

    public void ResetCountdown()
    {
        countdownSigns.GetComponent<Animator>().SetTrigger("resetCountdown");
    }

    public void StartGame() //start game
    {
        currentGameState = GameState.inGame;
        shoot.currentGameState = currentGameState;
        targetPlacer.InitializeTargets(targetPlacer.activeTargets);
        FindObjectOfType<CylinderPopulate>().FillBarrel(6);
    }

    public void EndGame() //only called when time is up, otherwise call ReturnToMenu
    {
        audSource.Play();
        targetPlacer.RemoveTargets();
        achievementHandler.SendAchievements();
        spawnedBoss = false;

        //Code to end the round and save score
        if (!scoreController.CheckLeaderboard(chosenDifficulty)) //if we're not on the leaderboard, automatically return to menu, otherwise we return throught SubmitScore
            uiController.ReturnToMenu();

        currentGameState = GameState.inMenu;
        //this needs to happen last
        shoot.currentGameState = currentGameState;

        scoreController.leaderboardAnim.SetBool("showLeaderboard", true); //show leaderboards again
    }

    public void ReturnToMenu() //only called when pressing "Choose New Difficulty", otherwise call EndGame
    {
        spawnedBoss = false;
        currentGameState = GameState.inMenu;
        shoot.currentGameState = currentGameState;
        targetPlacer.RemoveTargets();
        scoreController.leaderboardAnim.SetBool("showLeaderboard", true); //show leaderboards again
    }

    public void BulletFired(bool wasOnTarget)
    {
        scoreController.BulletWasFired(wasOnTarget);
    }

    public void OverrideTime(int time)
    {
        gameTimer += time;
    }
}