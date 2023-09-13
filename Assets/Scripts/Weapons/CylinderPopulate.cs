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
    public List<GameObject> bullets = new List<GameObject>();
    private bool[] bulletSpent;


    // Start is called before the first frame update
    void Start()
    {
        bulletSpent = new bool[numberOfChambers];
        FillBarrel(6);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isAnimating)

        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, this.rotationTarget, rotationSpeed);
            float curAngle = Quaternion.Angle(this.transform.localRotation, this.rotationTarget);
            if (curAngle < 1)
            {
                this.isAnimating = false;
            }
        }
    }

    void FillBarrel(int amount)
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
                GameObject bullet = Instantiate(bulletPrefab, this.transform);
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
        cylinderOpen = true;
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
        }

        bullets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(".44"))
        {
            FillBarrel(1);
            Destroy(other.gameObject);
        }
    }
}
