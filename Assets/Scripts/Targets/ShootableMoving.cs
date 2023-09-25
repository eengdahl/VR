using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMoving : MonoBehaviour
{
    public enum MoveType { Waypoints, FlipUp }
    [Tooltip("Should the target move from point to point or only flip up?")]
    [SerializeField] public MoveType moveType;

    [Header("Waypoints")]
    [Tooltip("An empty gameobject with the transforms of the waypoints as children")]
    [SerializeField] Transform waypointParent;
    [HideInInspector] public List<Transform> waypoints = new(); //added automatically

    [Tooltip("Should the target move in sequence or randomly?")]
    [SerializeField] bool randomWaypoint = false;

    [Tooltip("Should waypoints be visited in loop or back and forth?")]
    [SerializeField] bool pingPong = false;
    private bool ascending;

    [Header("Variables")]
    [Tooltip("How long the target should wait at each waypoint")]
    [SerializeField] float waitTime = 1f;
    float waitTimer;

    [Tooltip("How fast the target is walking between waypoints, Recommended between 1 and 5")]
    [SerializeField] float moveSpeed = 1f;
    float acceleration;

    [Header("Debugging")]
    [Tooltip("Only viewable for debugging purposes, don't touch!")]
    [SerializeField] int currentwaypoint;
    [Tooltip("Targets that exists in the scene on start need this")]
    [SerializeField] bool testTarget = false;

    //-----HitFeedback has been moved to ShootableTarget 

    float returnBuffer;
    [HideInInspector] public bool shouldMove;

    public enum CurrentState { Moving, Waiting, Idle }
    CurrentState currentState = CurrentState.Waiting;

    ShootableTarget targetBase;
    monsterspawnSound spawnSound;

    private void Awake()
    {
        spawnSound = FindAnyObjectByType<monsterspawnSound>();
        targetBase = GetComponent<ShootableTarget>();
        targetBase.anim = GetComponentInChildren<Animator>();
        //targetBase.anim.CrossFade("UpState", 0, 0);

        if (moveType == MoveType.Waypoints)
        {
            for (int i = 0; i < waypointParent.childCount; i++)
            {
                waypoints.Add(waypointParent.GetChild(i));
            }

            if (waypoints.Count == 0) Debug.LogErrorFormat("Waypoints List is empty, have you Tagged {0} the wrong MoveType?", gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveType == MoveType.Waypoints)
            MoveWaypoints();
    }

    void MoveWaypoints() //State Machine for a target moving with Waypoints
    {
        switch (currentState)
        {
            case CurrentState.Moving:
                WaypointMove();
                break;
            case CurrentState.Waiting:
                //StartCoroutine(nameof(WaypointWait));
                WaypointWait();
                break;
            case CurrentState.Idle:
                ReturnToStart();
                break;
            default:
                break;
        }
    }

    void WaypointMove()
    {
        if (!shouldMove) { return; }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentwaypoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentwaypoint].position) < 0.1f)
        {
            currentwaypoint = NewWaypoint(); //Get our new waypoint, depending on if we're moving sequential or random

            waitTimer = waitTime;
            currentState = CurrentState.Waiting;
            acceleration = 0;
        }
    }

    int NewWaypoint()
    {
        int newWayPoint = 0;

        if (randomWaypoint) //move random
        {
            newWayPoint = Random.Range(0, waypoints.Count);

            //if it rolls 3 twice it's fine, it just waits an extra half-second or something
            if (newWayPoint == currentwaypoint && currentwaypoint < waypoints.Count - 1)
                newWayPoint++;

            return newWayPoint;

        }

        else if (pingPong) //cycle back and forth waypoints
        {
            if (currentwaypoint < waypoints.Count - 1 && ascending)
                newWayPoint = currentwaypoint += 1;

            else if (currentwaypoint == waypoints.Count - 1 && ascending)
            {
                newWayPoint = currentwaypoint -= 1;
                ascending = false;
            }

            else if (currentwaypoint > 0 && !ascending)
                newWayPoint = currentwaypoint -= 1;

            else if (currentwaypoint == 0 && !ascending)
            {
                newWayPoint = currentwaypoint += 1;
                ascending = true;
            }

            return newWayPoint;
        }

        else //move in sequence
        {
            if (currentwaypoint < waypoints.Count - 1)
                newWayPoint = currentwaypoint += 1;
            else
            {
                newWayPoint = 0;
                PlayAwakeSound();
            }

            return newWayPoint;
        }
    }

    void WaypointWait()
    {
        if (!shouldMove)
            return;

        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0)
        {
            currentState = CurrentState.Moving;
        }
    }

    void ReturnToStart()
    {
        if (returnBuffer > 0) returnBuffer -= Time.deltaTime;

        else
        {
            currentwaypoint = 0;

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentwaypoint].position, moveSpeed * Time.deltaTime);
        }
    }

    public void ManualChangeState(CurrentState stateToChangeTo) //should you need to manually change the state of a target
    {
        if (stateToChangeTo == CurrentState.Idle)
            returnBuffer = .3f;

        currentState = stateToChangeTo;
    }

    public void ManualSetDownTarget()
    {
        targetBase.anim.SetTrigger("hit");
    }

    public void ManualSetUpTarget()
    {
        targetBase.anim.ResetTrigger("hit");
        targetBase.anim.SetTrigger("getUp");
    }

    void PlayAwakeSound()
    {
        var temp = spawnSound.PlaySpawnSound(this.targetBase.monsterType, this.targetBase.audSource);
        this.targetBase.audSource.PlayOneShot(temp);
    }
}
