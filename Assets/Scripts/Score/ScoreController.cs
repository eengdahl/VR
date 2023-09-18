using Carl;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class ScoreEntry
{
    public string name;
    public int score;
}

[Serializable]
class HighscoreSaveData
{
    public List<ScoreEntry> easyEntries = new(3);
    public List<ScoreEntry> mediumEntries = new(3);
    public List<ScoreEntry> hardEntries = new(3);
}

public class ScoreController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] Slider multiplierSlider;
    [SerializeField] private TextMeshProUGUI bulletsFiredText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject nameInputPanel;
    [SerializeField] TextMeshProUGUI[] nameButtons;

    [SerializeField] HighscoreSaveData hsSaveData = new HighscoreSaveData();

    [Header("Current Score")]
    public int score;
    private int latestScoreReceived;
    public int currentMultiplier;
    [SerializeField] int maxMultiplier = 9;

    [SerializeField] float multiplierTime;
    float multiplierTimer;

    private int bulletsFired;
    private int bulletsOnTarget;
    private float accuracy;

    [HideInInspector] public bool timeTrialEnabled;

    [HideInInspector] public GameController gameController;

    [Header("Highscore Lists")]
    [SerializeField] List<int> easyHighscores = new(3);
    [SerializeField] List<int> mediumHighscores = new(3);
    [SerializeField] List<int> hardHighscores = new(3);

    [SerializeField] List<string> easyNames = new(3);
    [SerializeField] List<string> mediumNames = new(3);
    [SerializeField] List<string> hardNames = new(3);

    [SerializeField] string currentName;
    [SerializeField] List<int> charInt = new(3);
    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [Header("Leaderboards")]
    [SerializeField] TextMeshProUGUI[] easyLeaderboard;
    [SerializeField] TextMeshProUGUI[] mediumLeaderboard;
    [SerializeField] TextMeshProUGUI[] hardLeaderboard;

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighscoreSaveData"))
        {
            //print("Found highscoredata: " + PlayerPrefs.GetString("HighscoreSaveData"));
            hsSaveData = JsonUtility.FromJson<HighscoreSaveData>(PlayerPrefs.GetString("HighscoreSaveData"));

            for (int i = 0; i < 3; i++)
            {
                easyHighscores[i] = hsSaveData.easyEntries[i].score;
                easyNames[i] = hsSaveData.easyEntries[i].name;

                mediumHighscores[i] = hsSaveData.mediumEntries[i].score;
                mediumNames[i] = hsSaveData.mediumEntries[i].name;

                hardHighscores[i] = hsSaveData.hardEntries[i].score;
                hardNames[i] = hsSaveData.hardEntries[i].name;
            }
        }

        UpdateLeaderboard();
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

    //---------INGAME SCORE---------------
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
        scoreText.text = "Score: " + score;
        multiplierText.text = "Combo: " + currentMultiplier;
        bulletsFiredText.text = "Shots fired: " + bulletsFired;
        accuracyText.text = "Accuracy: " + accuracy.ToString("P1");
    }

    //public void StartNewGame(bool timeTrial)
    //{
    //    ResetScore();
    //}

    //public void EndCurrentGame()
    //{
    //    //Add code to save score if time trial was enabled
    //}

    public void ResetScore()
    {
        score = 0;
        currentMultiplier = 0;
        multiplierTimer = 0;
    }

    //-----------HIGHSCORE---------------
    public bool CheckLeaderboard(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                if (score > easyHighscores[2])
                {
                    //TODO show panel for inputting name
                    ShowInputName();
                    return true;
                }
                break;
            case Difficulty.Normal:
                if (score > mediumHighscores[2])
                {
                    //TODO show panel for inputting name
                    ShowInputName();
                    return true;
                }
                break;
            case Difficulty.Hard:
                if (score > hardHighscores[2])
                {
                    //TODO show panel for inputting name
                    ShowInputName();
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    void ShowInputName()
    {
        nameInputPanel.SetActive(true);
        currentName = "AAA";

        for (int i = 0; i < charInt.Count; i++)
        {
            charInt[i] = 0;
            nameButtons[i].text = "A";
        }
    }

    public void SubmitHighscore()
    {
        SaveHighscoreToJson(gameController.chosenDifficulty, currentName);

        UpdateLeaderboard();

        nameInputPanel.SetActive(false);
    }

    public void SaveHighscoreToJson(Difficulty difficulty, string name)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                for (int i = 0; i < easyHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > easyHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        easyHighscores.Insert(i, score);
                        easyNames.Insert(i, name); //save the name
                        break;
                    }
                }

                //the list is too long, remove the now not-highscore
                easyHighscores.RemoveAt(3);
                easyNames.RemoveAt(3);

                for (int i = 0; i < 3; i++) //save it to our highscore savedata
                {
                    hsSaveData.easyEntries[i].score = easyHighscores[i];
                    hsSaveData.easyEntries[i].name = easyNames[i];
                }

                break;
            case Difficulty.Normal:
                for (int i = 0; i < mediumHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > mediumHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        mediumHighscores.Insert(i, score);
                        mediumNames.Insert(i, name);
                        break;
                    }
                }
                mediumHighscores.RemoveAt(3);
                mediumNames.RemoveAt(3);

                for (int i = 0; i < 3; i++) //save it to our highscore savedata
                {
                    hsSaveData.mediumEntries[i].score = mediumHighscores[i];
                    hsSaveData.mediumEntries[i].name = mediumNames[i];
                }

                break;
            case Difficulty.Hard:
                for (int i = 0; i < hardHighscores.Count; i++) //compare current score to current highscores
                {
                    if (score > hardHighscores[i]) //if it's higher, we insert it at that spot
                    {
                        hardHighscores.Insert(i, score);
                        hardNames.Insert(i, name);
                        break;
                    }
                }

                hardHighscores.RemoveAt(3);
                hardNames.RemoveAt(3);

                for (int i = 0; i < 3; i++) //save it to our highscore savedata
                {
                    hsSaveData.hardEntries[i].score = hardHighscores[i];
                    hsSaveData.hardEntries[i].name = hardNames[i];
                }

                break;
            default:
                break;
        }

        //if (easyHighscores.Count >= 4) //if we inserted, the list is too long and we trim the excess

        string jsonString = JsonUtility.ToJson(hsSaveData);
        PlayerPrefs.SetString("HighscoreSaveData", jsonString);

        //print(jsonString);
    }

    public void SaveHighscore(int difficulty)
    {
        //save all highscoredata to json and playerprefs
        string jsonString = JsonUtility.ToJson(hsSaveData);
        //PlayerPrefs.SetString("HighscoreSaveData", jsonString);
        print(jsonString);
        //PlayerPrefs.SetString("EasyHighscores", jsonString);
    }

    public void WriteName(int i)
    {
        if (charInt[i] < chars.Length - 1)
            charInt[i]++;
        else
            charInt[i] = 0;

        currentName = currentName.Remove(i, 1);
        currentName = currentName.Insert(i, GetLetter(charInt[i]).ToString());

        nameButtons[i].text = GetLetter(charInt[i]).ToString();
    }

    public string SubmitName()
    {
        string returnedName = "";

        for (int i = 0; i < 3; i++)
            returnedName += currentName[i];

        return returnedName;
    }

    public string RandomName() //NOT USED ANYMORE
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string returnedName = "";

        for (int i = 0; i < 3; i++)
        {
            int ind = UnityEngine.Random.Range(0, chars.Length);

            returnedName += chars[ind];
        }

        return returnedName;
    }

    public char GetLetter(int i)
    {
        return chars[i];
    }

    public void UpdateLeaderboard()
    {
        for (int i = 0; i < 3; i++)
        {
            easyLeaderboard[i].text = easyNames[i] + " " + easyHighscores[i].ToString("000000");
            mediumLeaderboard[i].text = mediumNames[i] + " " + mediumHighscores[i].ToString("000000");
            hardLeaderboard[i].text = hardNames[i] + " " + hardHighscores[i].ToString("000000");
        }
    }

    public void BulletWasFired(bool wasOnTarget)
    {
        bulletsFired++;
        if (wasOnTarget)
            bulletsOnTarget++;
        accuracy = bulletsFired / bulletsOnTarget;
    }

    //------------TIMER------------

    public void UpdateTimer(float time)
    {
        timerText.text = "TIME LEFT: " + Mathf.Round(time).ToString("00");
        if (time <= 0)
        {

            timerText.text = "GAME OVER! GOOD JOB!";
        }
    }
}
