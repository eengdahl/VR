using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProp : MonoBehaviour
{
    [Header("Waypoints")]
    [Tooltip("An empty gameobject with the transforms of the waypoints as children")]
    [SerializeField] Transform waypointParent;
    List<Transform> waypoints = new(); //added automatically

    [Tooltip("Should the target teleport back when reaching the last waypoint?")]
    [SerializeField] bool teleportToStart = false;

    [Tooltip("Should the target accelerate? Don't use for normal targets!")]
    [SerializeField] bool smoothMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
