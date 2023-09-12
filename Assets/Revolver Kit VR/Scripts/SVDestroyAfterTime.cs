using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVDestroyAfterTime : MonoBehaviour
{
    public float secondsToLive = 5.0f;
    public bool startManaully = false;

    private float startTime;
    private bool isStarted = false;
    // Use this for initialization
    void Awake()
    {

        Invoke(nameof(destroyMe), 5);
    }

    void Update()
    {
        if (startManaully && !isStarted)
        {
            return;
        }
        if (Time.time - startTime > secondsToLive)

        {
            Destroy(gameObject);
        }
    }
        void destroyMe()
    {
        Destroy(this.gameObject);
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isStarted = true;
    }


}
