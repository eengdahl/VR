using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTargetChild : ShootableTarget
{
    [HideInInspector] public ChainTarget chainParent;

    private new void OnEnable()
    {
        anim.CrossFade("TargetDownState", 0, 0);
    }

    public override void OnHit()
    {
        audSource.Play();
        ChildShotDown();
        StartHitFeedback();
        chainParent.StartChainReaction();

        //base.OnHit();
    }

    public void ChildGetUp()
    {
        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
    }

    public void ChildShotDown()
    {
        anim.SetTrigger("hit");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
