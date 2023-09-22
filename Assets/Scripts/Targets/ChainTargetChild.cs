using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTargetChild : ShootableTarget
{
    [HideInInspector] public ChainTarget chainParent;

    Collider col;

    private new void OnEnable()
    {
        anim.CrossFade("TargetDownState", 0, 0);
        col = GetComponent<Collider>();
        col.enabled = false;
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
        col.enabled = true;
    }

    public void ChildShotDown()
    {
        anim.ResetTrigger("getUp");
        anim.SetTrigger("hit");
        col.enabled = false;
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
