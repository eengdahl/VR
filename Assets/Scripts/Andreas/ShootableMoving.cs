using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMoving : ShootableTarget
{
    public enum MoveType { Waypoints, FlipUp }
    [Tooltip("Should the target move from point to point or only flip up?")]
    [SerializeField] public MoveType moveType;

    [Header("Waypoints")]
    [Tooltip("An empty gameobject with the transforms of the waypoints as children")]
    [SerializeField] Transform waypointParent;
    List<Transform> waypoints = new(); //added automatically

    [Tooltip("Should the target move in sequence or randomly?")]
    [SerializeField] bool randomWaypoint = false;

    [Tooltip("Should the target teleport back when reaching the last waypoint?")]
    [SerializeField] bool teleportToStart = false;

    [Tooltip("Should the target accelerate? Don't use for normal targets!")]
    [SerializeField] bool smoothMovement = false;

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

    float returnBuffer;
    [HideInInspector] public bool shouldMove;

    public enum CurrentState { Moving, Waiting, Idle }
    CurrentState currentState = CurrentState.Waiting;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        if (testTarget)
        {
            if (moveType == MoveType.Waypoints)
            {
                for (int i = 0; i < waypointParent.childCount; i++)
                {
                    waypoints.Add(waypointParent.GetChild(i));
                }

                if (waypoints.Count == 0) Debug.LogErrorFormat("Waypoints List is empty, have you Tagged {0} the wrong MoveType?", gameObject.name);
            }
        }
    }

    public void InitiatePatrol(GameObject newParent)
    {
        waypointParent = newParent.transform;
        //Create our list of waypoints and warns if it's empty
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

        if (!smoothMovement)
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentwaypoint].position, moveSpeed * Time.deltaTime);
        else
        {
            if (Vector3.Distance(transform.position, waypoints[currentwaypoint].position) > 5f)
                if (acceleration <= moveSpeed) acceleration += Time.deltaTime * 3;

            if (Vector3.Distance(transform.position, waypoints[currentwaypoint].position) < 5f)
                acceleration -= Time.deltaTime * 3;

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentwaypoint].position, acceleration * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, waypoints[currentwaypoint].position) < 0.1f)
        {
            currentwaypoint = NewWaypoint(); //Get our new waypoint, depending on if we're moving sequential or random

            waitTimer = waitTime;
            currentState = CurrentState.Waiting;
            acceleration = 0;

            if (currentwaypoint == 0 && teleportToStart)
            {
                transform.position = waypoints[0].position;
            }
        }
    }

    int NewWaypoint()
    {
        int newWayPoint = 0;

        if (!randomWaypoint) //move in sequence
        {
            if (currentwaypoint < waypoints.Count - 1)
                newWayPoint = currentwaypoint += 1;
            else
                newWayPoint = 0;

            return newWayPoint;
        }
        else //otherwise, pick a random point
        {
            newWayPoint = Random.Range(0, waypoints.Count);

            //if it rolls 3 twice it's fine, it just waits an extra half-second or something
            if (newWayPoint == currentwaypoint && currentwaypoint < waypoints.Count - 1)
                newWayPoint++;

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
        anim.SetTrigger("hit");
    }

    public void ManualSetUpTarget()
    {
        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
    }
}
