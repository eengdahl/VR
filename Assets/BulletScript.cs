using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface BulletStats
{
    public void InitialSpeed(float speed = 50);
    public void AimOffset(Vector3 offset);
    public bool HitTarget();

}
public class BulletScript : MonoBehaviour, BulletStats
{
    private Transform parent;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //parent = GetComponentInParent<Transform>();
        //transform.position = parent.transform.position;
        //transform.rotation = parent.transform.rotation;
    }

    private void OnDisable()
    {
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        //rb.rotation = Quaternion.identity;
    }

    public void InitialSpeed(float speed)
    {

    }
    public void AimOffset(Vector3 offset)
    {

    }
    public bool HitTarget()
    {

        if (true)
        {
            return true;
        }
        return false;
    }
}
