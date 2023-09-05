using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTest : MonoBehaviour
{

    PickupTest pickupScript;

    private GameObject gun;
    public GameObject eyes;
    public GameObject hand;
    private float aimLerpSpeed = 5f;

    // Start is called before the first frame update
    private void Start()
    {
        pickupScript = FindObjectOfType<PickupTest>();
        gun = GameObject.FindGameObjectWithTag("Gun");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (pickupScript.gunEquipped)
            {
                Debug.Log("Gun equipped and aiming");
                gun.transform.position = Vector3.Lerp(gun.transform.position, eyes.transform.position, Time.deltaTime * aimLerpSpeed);
            }
        }
        else
        {
            if (pickupScript.gunEquipped)
            {
                gun.transform.position = Vector3.Lerp(gun.transform.position, hand.transform.position, Time.deltaTime * aimLerpSpeed);
            }
        }
    }
}
