using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Carl;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] Slider multiplierSlider;

    public int score;
    private int latestScoreReceived;
    public int currentMultiplier;
    [SerializeField] int maxMultiplier = 9;

    [SerializeField] float multiplierTime;
    float multiplierTimer;

    public bool timeTrialEnabled;

    public GameController gameController;

    [SerializeField] List<int> easyHighscores = new(3);
    [SerializeField] List<int> mediumHighscores = new(3);
    [SerializeField] List<int> hardHighscores = new(3);

    private void Update()
    {
        if (multiplierTimer > 0)
        {
            multiplierTimer -= Time.deltaTime;
            multiplierSlider.value = multiplierTimer / multiplierTime;
        }
        else if (multiplierTimer <= 0)
        {
            currentMultiplier = 0;
            UpdateScoreText();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        latestScoreReceived = scoreToAdd;
        latestScoreReceived *= (currentMultiplier + 1);
        score += latestScoreReceived;

        if (currentMultiplier < maxMultiplier) currentMultiplier++;
        multiplierTimer = multiplierTime;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        //TODO update the text and all that
        scoreText.text = "Score: " + score.ToString();
        multiplierText.text = "Combo: " + currentMultiplier.ToString();
    }

    public void StartNewGame(bool timeTrial)
    {
        ResetScore();
    }

    public void EndCurrentGame()
    {
        //Add code to save score if time trial was enabled
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SaveHighscore(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                for (int i = 0; i < easyHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > easyHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        easyHighscores.Insert(i, score);
                        break;
                    }
                }
                break;
            case Difficulty.Normal:
                for (int i = 0; i < mediumHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > mediumHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        mediumHighscores.Insert(i, score);
                        break;
                    }
                }
                break;
            case Difficulty.Hard:
                for (int i = 0; i < hardHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > hardHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        hardHighscores.Insert(i, score);
                        break;
                    }
                }
                break;
            default:
                break;
        }

        if (easyHighscores.Count >= 4) //if we inserted, the list is too long and we trim the excess
            easyHighscores.RemoveAt(3);
    }
}
