using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMoving : MonoBehaviour
{
    enum MoveType { Waypoints, FlipUp }
    [SerializeField] MoveType moveType;

    [Header("Waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float moveSpeed = 1f;
    public int currentwaypoint;
    enum CurrentState { Moving, Waiting }
    CurrentState currentState = CurrentState.Waiting;

    // Update is called once per frame
    void Update()
    {
        if (moveType == MoveType.Waypoints)
            MoveWaypoints();
    }

    void MoveWaypoints()
    {
        switch (currentState)
        {
            case CurrentState.Moving:
                WaypointMove();
                break;
            case CurrentState.Waiting:
                StartCoroutine(nameof(WaypointWait));
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
            if (currentwaypoint < waypoints.Length - 1)
                currentwaypoint++;
            else
                currentwaypoint = 0;

            currentState = CurrentState.Waiting;
        }
    }

    IEnumerator WaypointWait()
    {
        yield return new WaitForSeconds(waitTime);

        currentState = CurrentState.Moving;
    }
}
