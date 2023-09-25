using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverAmmo : MonoBehaviour
{
    public bool collision = false;

    public void EnableCollision()
    {
        collision = true;
    }

    public void DestroyAfterTime()
    {
        Destroy(gameObject, 20);
    }

}
