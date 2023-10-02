using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGrave : MonoBehaviour
{
    float startRot;
    [SerializeField] float endRot;
    [SerializeField] float rotSpeed;
    Quaternion targetRot;

    private void Awake()
    {
        startRot = transform.rotation.z;
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
    }

    public void SetRotation()
    {
        targetRot = Quaternion.Euler(new(-90, 0, endRot));
    }

    public void ResetRotation()
    {
        targetRot = Quaternion.Euler(new(-90, 0, startRot));
    }
}
