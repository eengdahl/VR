using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class CylinderPopulate : MonoBehaviour
{
    public AudioClip shellLoaded;
    public AudioClip cylinderSpin;
    private AudioSource aS;
    private bool cylinderSpinOpened;

    public int numberOfChambers = 6;
    public Collider[] gunColliders;
    public float bulletPlacementRadius = 0.0102f;
    public float bulletForwardAxisOffset = 0.0092f;
    public Transform bulletTransform;

    private int currentRevolvIndex = 0;
    private Quaternion rotationTarget;
    private bool isAnimating = false;
    public bool cylinderOpen = false;
    private bool flickTriggered;
    public float rotationSpeed = 0.1f;
    public Vector3 cylinderFlickForce;
    public InputActionProperty righthandReleaseCylinder;
    public InputActionProperty lefthandReleaseCylinder;

    private Rigidbody cylinderRB;
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject cylinderMesh;
    public List<GameObject> bullets = new List<GameObject>();
    private bool[] bulletSpent;

    public InputActionReference righthandGrabSelect;
    public InputActionReference lefthandGrabSelect;

    Shoot shoot;
    DisplayInputData inputData;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        shoot = GetComponentInParent<Shoot>();
        inputData = FindObjectOfType<DisplayInputData>();
        cylinderRB = GetComponent<Rigidbody>();
        bulletSpent = new bool[numberOfChambers];
        FillBarrel(6);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            righthandGrabSelect.action.Enable();
            lefthandGrabSelect.action.Enable();   
        }

        if (this.isAnimating)

        {
            cylinderMesh.transform.localRotation = Quaternion.Lerp(cylinderMesh.transform.localRotation, this.rotationTarget, rotationSpeed);
            float curAngle = Quaternion.Angle(this.transform.localRotation, this.rotationTarget);
            if (curAngle < 1)
            {
                this.isAnimating = false;
            }
        }

        //Debug.Log(transform.localRotation);
        //Input for cylinder release

        //float righthandCylinderRelease = this.righthandReleaseCylinder.action.ReadValue<float>();
        float lefthandCylinderRelease = this.lefthandReleaseCylinder.action.ReadValue<float>();

        //button held, release it
        //if (righthandCylinderRelease != 0)
        //{
        //    cylinderRB.isKinematic = false;

        //}
        ////cylinder returns to default spot and no input, lock it
        //if (transform.localRotation.z < 0 && righthandCylinderRelease == 0)
        //{
        //    cylinderRB.isKinematic = true;
        //    cylinderOpen = false;
        //}

        if (lefthandCylinderRelease != 0)
        {
            cylinderRB.isKinematic = false;
            flickTriggered = false;

        }
        if (transform.localRotation.z < 0 && lefthandCylinderRelease == 0)
        {
            cylinderRB.isKinematic = true;
            cylinderOpen = false;
        }

        if (inputData != null && cylinderOpen && !flickTriggered)
        {

            if (inputData.leftControllerVelocity.magnitude > 1.2f)
            {
                cylinderRB.AddForce(cylinderFlickForce, ForceMode.Impulse);
                flickTriggered = true;
                if (cylinderSpinOpened)
                {
                    aS.clip = cylinderSpin;
                    aS.Play();
                    cylinderSpinOpened = false;
                }
            }
        }
    }
    public void FillBarrel(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            int space = 0;

            if (bullets != null)
            {
                space = bullets.Count;
            }
            else
            {
                space = 0;
            }
            if (space < 6)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletTransform);
                bullet.transform.parent = bulletTransform;
                float degrees = AngleForIndex(space);
                float x = bulletPlacementRadius * Mathf.Cos(degrees * Mathf.Deg2Rad);  // we're rotated so x == z
                float y = bulletPlacementRadius * Mathf.Sin(degrees * Mathf.Deg2Rad);

                bullet.transform.localPosition = new Vector3(bulletForwardAxisOffset, x, y);
                Collider col = bullet.GetComponent<Collider>();
                foreach (Collider col2 in gunColliders)
                {
                    Physics.IgnoreCollision(col, col2);
                }
                col.enabled = false;
                bullet.GetComponent<Collider>().enabled = false;
                bullets.Add(bullet);
                shoot.currentAmmo++;
            }
        }
    }

    float AngleForIndex(int curIndex)
    {
        return 360.0f * ((float)curIndex / (float)numberOfChambers);
    }

    Quaternion RotationForIndex(int curIndex)
    {
        float angle = AngleForIndex(curIndex);
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Revolve()
    {
        bulletSpent[currentRevolvIndex] = true;
        this.rotationTarget = RotationForIndex(++currentRevolvIndex);
        this.isAnimating = true;

        if (currentRevolvIndex > numberOfChambers - 1)
        {
            currentRevolvIndex = 0;
        }
    }

    public void OpenCylinder()
    {
        cylinderOpen = true;
        cylinderSpinOpened = true;
        flickTriggered = false;
    }

    public void CloseCylinder()
    {
        cylinderOpen = false;
    }

    public void ReleaseBullets()
    {
        if (cylinderOpen)
        {
            foreach (GameObject bullet in bullets)
            {
                bullet.GetComponent<CylinderShell>().StartCoroutine("EnablePhysics");
                bullet.transform.parent = null;
                bullet.GetComponent<Rigidbody>().isKinematic = false;
            }
            shoot.currentAmmo = 0;
            bullets.Clear();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(".44"))
        {
            FillBarrel(1);
            aS.clip = shellLoaded;
            aS.Play();
            Destroy(other.gameObject);
        }        
        if (other.CompareTag("SixpackAmmo") && other.gameObject.GetComponent<RevolverAmmo>().collision && bullets.Count == 0 && cylinderOpen)
        {
            FillBarrel(6);
            aS.clip = shellLoaded;
            aS.Play();
            Destroy(other.gameObject);
        }
    }

    public void DisableGrabInput()
    {    
        righthandGrabSelect.action.Disable();
    }
    public void EnableGrabInput()
    {
        righthandGrabSelect.action.Enable();  
    }
}
