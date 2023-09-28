using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void TriggerAnim()
    {
        gameObject.GetComponentInParent<Animator>().CrossFade("LockFall", 0);
    }
}
