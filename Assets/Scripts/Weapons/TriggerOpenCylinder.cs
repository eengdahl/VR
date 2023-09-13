using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenCylinder : MonoBehaviour
{
    CylinderPopulate cylinderScript;
    public Collider triggerCollider;

    private void Awake()
    {
        cylinderScript = gameObject.GetComponentInParent<CylinderPopulate>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == triggerCollider)
        {
            cylinderScript.CloseCylinder();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider)
        {
            cylinderScript.OpenCylinder();
        }
    }
}
