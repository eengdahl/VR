using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ontriggerdropzone : MonoBehaviour
{
    public CylinderPopulate cylinderScript;
    public Collider dropzone;
    private void OnTriggerEnter(Collider other)
    {
        if (other == dropzone)
        {
            Debug.Log("Enter");

        }
        if (other == dropzone)
        {
            cylinderScript.ReleaseBullets();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == dropzone)
        {
            Debug.Log("Exit");
        }
    }
}
