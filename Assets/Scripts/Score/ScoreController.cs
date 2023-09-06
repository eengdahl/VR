using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
}
