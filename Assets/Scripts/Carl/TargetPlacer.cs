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

    [HideInInspector] public List<GameObject> activeTargets = new();

    private TargetTrailsRenderer targetTrailRenderer;

    [Header("Variables")]
    [Tooltip("The random range of how long targets should stay down in each difficulty. Easy is 0 and 1 etc.")]
    [SerializeField] List<float> downTimes = new List<float>();

    private void Start()
    {
        targetTrailRenderer = FindObjectOfType<TargetTrailsRenderer>();
    }

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
                activeTargets.AddRange(easyTargets);
                break;
            case Difficulty.Normal:
                activeTargets.AddRange(mediumTargets);
                break;
            case Difficulty.Hard:
                activeTargets.AddRange(hardTargets);
                break;
        }

        foreach (var target in activeTargets)
        {
            target.GetComponent<ShootableTarget>().firstTimeDeactivate = true;
            target.gameObject.SetActive(true);

            if (difficulty == Difficulty.Easy)
            {
                target.GetComponent<ShootableTarget>().minDownTime = downTimes[0];
                target.GetComponent<ShootableTarget>().maxDownTime = downTimes[1];
            }
            else if (difficulty == Difficulty.Normal)
            {
                target.GetComponent<ShootableTarget>().minDownTime = downTimes[2];
                target.GetComponent<ShootableTarget>().maxDownTime = downTimes[3];
            }
            if (difficulty == Difficulty.Hard)
            {
                target.GetComponent<ShootableTarget>().minDownTime = downTimes[4];
                target.GetComponent<ShootableTarget>().maxDownTime = downTimes[5];
            }
        }
        targetTrailRenderer.PopulateList(activeTargets);
    }

    public void RemoveTargets()
    {
        DeactivateTargets();

        activeTargets.Clear();
        targetTrailRenderer.DePopulateList();
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

    public void DeactivateTargets()
    {
        foreach (var target in activeTargets)
        {
            target.GetComponent<ShootableMoving>().shouldMove = false;
            target.gameObject.SetActive(false);
        }
    }
}
