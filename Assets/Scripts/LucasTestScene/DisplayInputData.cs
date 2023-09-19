using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
[RequireComponent(typeof(InputData))] 
public class DisplayInputData : MonoBehaviour
{
    private InputData inputData;
    // Start is called before the first frame update
    void Start()
    {
        inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        //get leftcontroller velocity
        if (inputData.leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            //use velocity here
        }
        //get rightcontroller velocity
        if (inputData.leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            //use velocity here
        }
    }
}
