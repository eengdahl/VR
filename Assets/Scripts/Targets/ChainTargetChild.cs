using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTargetChild : ShootableTarget
{
    [HideInInspector] public ChainTarget chainParent;

    Collider col;
    bool hit;
    monsterspawnSound _monsterspawnSound;
    public MonsterType monsterTypeBo;
    public AudioSource aS;
    private new void OnEnable()
    {
    
        anim.CrossFade("TargetDownState", 0, 0);
        col = GetComponent<Collider>();
        col.enabled = false;
        hit = true;

    }

    public override void OnHit()
    {
        audSource.Play();
        ChildShotDown();
        StartHitFeedback();
        chainParent.StartChainReaction();

        hit = true;
        //base.OnHit();
    }

    public void ChildGetUp()
    {
        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
        col.enabled = true;
        hit = false;

    }

    public void ChildShotDown()
    {
        anim.ResetTrigger("getUp");
        anim.SetTrigger("hit");
        col.enabled = false;
    }

    public void ChildForceDown()
    {
        anim.ResetTrigger("getUp");
        anim.ResetTrigger("hit");

        if (hit)
            anim.CrossFade("TargetDownState", 0);
        else
            anim.CrossFade("TargetShotdown", 0);
    }
}
