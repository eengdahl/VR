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
        headsetPos = Camera.main.transform;
    }

    private void Update()
    {
        transform.position = new Vector3(headsetPos.position.x - 0.3f, headsetPos.position.y - 0.5f, headsetPos.position.z);
    }
}
