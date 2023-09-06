using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionProperty weaponTrigger;
    public Transform gun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool triggerActive = weaponTrigger.action.ReadValue<bool>();
        if (triggerActive)
        {
            var aS = gameObject.GetComponent<AudioSource>();
            aS.Play();
            RaycastHit hit;
            Physics.Raycast(gun.position, gun.forward, out hit, 1000);
            //shootcode sound instatiate decal etc

            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("hit");
                //hit code
            }
        }
    }
}
