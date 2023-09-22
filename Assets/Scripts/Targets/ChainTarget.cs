using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTarget : MonoBehaviour
{
    [SerializeField] bool test;

    [SerializeField] List<ChainTargetChild> targets; //the chainTargets targets
    public bool chainReaction = false;
    [SerializeField] float chainTime;
    float chainTimer;

    private void OnEnable()
    {
        targets.Clear();
        foreach (Transform child in transform)
            targets.Add(child.GetComponent<ChainTargetChild>());

        chainTimer = chainTime;
        chainReaction = false;
    }

    public void InitializeChildren()
    {
        foreach (var item in targets)
        {
            item.gameObject.SetActive(true);
            item.ChildGetUp();
            item.chainParent = this;
        }
    }

    public void StartChainReaction()
    {
        chainReaction = true;
    }

    private void StopChainReaction()
    {
        foreach (var item in targets)
            item.ChildShotDown();

        chainTimer = chainTime;
        chainReaction = false;
    }

    private void Update()
    {
        if (test)
        {
            InitializeChildren();
            test = false;
        }

        if (chainReaction)
        {
            chainTimer -= Time.deltaTime;
            if (chainTimer <= 0)
            {
                StopChainReaction();
            }
        }
    }
}
