using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] bool hit; //for testing purposes

    Quaternion startRot = new(0, 0, 0, 0);
    Quaternion downRot = new(-90, 0, 0, 0);
    [SerializeField] float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        //for testing purposes
        if (hit)
            OnHit();
    }

    public void OnHit()
    {
        //Idk what to do here yet

    }
}
