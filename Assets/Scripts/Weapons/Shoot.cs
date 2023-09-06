using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionProperty weaponTrigger;
    public Transform gun;
    private bool shooting = false;
    private bool isCock; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!shooting)
        {
            float triggerValue = weaponTrigger.action.ReadValue<float>();
            if (triggerValue != 0)
            {
                StartCoroutine(Shooting());
            }
        }
    }

    IEnumerator Shooting()
    {
        shooting = true;
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
        yield return new WaitForSeconds(0.1f);
        shooting = false;
    }
}
