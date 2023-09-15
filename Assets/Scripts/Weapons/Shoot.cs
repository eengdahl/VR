using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;


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

    public GameController gameController;

    public XRBaseController thisController;
    public Transform rightController;
    public Transform leftController;
    public XRGrabInteractable grabscript;

    //Play state (round started or not), controlled and updated by GameController
    public bool playing;

    HapticScript haptic;

    private void Start()
    {

        haptic = FindAnyObjectByType<HapticScript>();
        bulletPool = FindAnyObjectByType<BulletPool>();
        scoreController = FindObjectOfType<ScoreController>();
        currentAmmo = magSize;
        gunStabilizer = 0.2f;
        grabscript = GetComponent<XRGrabInteractable>();
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
            if (playing)
                Fire();
            else
                BlankFire();
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
                    if (playing)
                        Fire();
                    else
                        BlankFire();
                }
            }

            if (leftTriggerValue || rightTriggerValue)
            {
                if (playing)
                    Fire();
                else
                    BlankFire();
            }

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }

    private void Fire()
    {
        //print("Fired for real for real");

        haptic.TriggerHapticEvent(thisController);
        var aS = gameObject.GetComponent<AudioSource>();
        aS.pitch = Random.Range(0.80f, 1.20f);
        aS.Play();
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
                gameController.BulletFired(false);
            }

            if (hit.collider != null && hit.collider.CompareTag("Target"))
            {
                //Debug.Log("real hit");
                gameController.BulletFired(true);
                if (hit.collider.gameObject.GetComponent<AudioSource>() != null)
                {
                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                }
                Instantiate(hitSparkPS, hit.point, Quaternion.identity);

                hit.collider.gameObject.GetComponent<ShootableTarget>().OnHit();
                scoreController.AddScore(100);
            }

            if (hit.collider != null && hit.collider.CompareTag("UI"))
            {
                gameController.BulletFired(false);
                hit.collider.gameObject.GetComponent<ShootableButton>().TriggerButton();
            }

            currentAmmo--;
        }
    }

    private void BlankFire()
    {
        var aS = gameObject.GetComponent<AudioSource>();
        aS.pitch = Random.Range(0.80f, 1.20f);
        aS.Play();

        //RaycastHit hit;
        Physics.Raycast(gunhead.position, gunhead.forward, out hit, 1000);

        //shootcode sound instatiate decal etc

        //Physical bullet
        var physBullet = bulletPool.GetBullet();
        physBullet.transform.position = gunhead.transform.position;
        physBullet.transform.rotation = gunhead.transform.rotation;

        physBullet.GetComponent<BulletScript>().AimOffset(offset);

        physBullet.GetComponent<Rigidbody>().velocity = gunhead.transform.forward * 100;

        var Line = GetLine();

        Line.GetComponent<LineController>().DrawLine(gunhead.localToWorldMatrix.GetPosition(), hit.point);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Instantiate(smokePuffPS, hit.point, Quaternion.identity);
            }
            else if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("blank hit");
                if (hit.collider.gameObject.GetComponent<AudioSource>() != null)
                {
                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                }
                Instantiate(hitSparkPS, hit.point, Quaternion.identity);
            }
            else if (hit.collider.CompareTag("UI"))
            {
                hit.collider.gameObject.GetComponent<ShootableButton>().TriggerButton();
                Instantiate(hitSparkPS, hit.point, Quaternion.identity);
            }

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
    }

    public void Dropped()
    {
        equipped = false;

    }
}
