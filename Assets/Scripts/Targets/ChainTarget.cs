using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChainListener
{
    public void ObserveChainDone();
}

public class ChainTarget : MonoBehaviour
{
    [SerializeField] bool test;

    [Tooltip("The children targets, should be added automatically")]
    [SerializeField] List<ChainTargetChild> targets; //the chainTargets targets
    [Tooltip("Whether the first one has been hit")]
    public bool chainReaction = false;
    [Tooltip("How long after the first one has been hit the rest go down")]
    [SerializeField] float chainTime;
    float chainTimer;

    public bool selfReact;

    [SerializeField] float[] downTime;

    List<IChainListener> listeners = new();

    private void OnEnable()
    {
        targets.Clear();
        foreach (Transform child in transform)
            targets.Add(child.GetComponent<ChainTargetChild>());

        chainTimer = chainTime;
        chainReaction = false;

        if (selfReact)
            StartCoroutine(nameof(WaitForReset));
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
            item.ChildForceDown();

        chainTimer = chainTime;
        chainReaction = false;

        foreach (var listener in listeners)
        {
            listener.ObserveChainDone();
        }

        if (!selfReact) return;

        StartCoroutine(nameof(WaitForReset));
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

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(Random.Range(downTime[0], downTime[1]));

        InitializeChildren();
    }

    public void ChainSubscribe(IChainListener subscriber)
    {
        listeners.Add(subscriber);
    }
}
