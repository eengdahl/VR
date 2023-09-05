using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] bool hit; //for testing purposes

    Quaternion startRot = new(0, 0, 0, 0);
    Quaternion downRot = new(-90, 0, 0, 0);
    [SerializeField] float rotateSpeed;

    [SerializeField] float downTime = 2f;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //for testing purposes
        if (hit)
            OnHit();
    }

    public void OnHit()
    {
        //Idk what to do here yet
        StartCoroutine(nameof(PlayHitAnim));
    }

    IEnumerator PlayHitAnim()
    {
        anim.SetTrigger("hit");

        yield return new WaitForSeconds(downTime);

        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
    }
}
