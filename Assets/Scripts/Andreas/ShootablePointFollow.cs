using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootablePointFollow : MonoBehaviour
{
    [SerializeField] Transform pointToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = pointToFollow.position;
    }
}
