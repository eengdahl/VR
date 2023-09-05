using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTest : MonoBehaviour
{
    RaycastHit hit;
    public Transform hand;
    public bool gunEquipped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10.0f, Color.yellow);

        if (Input.GetKeyDown("e"))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10.0f))
            {
                if (hit.collider.CompareTag("Gun"))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.transform.parent != hand)
                    {
                        Rigidbody gunRigidbody = hit.collider.transform.GetComponent<Rigidbody>();
                        if (gunRigidbody != null)
                        {
                            gunRigidbody.isKinematic = true;
                        }
                        hit.collider.transform.localRotation = Quaternion.identity;
                        hit.collider.transform.Rotate(hand.rotation.eulerAngles);
                        hit.collider.transform.SetParent(hand);
                        hit.collider.transform.localPosition = Vector3.zero;
                        gunEquipped = true;
                    }
                }
            }

        }
    }
}
