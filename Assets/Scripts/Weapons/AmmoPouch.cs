using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AmmoPouch : MonoBehaviour
{
    public Transform headsetPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        transform.position = new Vector3(headsetPos.position.x - 0.2f, headsetPos.position.y - 0.4f, headsetPos.position.z);
    }
}
