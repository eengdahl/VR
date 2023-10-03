using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AmmoPouch : MonoBehaviour
{
    public GameObject ammoPrefab;
    private bool ammoReady;
    // Start is called before the first frame update
    void Start()
    {
        ammoReady = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && !ammoReady)
        {
            Instantiate(ammoPrefab, transform.position, Quaternion.identity);
            ammoReady = true;
        }
    }

    public void AmmoPickedUp()
    {
        ammoReady = false;
    }
}
