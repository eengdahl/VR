using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverAmmo : MonoBehaviour
{
    public bool collision = false;

    public void EnableCollision()
    {
        collision = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void DestroyAfterTime()
    {
        Invoke(nameof(DestroyForReal), 20);
    }

    public void DestroyForReal()
    {

        Destroy(gameObject);
    }

}
