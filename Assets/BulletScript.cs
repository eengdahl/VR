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
   // public Transform parent;
    private Rigidbody rb;
    private float deathTimer = 5;
    BulletPool pool;
    bool firstRun = true;


    private void Start()
    {
        pool = FindAnyObjectByType<BulletPool>();
        rb = GetComponent<Rigidbody>();
        firstRun = true;
    }

    private void OnEnable()
    {
        //if (firstRun)
        //{
        //    firstRun = false;
        //    return;
        //}

        Invoke(nameof(DisableMe), 1);
        Debug.Log("ping");

       // parent = GetComponentInParent<Transform>();
       transform.position=GetComponentInParent<Transform>().position;
        transform.rotation = GetComponentInParent<Transform>().rotation;
        //transform.position = parent.transform.position;
      //  transform.rotation = parent.transform.rotation;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
    }

    public void DisableMe()
    {
        pool.DisableBullet(this.transform.gameObject);
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
