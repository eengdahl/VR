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
        bullets ??= new Queue<GameObject>();
    }
    private void OnEnable()
    {
        //Creating new queue if non exists 
        bullets ??= new Queue<GameObject>();

        PopulateBulletPool(20);
    }


    private void PopulateBulletPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newBullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
            newBullet.transform.parent = parent;
            bullets.Enqueue(newBullet);
        }
        DisableInitialBullets();
    }
    public void DisableBullet(GameObject usedBullet)
    {
        //Put bullet to sleep
        usedBullet.transform.parent = null;
        usedBullet.SetActive(false);
        bullets.Enqueue(usedBullet);
    }
    private void DisableInitialBullets()
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
        //If no bullets is non-active
        if (bullets.Count == 0)
        {
            var newBullet = Instantiate(bulletPrefab);
            return newBullet;
        }
        //If bullets are disable, get from here
        var bullet = bullets.Dequeue();

        bullet.gameObject.SetActive(true);
        return bullet;
    }
}
