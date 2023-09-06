using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMoving : MonoBehaviour
{
    enum MoveType { Waypoints, FlipUp }
    [Tooltip("Should the target move from point to point or only flip up? NOTE! FlipUp is not finished")]
    [SerializeField] MoveType moveType;

    [Header("Waypoints")]
    [Tooltip("An empty gameobject with the transforms of the waypoints as children")]
    [SerializeField] Transform waypointParent;
    List<Transform> waypoints = new(); //added automatically
    [Tooltip("Should the target move in sequence or randomly?")]
    [SerializeField] bool randomWaypoint = false;
    [Header("Variables")]
    [Tooltip("How long the target should wait at each waypoint")]
    [SerializeField] float waitTime = 1f;
    [Tooltip("How fast the target is walking between waypoints, Recommended between 1 and 5")]
    [SerializeField] float moveSpeed = 1f;
    float waitTimer;
    [Tooltip("Only viewable for debugging purposes, don't touch!")]
    [SerializeField] int currentwaypoint;
    enum CurrentState { Moving, Waiting }
    CurrentState currentState = CurrentState.Waiting;

    private void Start()
    {
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
            default:
                break;
        }
    }

    void WaypointMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentwaypoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentwaypoint].position) < 0.1f)
        {
            currentwaypoint = NewWaypoint(); //Get our new waypoint, depending on if we're moving sequential or random

            waitTimer = waitTime;
            currentState = CurrentState.Waiting;
        }
    }

    int NewWaypoint()
    {
        int newWayPoint = 0;

        if (!randomWaypoint)
        {
            if (currentwaypoint < waypoints.Count - 1)
                newWayPoint = currentwaypoint++;
            else
                newWayPoint = 0;

            return newWayPoint;
        }
        else
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
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0)
        {
            currentState = CurrentState.Moving;
        }
    }

    //IEnumerator WaypointWait()
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    currentState = CurrentState.Moving;
    //}
}
