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
    [SerializeField] private List<Transform> easyTargetStationaryPoints = new();
    [SerializeField] private List<Transform> normalTargetStationaryPoints = new();
    [SerializeField] private List<Transform> hardTargetStationaryPoints = new();
    [Header("Moving target objects")]
    [SerializeField] private List<GameObject> easyTargetMovingParents;
    [SerializeField] private List<GameObject> normalTargetMovingParents;
    [SerializeField] private List<Transform> hardTargetMovingParents;
    [Header("Scriptable Objects for the difficulty settings")]
    [SerializeField] private DifficultySettings easySettings;
    [SerializeField] private DifficultySettings normalSettings;
    [SerializeField] private DifficultySettings hardSettings;

    private List<Transform> shuffledPointsList = new();
    public List<GameObject> shuffledParentsList = new();
    private int currentIndex = 0;
    public DifficultyController difficultyController;
    
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
                ShuffleStationaryTargetPoints(easyTargetStationaryPoints);
                ShuffleMovingParents(easyTargetMovingParents);
                int easyTotalTargetCount = easySettings.totalTargets;
                int easyMovingTargetCount = easySettings.movingTargets;
                int easyStationaryTargetCount = easyTotalTargetCount - easyMovingTargetCount;
                
                for (int j = 0; j <= easyStationaryTargetCount; j++)
                {
                    Transform newPoint = GetRandomPoint();
                    GameObject newTarget = Instantiate(stationaryTarget, newPoint.position, newPoint.rotation);
                    difficultyController.activeTargets.Add(newTarget);
                    j++;
                }

                currentIndex = 0;
                
                for (int j = 0; j <= easyMovingTargetCount; j++)
                {
                    GameObject newParent = GetRandomParent();
                    GameObject newTarget = Instantiate(movingTarget, newParent.transform.position, newParent.transform.rotation);
                    newTarget.GetComponent<ShootableMoving>().InitiatePatrol(newParent);
                    difficultyController.activeTargets.Add(newTarget);
                }
                
                currentIndex = 0;
                // foreach (var transform in easyTargetStationaryPoints)
                // {
                //     GameObject newTarget = Instantiate(stationaryTarget, transform.position, transform.rotation);
                //     newTarget.GetComponent<ShootableMoving>().moveType = ShootableMoving.MoveType.FlipUp;
                //     difficultyController.activeTargets.Add(newTarget);
                // }
                break;
            
            case Difficulty.Normal:
                ShuffleStationaryTargetPoints(normalTargetStationaryPoints);
                ShuffleMovingParents(normalTargetMovingParents);
                int normalTotalTargetCount = normalSettings.totalTargets;
                int normalMovingTargetCount = normalSettings.movingTargets;
                int normalStationaryTargetCount = normalTotalTargetCount - normalMovingTargetCount;
                
                for (int j = 1; j <= normalStationaryTargetCount; j++)
                {
                    Transform newPoint = GetRandomPoint();
                    GameObject newTarget = Instantiate(stationaryTarget, newPoint.position, newPoint.rotation);
                    difficultyController.activeTargets.Add(newTarget);
                    j++;
                }

                currentIndex = 0;
                
                for (int j = 0; j < normalMovingTargetCount; j++)
                {
                    GameObject newParent = GetRandomParent();
                    GameObject newTarget = Instantiate(movingTarget, newParent.transform.position, newParent.transform.rotation);
                    newTarget.GetComponent<ShootableMoving>().InitiatePatrol(newParent);
                    difficultyController.activeTargets.Add(newTarget);
                    j++;
                }
                
                currentIndex = 0;
                
                
                // foreach (var transform in normalTargetStationaryPoints)
                // {
                //     GameObject newTarget = Instantiate(stationaryTarget, transform.position, transform.rotation);
                //     difficultyController.activeTargets.Add(newTarget);
                // }
                break;
            case Difficulty.Hard:
                foreach (var transform in normalTargetStationaryPoints)
                {
                    GameObject newTarget = Instantiate(stationaryTarget, transform.position, transform.rotation);
                    difficultyController.activeTargets.Add(newTarget);
                }
                break;
        }
    }

    private void ShuffleStationaryTargetPoints(List<Transform> targetPointList)
    {
        shuffledPointsList.Clear();
        shuffledPointsList.AddRange(targetPointList);

        int n = shuffledPointsList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Transform temp = shuffledPointsList[k];
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

    private Transform GetRandomPoint()
    {
        Transform randomPoint = shuffledPointsList[currentIndex];
        currentIndex++;
        return randomPoint;
    }

    private GameObject GetRandomParent()
    {
        GameObject randomParent = shuffledParentsList[currentIndex];
        currentIndex++;
        return randomParent;
    }
}
