using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    RaycastHit hit;
    public GameObject linePrefab;
    PickupTest pickupScript;
    AudioSource gunAS;
    DecalPainter decalPainter;
    public Transform gunTip;
    public GameObject smokePuff;
    // Start is called before the first frame update
    void Start()
    {
        pickupScript = FindObjectOfType<PickupTest>();
        gunAS = GameObject.Find("Gun").GetComponent<AudioSource>();
        decalPainter = FindObjectOfType<DecalPainter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && pickupScript.gunEquipped)
        {
            gunAS.Play();

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f))
            {
                var Line = GetLine();
                Line.GetComponent<LineController>().DrawLine(gunTip.localToWorldMatrix.GetPosition(), hit.point);
                decalPainter.PaintDecal(hit.point, hit.normal, hit.collider);
                if (hit.collider.CompareTag("Bottle"))
                {

                    Vector3 direction = hit.rigidbody.transform.position - hit.point;
                    hit.rigidbody.AddForceAtPosition(direction * 1000, hit.point);
                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                }
                if (hit.collider.CompareTag("Ground"))
                {
                    Instantiate(smokePuff, hit.point, Quaternion.identity);
                }
            }

        }
    }

    private GameObject GetLine()
    {
        var newLine = Instantiate(linePrefab);
        return newLine;
    }
}
