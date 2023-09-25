using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public GameObject bulletPrefab;
    public int maxBullet = 100;
    private Transform parent;
    public Queue<GameObject> bullets = new Queue<GameObject>();

    private void Start()
    {
        parent = this.gameObject.transform;
        //transform.forward = Vector3.right;
        bullets ??= new Queue<GameObject>();
    }
    private void OnEnable()
    {
        bullets ??= new Queue<GameObject>();

        CreateBullets(10);
    }


    private void CreateBullets(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.parent = parent;
            bullets.Enqueue(newBullet);
        }
        DeactivateBullets();
    }
    public void DisableBullet(GameObject usedBullet)
    {
        usedBullet.transform.parent = null;
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
            bullets.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            var newBullet = Instantiate(bulletPrefab);
            return newBullet;
        }
         var bullet = bullets.Dequeue();

        bullet.gameObject.SetActive(true);
        return bullet;
    }
}
