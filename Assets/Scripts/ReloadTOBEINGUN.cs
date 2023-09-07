using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadTOBEINGUN : MonoBehaviour
{
    private int maxAmmo = 1000;
    public int currentAmmo;
    public int magSize = 10;
    private float reloadTime = 1f;
    private bool reloading = false;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }


    private void OnEnable()
    {
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magSize;
        reloading = false;
    }
}
