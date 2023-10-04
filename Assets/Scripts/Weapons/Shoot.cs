using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour
{
    public InputActionProperty leftWeaponTrigger;
    public InputActionProperty rightWeaponTrigger;
    public InputActionProperty leftFanReleased;
    public InputActionProperty rightFanReleased;
    public InputActionProperty revolverCock;


    public GameObject linePrefab;
    public GameObject smokePuffPS;
    public GameObject hitSparkPS;
    public GameObject pumpkinSplat;
    private ParticleSystem muzzlePS;
    BulletPool bulletPool;
    public Transform gunhead;
    private bool shooting = false;
    public bool isCock = false;
    private bool equipped = false;
    ScoreController scoreController;

    //Realoding
    private float reloadTime = 1.5f;
    private int maxAmmo = 1000;
    public int currentAmmo;
    public int magSize;
    private AudioSource audioSource;
    public AudioClip emptyClip;
    public AudioClip reloadingSound;
    public AudioClip fanShootSound;
    public AudioClip ShootSound;

    //HitOffset
    RaycastHit hit;
    private float offsetTimer;
    private float gunStabilizer;
    private float burst;
    public Vector3 offset;

    public GameController gameController;

    public XRBaseController rightControllerAction;
    public XRBaseController leftControllerAction;
    public Transform rightController;
    public Transform leftController;
    public XRGrabInteractable grabscript;

    //Play state (round started or not), controlled and updated by GameController
    public bool playing;

    public GameState currentGameState;


    public Animator revolverAnims;
    HapticScript haptic;
    CylinderPopulate cylinderScript;

    private void Start()
    {
        cylinderScript = FindObjectOfType<CylinderPopulate>();
        haptic = FindAnyObjectByType<HapticScript>();
        bulletPool = FindAnyObjectByType<BulletPool>();
        scoreController = FindObjectOfType<ScoreController>();
        gunStabilizer = 0.2f;
        grabscript = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        muzzlePS = GetComponentInChildren<ParticleSystem>();
    }

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
                Fire();
        }


        if (equipped)
        {
            //float leftTriggerHeld = leftWeaponTrigger.action.ReadValue<float>();
            //float righttriggerHeld = rightWeaponTrigger.action.ReadValue<float>();

            bool leftTriggerValue = leftWeaponTrigger.action.WasPressedThisFrame();
            //bool rightTriggerValue = rightWeaponTrigger.action.WasPressedThisFrame();

            bool leftFanReleased = this.leftFanReleased.action.WasReleasedThisFrame();
            //bool rightFanReleased = this.rightFanReleased.action.WasReleasedThisFrame();


            if (leftFanReleased)
            {
                if (currentGameState == GameState.inGame)
                {
                    if (currentAmmo > 0 && !cylinderScript.cylinderOpen)
                    {
                        audioSource.clip = fanShootSound;
                        Fire();
                    }
                    else audioSource.PlayOneShot(emptyClip, 0.6f);
                }
                if (currentGameState == GameState.inMenu)
                {
                    BlankFire();
                }
            }


            if (leftTriggerValue)
            {
                if (currentGameState == GameState.inGame)
                {
                    if (currentAmmo > 0 && !cylinderScript.cylinderOpen)
                    {
                        audioSource.clip = ShootSound;
                        Fire();
                    }
                    else
                    {
                        audioSource.PlayOneShot(emptyClip, 0.6f);
                    }

                }
                if (currentGameState == GameState.inMenu)
                {
                    BlankFire();
                }
            }
        }
    }

    private void Fire()
    {
        cylinderScript.Revolve();
        HapticCall();

        muzzlePS.Play();

        audioSource.pitch = Random.Range(0.80f, 1.20f);
        audioSource.Play();
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
        physBullet.GetComponent<Rigidbody>().velocity = gunhead.transform.forward * 200;
        physBullet.transform.position = gunhead.transform.position;
        physBullet.transform.rotation = gunhead.transform.rotation;
        physBullet.GetComponent<TrailRenderer>().enabled = true;

        //  physBullet.GetComponent<BulletScript>().AimOffset(offset);


        //replace with trailrenderer on physical bullets
        //var Line = GetLine();
        //Line.GetComponent<LineController>().DrawLine(gunhead.localToWorldMatrix.GetPosition(), hit.point);
        currentAmmo--;
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
                gameController.BulletFired(true);
                if (hit.collider.gameObject.GetComponent<AudioSource>() != null)
                {
                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                }
                Instantiate(hitSparkPS, hit.point, Quaternion.identity);

                if (hit.collider.gameObject.GetComponent<ShootableTarget>() != null)
                {
                    hit.collider.gameObject.GetComponent<ShootableTarget>().OnHit();
                }
                if (hit.collider.gameObject.GetComponentInParent<ShootableBoss>() != null)
                {
                    hit.collider.gameObject.GetComponentInParent<ShootableBoss>().OnHit();
                }
                //score is now given in ShootableTarget!
            }

            if (hit.collider != null && hit.collider.CompareTag("UI"))
            {
                gameController.BulletFired(false);
                if (hit.collider.gameObject.GetComponent<ShootableButton>() != null)
                {
                    hit.collider.gameObject.GetComponent<ShootableButton>().TriggerButton();
                }
                else if (hit.collider.gameObject.GetComponent<LockScript>() != null)
                {
                    hit.collider.GetComponent<LockScript>().TriggerAnim();
                }


            }


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
        physBullet.GetComponent<Rigidbody>().velocity = gunhead.transform.forward * 100;
        physBullet.transform.position = gunhead.transform.position;
        physBullet.transform.rotation = gunhead.transform.rotation;
        physBullet.GetComponent<TrailRenderer>().enabled = true;

        //  physBullet.GetComponent<BulletScript>().AimOffset(offset);



        //var Line = GetLine();
        //Line.GetComponent<LineController>().DrawLine(gunhead.localToWorldMatrix.GetPosition(), hit.point);

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
                gameController.BulletFired(false);
                if (hit.collider.gameObject.GetComponent<ShootableButton>() != null)
                {
                    hit.collider.gameObject.GetComponent<ShootableButton>().TriggerButton();
                }
                else if (hit.collider.gameObject.GetComponent<LockScript>() != null)
                {
                    hit.collider.GetComponent<LockScript>().TriggerAnim();
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {
                SceneManager.LoadScene(0);
            }
            else if (hit.collider.CompareTag("Pumpkin"))
                Instantiate(pumpkinSplat, hit.point, Quaternion.identity);

        }
    }


    private void HapticCall()
    {

        bool tempHand = UsingHand.Instance.usingRighthand;
        if (tempHand)
            haptic.TriggerHapticEvent(rightControllerAction);
        else
        {
            haptic.TriggerHapticEvent(leftControllerAction);
        }
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
