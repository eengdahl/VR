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
    [Tooltip("0 = Easy boss, 1 = Medium boss, 2 = hard boss")]
    [SerializeField] List<GameObject> bossTargets = new(); //find a way to spawn targets

    [HideInInspector] public List<GameObject> activeTargets = new();

    private TargetTrailsRenderer targetTrailRenderer;

    [Header("Variables")]
    [Tooltip("The random range of how long targets should stay down in each difficulty. Easy is 0 and 1 etc.")]
    [SerializeField] List<float> downTimes = new List<float>();

    private void Awake()
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
            target.gameObject.SetActive(true);

            if (target.GetComponent<ShootableTarget>() != null)
            {

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
                else if (difficulty == Difficulty.Hard)
                {
                    target.GetComponent<ShootableTarget>().minDownTime = downTimes[4];
                    target.GetComponent<ShootableTarget>().maxDownTime = downTimes[5];
                }
            }
        }
        targetTrailRenderer.PopulateList(activeTargets);

        //this needs to happen somewhere else, for example when timer is 10
        //InitializeTargets(SpawnBoss(difficulty)); //spawn our boss monster
    }

    public GameObject SpawnBoss(Difficulty difficulty) //spawn appropriate boss target
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return bossTargets[0];
            case Difficulty.Normal:
                return bossTargets[1];
            case Difficulty.Hard:
                return bossTargets[2];
            default:
                return bossTargets[2];
        }
    }

    public void RemoveTargets()
    {
        DeactivateTargets();

        activeTargets.Clear();
        targetTrailRenderer.DePopulateList();
    }

    public void InitializeTargets(List<GameObject> targetsToInitalize)
    {
        foreach (var target in targetsToInitalize)
        {
            if (target.GetComponent<ShootableMoving>() != null)
            {
                target.GetComponent<ShootableMoving>().ManualSetUpTarget();
                target.GetComponent<ShootableMoving>().shouldMove = true;
                target.GetComponent<ShootableMoving>().ManualChangeState(ShootableMoving.CurrentState.Moving);
            }
        }
    }

    public void InitializeTargets(GameObject bossToSpawn) //spawn our boss monster
    {
        bossToSpawn.SetActive(true);
        bossToSpawn.GetComponent<ShootableMoving>().ManualSetUpTarget();
        bossToSpawn.GetComponent<ShootableMoving>().shouldMove = true;
        bossToSpawn.GetComponent<ShootableMoving>().ManualChangeState(ShootableMoving.CurrentState.Moving);
    }

    public void DeactivateTargets()
    {
        foreach (var target in activeTargets)
        {
            target.GetComponent<ShootableMoving>().shouldMove = false;

            if (target.GetComponent<ShootableMoving>().moveType == ShootableMoving.MoveType.Waypoints)
                target.transform.position = target.GetComponent<ShootableMoving>().waypoints[0].position;

            target.gameObject.SetActive(false);
        }

        foreach (var item in bossTargets)
        {
            item.SetActive(false);
        }
        targetTrailRenderer.DePopulateList();
    }
}
