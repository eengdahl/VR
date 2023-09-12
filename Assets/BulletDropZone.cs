using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDropZone : MonoBehaviour
{
    // Reference to the parent transform.
    public Transform parentTransform;
    public Vector3 offsetY;
    public Collider tipCollider;
    public CylinderPopulate cylinderScript;

    void Update()
    {
        if (parentTransform != null)
        {
            // Only follow the parent's position.
            transform.position = parentTransform.position + offsetY;

            // Set the child's local rotation to identity (no rotation).
            transform.rotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == tipCollider)
        {
            cylinderScript.ReleaseBullets();
        }
    }
}



