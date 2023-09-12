using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderPopulate : MonoBehaviour
{

    public int numberOfChambers = 6;
    public Collider[] gunColliders;
    public float bulletPlacementRadius = 0.0102f;
    public float bulletForwardAxisOffset = 0.0092f;

    private int currentRevolvIndex = 0;
    private Quaternion rotationTarget;
    private bool isAnimating = false;
    private bool cylinderOpen = false;
    public float rotationSpeed = 0.1f;



    public GameObject gun;
    public GameObject bulletPrefab;
    private GameObject[] bullets;
    private bool[] bulletSpent;


    // Start is called before the first frame update
    void Start()
    {
        bullets = new GameObject[numberOfChambers];
        bulletSpent = new bool[numberOfChambers];
        FillBarrel();
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.isAnimating)

        //{
        //    transform.localRotation = Quaternion.Lerp(transform.localRotation, this.rotationTarget, rotationSpeed);
        //    float curAngle = Quaternion.Angle(this.transform.localRotation, this.rotationTarget);
        //    if (curAngle < 1)
        //    {
        //        this.isAnimating = false;
        //    }
        //}
    }

    void FillBarrel()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }

        for (int i = 0; i < numberOfChambers; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, this.transform);
            float degrees = AngleForIndex(i);
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

            bulletSpent[i] = false;
            bullets[i] = bullet;
        }
    }

    float AngleForIndex(int curIndex)
    {
        return 360.0f * ((float)curIndex / (float)numberOfChambers);
    }

    Quaternion RotationForIndex(int curIndex)
    {
        float angle = AngleForIndex(curIndex);
        return Quaternion.AngleAxis(angle, Vector3.left);
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
        Debug.Log("Open");
        cylinderOpen = true;
    }

    public void CloseCylinder()
    {
        Debug.Log("Closed");
        cylinderOpen = false;
    }

    public void ReleaseBullets()
    {
            Debug.Log("shouldfall");
            foreach (GameObject bullet in bullets)
            {
                bullet.GetComponent<CylinderShell>().StartCoroutine("EnablePhysics");
                bullet.transform.parent = null;
                bullet.GetComponent<Rigidbody>().isKinematic = false;
            }
    }
}
