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
    public GameObject shell;
    BulletPool bulletPool;
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

    //HitOffset
    RaycastHit hit;
    private float offsetTimer;
    private float gunStabilizer;
    private float burst;
    public Vector3 offset;

    private void Start()
    {
        bulletPool = FindAnyObjectByType<BulletPool>();
        scoreController = FindObjectOfType<ScoreController>();
        currentAmmo = magSize;
        gunStabilizer = 0.2f;

    }
    private void OnEnable()
    {
        reloading = false;
    }


    // Update is called once per frame
    void Update()
    {

        //Counting how many bullets that has been shot in this burst
        offsetTimer += Time.deltaTime;
        if (offsetTimer > gunStabilizer)
        {
            burst = 0;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Fire();
        }


        if (!reloading && equipped)
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
        Instantiate(shell, transform.position, transform.rotation);
        if (burst > 0.5f)
            burst = 0.5f;

        // shots fired rapidly 
        if (burst > 0.1f)
        {
            offset = new Vector3(Random.Range(gunhead.forward.x - burst, gunhead.forward.x + burst), Random.Range(gunhead.forward.y, gunhead.forward.y + burst), gunhead.forward.z);
            Physics.Raycast(gunhead.position, offset, out hit, 1000);
        }
        //RaycastHit hit;
        else
        {
            offset = Vector3.zero;
            Physics.Raycast(gunhead.position, gunhead.forward, out hit, 1000);
        }
        burst += 0.1f;
        offsetTimer = 0;

        //shootcode sound instatiate decal etc

        //Physical bullet
        var physBullet = bulletPool.GetBullet();
        physBullet.transform.parent = gunhead;
        physBullet.transform.position = gunhead.transform.position;
        physBullet.transform.rotation = gunhead.transform.rotation;

        physBullet.GetComponent<BulletScript>().AimOffset(offset);

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
                hit.collider.GetComponentInParent<ShootableTarget>().OnHit();
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

    public void Equipped()
    {
        equipped = true;
        Debug.Log(equipped);
    }

    public void Dropped()
    {
        equipped = false;
        Debug.Log(equipped);

    }
}
