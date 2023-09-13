using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderShell : MonoBehaviour
{
    public IEnumerator EnablePhysics()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
