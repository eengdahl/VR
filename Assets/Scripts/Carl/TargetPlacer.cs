using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class TargetPlacer : MonoBehaviour
{
    [Header("New TargetLists")]
    [SerializeField] List<GameObject> easyTargets = new();
    [SerializeField] List<GameObject> mediumTargets = new();
    [SerializeField] List<GameObject> hardTargets = new();

    public List<GameObject> activeTargets = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaceTargets(Difficulty.Normal);
        }
    }

    public void PlaceTargets(Difficulty difficulty) //activate the appropriate targets
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                activeTargets = easyTargets;
                break;
            case Difficulty.Normal:
                activeTargets = mediumTargets;
                break;
            case Difficulty.Hard:
                activeTargets = hardTargets;
                break;
        }

        foreach (var target in activeTargets)
        {
            target.GetComponent<ShootableTarget>().firstTimeDeactivate = true;
            target.gameObject.SetActive(true);
        }
    }

    public void RemoveTargets()
    {
        foreach (var target in activeTargets)
        {
            target.gameObject.SetActive(false);
        }

        activeTargets.Clear();
    }

    public void InitializeTargets()
    {
        foreach (var target in activeTargets)
        {
            target.GetComponent<ShootableMoving>().ManualSetUpTarget();
            target.GetComponent<ShootableMoving>().shouldMove = true;
            target.GetComponent<ShootableMoving>().ManualChangeState(ShootableMoving.CurrentState.Moving);
        }
    }
}
