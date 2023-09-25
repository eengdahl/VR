using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBoss : MonoBehaviour, IChainListener
{
    [SerializeField] List<ChainTarget> targets;

    [SerializeField] int targetToActivate;
    [SerializeField] int lastActivatedTarget;

    Animator anim;

    private void OnEnable()
    {
        //play spawn animation, since we've removed ShootableTarget and ShootableMoving
        anim = GetComponentInChildren<Animator>();
        anim.CrossFade("TargetDownState", 0, 0);
        anim.SetTrigger("getUp");

        foreach (var item in targets)
        {
            item.gameObject.SetActive(true);
        }
        ActivateTarget();
    }

    public void ObserveChainDone()
    {
        ActivateTarget();
    }

    void ActivateTarget()
    {
        Debug.Log("Activating target: " + targetToActivate);
        targets[PickTarget()].InitializeChildren();
    }

    int PickTarget()
    {
        targetToActivate = Random.Range(0, targets.Count);

        if (targetToActivate == lastActivatedTarget && targetToActivate < targets.Count - 1)
            targetToActivate++;
        else if (targetToActivate == lastActivatedTarget)
            targetToActivate = 0;

        return targetToActivate;
    }

    private void OnDisable()
    {
        foreach (var item in targets)
        {
            item.gameObject.SetActive(false);
        }
    }
}
