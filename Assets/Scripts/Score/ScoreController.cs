using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Carl;

[Serializable]
class HighscoreSaveData
{
    public int[] easyHighscores = new int[3];
    public int[] mediumHighscores = new int[3];
    public int[] hardHighscores = new int[3];
}

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] Slider multiplierSlider;

    HighscoreSaveData hsSaveData = new HighscoreSaveData();

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

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighscoreSaveData"))
        {
            print("Found highscoredata: " + PlayerPrefs.GetString("HighscoreSaveData"));
            hsSaveData = JsonUtility.FromJson<HighscoreSaveData>(PlayerPrefs.GetString("HighscoreSaveData"));

            for (int i = 0; i < easyHighscores.Count; i++)
            {
                easyHighscores[i] = hsSaveData.easyHighscores[i];
                mediumHighscores[i] = hsSaveData.mediumHighscores[i];
                hardHighscores[i] = hsSaveData.hardHighscores[i];
            }
        }
    }

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

                for (int i = 0; i < easyHighscores.Count; i++)
                    hsSaveData.easyHighscores[i] = easyHighscores[i];

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

        string jsonString = JsonUtility.ToJson(hsSaveData);
        print(jsonString);
    }
    public void SaveHighscore(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                for (int i = 0; i < easyHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > easyHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        easyHighscores.Insert(i, score);
                        break;
                    }
                }

                for (int i = 0; i < easyHighscores.Count; i++)
                    hsSaveData.easyHighscores[i] = easyHighscores[i];

                break;
            case 1:
                for (int i = 0; i < mediumHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > mediumHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        mediumHighscores.Insert(i, score);
                        break;
                    }
                }

                for (int i = 0; i < mediumHighscores.Count; i++)
                    hsSaveData.mediumHighscores[i] = mediumHighscores[i];

                break;
            case 2:
                for (int i = 0; i < hardHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > hardHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        hardHighscores.Insert(i, score);
                        break;
                    }
                }

                for (int i = 0; i < hardHighscores.Count; i++)
                    hsSaveData.hardHighscores[i] = hardHighscores[i];

                break;
            default:
                break;
        }

        if (easyHighscores.Count >= 4) //if we inserted, the list is too long and we trim the excess
            easyHighscores.RemoveAt(3);

        //save all highscoredata to json and playerprefs
        string jsonString = JsonUtility.ToJson(hsSaveData);
        PlayerPrefs.SetString("HighscoreSaveData", jsonString);
        print(jsonString);
    }
}
