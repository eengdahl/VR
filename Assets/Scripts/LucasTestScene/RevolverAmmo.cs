using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverAmmo : MonoBehaviour
{
    public bool collision = false;
    public int time = 20;

    public void EnableCollision()
    {
        collision = !collision;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void DestroyAfterTime()
    {
        Invoke(nameof(DestroyForReal), time);
    }

    public void DestroyForReal()
    {

        Destroy(gameObject);
    }

}
