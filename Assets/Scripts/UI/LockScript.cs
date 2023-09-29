using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    private GameObject locker;
    // Start is called before the first frame update
    public void TriggerAnim()
    {
        gameObject.GetComponentInParent<Animator>().CrossFade("LockFall", 0);
        Invoke(nameof(Delay), 2);
    }
    public void Delay()
    {
        FindObjectOfType<MovePlayer>().StartWalking();

    }
}
