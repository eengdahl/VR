using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class TargetPlacer : MonoBehaviour
{
    [Header("Target prefabs")]
    [SerializeField] private GameObject stationaryTarget;
    [SerializeField] private GameObject movingTarget;
    [Header("Stationary target points")]
    [SerializeField] private List<GameObject> easyStationaryTargets = new();
    [SerializeField] private List<GameObject> normalStationaryTargets = new();
    [SerializeField] private List<GameObject> hardStationaryTargets = new();
    [Header("Moving target objects")]
    [SerializeField] private List<GameObject> easyMovingTargets;
    [SerializeField] private List<GameObject> normalMovingTargets;
    [SerializeField] private List<GameObject> hardTargetMovingParents;
    [Header("Scriptable Objects for the difficulty settings")]
    [SerializeField] private DifficultySettings easySettings;
    [SerializeField] private DifficultySettings normalSettings;
    [SerializeField] private DifficultySettings hardSettings;

    private List<GameObject> shuffledPointsList = new();
    public List<GameObject> shuffledParentsList = new();
    private int currentIndex = 0;
    public DifficultyController difficultyController;
    
    public List<GameObject> activeTargets = new();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaceTargets(Difficulty.Normal);
        }
    }

    public void PlaceTargets(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                ShuffleStationaryTargetPoints(easyStationaryTargets);
                ShuffleMovingParents(easyMovingTargets);
                int easyTotalTargetCount = easySettings.totalTargets;
                int easyMovingTargetCount = easySettings.movingTargets;
                int easyStationaryTargetCount = easyTotalTargetCount - easyMovingTargetCount;
                
                for (int j = 0; j <= easyStationaryTargetCount; j++)
                {
                    GameObject newTarget = GetRandomStationaryTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                    j++;
                }

                currentIndex = 0;
                
                for (int j = 0; j <= easyMovingTargetCount; j++)
                {
                    GameObject newTarget = GetRandomMovingTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                }
                
                currentIndex = 0;
                break;
            
            case Difficulty.Normal:
                ShuffleStationaryTargetPoints(normalStationaryTargets);
                ShuffleMovingParents(normalMovingTargets);
                int normalTotalTargetCount = normalSettings.totalTargets;
                int normalMovingTargetCount = normalSettings.movingTargets;
                int normalStationaryTargetCount = normalTotalTargetCount - normalMovingTargetCount;
                
                for (int j = 0; j < normalStationaryTargetCount; j++)
                {
                    GameObject newTarget = GetRandomStationaryTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                    j++;
                }

                currentIndex = 0;
                
                for (int j = 0; j < normalMovingTargetCount; j++)
                {
                    GameObject newTarget = GetRandomMovingTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                    j++;
                }
                
                currentIndex = 0;
                break;
            
            case Difficulty.Hard:
                ShuffleStationaryTargetPoints(hardStationaryTargets);
                ShuffleMovingParents(hardTargetMovingParents);
                int hardTotalTargetCount = normalSettings.totalTargets;
                int hardMovingTargetCount = normalSettings.movingTargets;
                int hardStationaryTargetCount = hardTotalTargetCount - hardMovingTargetCount;
                
                for (int j = 0; j < hardStationaryTargetCount; j++)
                {
                    GameObject newTarget = GetRandomStationaryTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                    j++;
                }

                currentIndex = 0;
                
                for (int j = 0; j < hardMovingTargetCount; j++)
                {
                    GameObject newTarget = GetRandomMovingTarget();
                    newTarget.gameObject.SetActive(true);
                    activeTargets.Add(newTarget);
                    j++;
                }
                
                currentIndex = 0;
                break;
        }
    }

    private void ShuffleStationaryTargetPoints(List<GameObject> targetPointList)
    {
        shuffledPointsList.Clear();
        shuffledPointsList.AddRange(targetPointList);

        int n = shuffledPointsList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject temp = shuffledPointsList[k];
            shuffledPointsList[k] = shuffledPointsList[n];
            shuffledPointsList[n] = temp;
        }
    }

    private void ShuffleMovingParents(List<GameObject> targetParentsList)
    {
        shuffledParentsList.Clear();
        shuffledParentsList.AddRange(targetParentsList);

        int n = shuffledParentsList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject temp = shuffledParentsList[k];
            shuffledParentsList[k] = shuffledParentsList[n];
            shuffledParentsList[n] = temp;
        }
    }

    private GameObject GetRandomStationaryTarget()
    {
        GameObject randomTarget = shuffledPointsList[currentIndex];
        currentIndex++;
        return randomTarget;
    }

    private GameObject GetRandomMovingTarget()
    {
        GameObject randomTarget = shuffledParentsList[currentIndex];
        currentIndex++;
        return randomTarget;
    }

    public void RemoveTargets()
    {
        foreach (GameObject target in activeTargets)
        {
            target.gameObject.SetActive(false);
        }
    }
}
