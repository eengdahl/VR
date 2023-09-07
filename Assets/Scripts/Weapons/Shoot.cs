using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionProperty weaponTrigger;
    public InputActionProperty fanRelease;
    BulletPool bulletPool;
    public Transform gun;
    public Transform gunhead;
    private bool shooting = false;
    private bool isCock;
    // Start is called before the first frame update
    private void Start()
    {
        bulletPool = FindAnyObjectByType<BulletPool>();

    }


    // Update is called once per frame
    void Update()
    {
        if (!shooting)
        {
            bool triggerHeld = weaponTrigger.action.ReadValue<bool>();
            bool triggerValue = weaponTrigger.action.WasPressedThisFrame();
            bool fanReleased = fanRelease.action.WasReleasedThisFrame();
            if (triggerHeld)
            {
                if (fanReleased)
                {
                    Fire();
                }
            }

            if (triggerValue)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        var aS = gameObject.GetComponent<AudioSource>();
        aS.Play();
        RaycastHit hit;
        Physics.Raycast(gun.position, gun.forward, out hit, 1000);
        //shootcode sound instatiate decal etc

        //Physical bullet
        var physBullet = bulletPool.GetBullet();
        physBullet.transform.parent = gunhead;
        physBullet.transform.position = gunhead.transform.position;
        physBullet.transform.rotation = gunhead.transform.rotation;
        physBullet.GetComponent<Rigidbody>().velocity = gunhead.transform.forward * 100;

        if (hit.collider != null && hit.collider.CompareTag("Target"))
        {
            Debug.Log("hit");
            hit.collider.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
