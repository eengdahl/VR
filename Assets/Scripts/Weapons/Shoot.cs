using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionProperty leftWeaponTrigger;
    public InputActionProperty rightWeaponTrigger;
    public InputActionProperty leftFanReleased;
    public InputActionProperty rightFanReleased;
    public GameObject linePrefab;
    public GameObject smokePuffPS;
    public GameObject hitSparkPS;
    BulletPool bulletPool;
    public Transform gun;
    public Transform gunhead;
    private bool shooting = false;
    private bool isCock;
    private bool equipped = false;
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
            float leftTriggerHeld = leftWeaponTrigger.action.ReadValue<float>();
            float righttriggerHeld = rightWeaponTrigger.action.ReadValue<float>();
            //Debug.Log(triggerHeld);
            bool leftTriggerValue = leftWeaponTrigger.action.WasPressedThisFrame();
            bool rightTriggerValue = rightWeaponTrigger.action.WasPressedThisFrame();

            bool leftFanReleased = this.leftFanReleased.action.WasReleasedThisFrame();
            bool rightFanReleased = this.rightFanReleased.action.WasReleasedThisFrame();
            if (leftTriggerHeld != 0 || righttriggerHeld != 0)
            {
                if (leftFanReleased || rightFanReleased)
                {
                    Fire();
                }
            }

            if (leftTriggerValue || rightTriggerValue)
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

        var Line = GetLine();
        Line.GetComponent<LineController>().DrawLine(gunhead.localToWorldMatrix.GetPosition(), hit.point);
        if (hit.collider == null)
        {
            return;
        }
        else
        {

        if (hit.collider.CompareTag("Ground"))
        {
            Instantiate(smokePuffPS, hit.point, Quaternion.identity);
        }

        if (hit.collider != null && hit.collider.CompareTag("Target"))
        {
            Debug.Log("hit");
            if (hit.collider.gameObject.GetComponent<AudioSource>() != null)
            {
                hit.collider.gameObject.GetComponent<AudioSource>().Play();
            }
            Instantiate(hitSparkPS, hit.point, Quaternion.identity);
            hit.collider.GetComponent<ShootableTarget>().OnHit();
            scoreController.AddScore(100);

        }

        currentAmmo--;
        }
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

    private GameObject GetLine()
    {
        var newLine = Instantiate(linePrefab);
        return newLine;
    }
}
