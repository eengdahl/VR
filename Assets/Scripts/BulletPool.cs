using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public GameObject bulletPrefab;
    public int maxBullet = 100;
    private LinkedList<GameObject> freeBullet = new LinkedList<GameObject>();


    //private void OnDisable()
    //{
    //    foreach (var objPrefab in freeBullet)
    //    {
    //        Destroy(objPrefab);

    //        freeBullet.Clear();
    //    }
    //}





    private void OnEnable()
    {
        for (int i = 0; i < maxBullet; i++)
        {
            var temp = CreateObj();
            DestroyBullet(temp);
        }

        Invoke(nameof(TestShoot), 2);
    }

    private void TestShoot()
    {
        var bullet = CreateObj();

        bullet.transform.up = transform.up;
        bullet.transform.up = this.transform.up;
        bullet.transform.parent = this.transform;
        bullet.transform.position = this.transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Rigidbody>().velocity = transform.up * 10;
        Invoke(nameof(TestShoot), 1);
    }

    public GameObject CreateObj()
    {
        if (freeBullet.Count <= 0)
        {
            return Instantiate(bulletPrefab);
        }

        var freeObjNode = freeBullet.First;
        var tempobject = freeObjNode.Value;
        freeBullet.Remove(freeObjNode);
        tempobject.SetActive(true);


        return tempobject;
    }

    public void DestroyBullet(GameObject usedObj)
    {

        if (freeBullet.Count >= maxBullet)
        {
            Debug.Log(freeBullet.Count);
            Destroy(usedObj);
            return;
        }

        usedObj.transform.parent = null;
        usedObj.SetActive(false);
        // freeBullet.AddFirst(usedObj);
        //  freeBullet.AddLast(usedObj);


    }
}
