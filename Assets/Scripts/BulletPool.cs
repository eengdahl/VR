using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public GameObject bulletPrefab;
    public int maxBullet = 100;
    public Queue<GameObject> bullets = new Queue<GameObject>();

    private void Start()
    {
        transform.forward = Vector3.right;
        bullets ??= new Queue<GameObject>();
    }




    private void OnEnable()
    {
        bullets ??= new Queue<GameObject>();

        CreateBullets(10);
         DeactivateBullets();

        Invoke(nameof(TestShoot), 2);
    }




    private void CreateBullets(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.parent = this.transform;
            //newBullet.transform.parent = transform;
             bullets.Enqueue(newBullet);
           // DisableBullet(newBullet);
        }
    }
    public void DisableBullet(GameObject usedBullet)
    {
        usedBullet.transform.parent = null;
        // usedBullet = bullets.Dequeue();
        usedBullet.SetActive(false);
        bullets.Enqueue(usedBullet);
    }
    private void DeactivateBullets()
    {
        int amount = bullets.Count;
        for (int i = 0; i < amount; i++)
        {
            var bullet = bullets.Dequeue();
            bullet.transform.parent = null;
          //  bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }


    private void TestShoot()
    {
        GameObject bullet = GetBullet();
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        Invoke(nameof(TestShoot), 1);
    }

    private GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            var newBullet = Instantiate(bulletPrefab);
            return newBullet;
        }
        var bullet = bullets.Dequeue();
        
        bullet.gameObject.SetActive(true);
        //  bullets.Enqueue(bullet);
        return bullet;
    }
}
