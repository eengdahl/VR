using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenCylinder : MonoBehaviour
{
    CylinderPopulate cylinderScript;
    ScoreController scoreController;
    public Collider triggerCollider;

    private void Awake()
    {
        scoreController = FindAnyObjectByType<ScoreController>();
        cylinderScript = gameObject.GetComponentInParent<CylinderPopulate>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == triggerCollider)
        {
            cylinderScript.CloseCylinder();
            //scoreController.EndCombo();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == triggerCollider)
        {
            cylinderScript.OpenCylinder();
            //scoreController.StartCombo();
        }
    }
}
