using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface BulletStats
{
    public void InitialSpeed(float speed = 500);
    public void AimOffset(Vector3 offset);
    public bool HitTarget();


}
public class BulletScript : MonoBehaviour, BulletStats
{
    // public Transform parent;
    private Rigidbody rb;
    private float deathTimer = 5;
    BulletPool pool;
    public int magazineSize = 6;


    private void Start()
    {
        pool = FindAnyObjectByType<BulletPool>();
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DisableMe), 1);
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableMe), 1);
        //transform.position = GetComponentInParent<Transform>().position;
        //transform.rotation = GetComponentInParent<Transform>().rotation;
    }

    private void OnDisable()
    {
        GetComponent<TrailRenderer>().enabled = false;

   //     rb.velocity = Vector3.zero;
     //   rb.angularVelocity = Vector3.zero;
       // rb.rotation = Quaternion.identity;
    }

    public void DisableMe()
    {

        // this.transform.position = transform.parent.position;
        pool.DisableBullet(this.transform.gameObject);
    }

    public void InitialSpeed(float speed)
    {

    }
    public void AimOffset(Vector3 offset)
    {


        if (offset == Vector3.zero)
        {
            return;
        }
        this.transform.forward = offset;
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
