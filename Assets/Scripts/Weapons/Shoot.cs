using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionProperty weaponTrigger;
    public InputActionProperty fanRelease;
    public GameObject linePrefab;
    BulletPool bulletPool;
    public Transform gun;
    public Transform gunhead;
    private bool shooting = false;
    private bool isCock;
    ScoreController scoreController;

    //Realoding
    private bool reloading = false;
    private float reloadTime = 1.5f;
    private int maxAmmo = 1000;
    public int currentAmmo;
    public int magSize = 10;
    public AudioClip reloadingClip;

    private void Start()
    {
        bulletPool = FindAnyObjectByType<BulletPool>();
        scoreController = FindObjectOfType<ScoreController>();
        currentAmmo = magSize;

    }
    private void OnEnable()
    {
        reloading = false;
    }


    // Update is called once per frame
    void Update()
    {


        if (!reloading)
        {
            float triggerHeld = weaponTrigger.action.ReadValue<float>();
            Debug.Log(triggerHeld);
            bool triggerValue = weaponTrigger.action.WasPressedThisFrame();
            bool fanReleased = fanRelease.action.WasReleasedThisFrame();
            if (triggerHeld != 0)
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

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }

    private void Fire()
    {
        Instantiate(linePrefab);
        var aS = gameObject.GetComponent<AudioSource>();
        aS.pitch = Random.Range(0.80f, 1.20f);
        aS.Play();
        //RaycastHit hit;
        Physics.Raycast(gun.position, gun.forward, out RaycastHit hit, 1000);
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
            if (hit.collider.gameObject.GetComponent<AudioSource>() != null)
            {
                hit.collider.gameObject.GetComponent<AudioSource>().Play();
            }

            hit.collider.GetComponent<ShootableTarget>().OnHit();
            scoreController.AddScore(100);

        }

        currentAmmo--;
    }



    IEnumerator Reloading()
    {
        reloading = true;
        var aS = gameObject.GetComponent<AudioSource>();
        aS.PlayOneShot(reloadingClip);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magSize;
        reloading = false;
    }
}
