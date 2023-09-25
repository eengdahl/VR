using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownSign : MonoBehaviour
{
    public Animator anim; 
    [SerializeField] private int signNumber;
    
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void MoveUpSigns()
    {
        
        StartCoroutine(nameof(MoveSignUp));
    }
    
    public void CountdownSignFlip()
    {
        StartCoroutine(nameof(PlayHitAnim));
    }

    IEnumerator MoveSignUp()
    {
        anim.SetTrigger("MoveUp");
        yield return new WaitForSeconds(1);
        anim.ResetTrigger("MoveUp");
    }
    
    public IEnumerator PlayHitAnim()
    {
        
        anim.SetTrigger("hit");

        yield return new WaitForSeconds(signNumber+1);
        
        anim.ResetTrigger("hit");
        anim.SetTrigger("MoveDown");

        yield return new WaitForSeconds(1);
        
        anim.ResetTrigger("MoveDown");
        anim.SetTrigger("getUp");
        
        yield return new WaitForSeconds(1);
        
        anim.ResetTrigger("getUp");
    }
}
