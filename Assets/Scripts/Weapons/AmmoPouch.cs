using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AmmoPouch : MonoBehaviour
{
    public InputActionProperty grabAmmo;
    public GameObject ammoPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {

        bool grabButton = grabAmmo.action.WasPressedThisFrame();

        
    }
    public void GrabAmmo()
    {
        //Instantiate()
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hand"))
        {
            Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        }
    }
}
