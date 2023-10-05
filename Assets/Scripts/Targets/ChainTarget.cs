using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootableTarget;

public interface IChainListener
{
    public void ObserveChainDone();
}

public class ChainTarget : MonoBehaviour
{
    [SerializeField] bool test;
    AudioSource aS;
    monsterspawnSound _monsterspawnSound;
   
    public MonsterType monsterTypeBo;

    [Tooltip("The children targets, should be added automatically")]
    [SerializeField] List<ChainTargetChild> targets; //the chainTargets targets
    [Tooltip("Whether the first one has been hit")]
    public bool chainReaction = false;
    [Tooltip("How long after the first one has been hit the rest go down")]
    [SerializeField] float chainTime;
    float chainTimer;

    public bool selfReset;

    [SerializeField] float[] downTime;

    List<IChainListener> listeners = new();

    private void OnEnable()
    {
        _monsterspawnSound = FindAnyObjectByType<monsterspawnSound>();
        aS = GetComponent<AudioSource>();
        targets.Clear();
        foreach (Transform child in transform)
            targets.Add(child.GetComponent<ChainTargetChild>());

        chainTimer = chainTime;
        chainReaction = false;

        if (selfReset)
            StartCoroutine(nameof(WaitForReset));
    }

    public void InitializeChildren()
    {
        var imp = _monsterspawnSound.PlaySpawnSound(this.monsterTypeBo, this.aS);
        aS.PlayOneShot(imp);
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

    public void ForceDownLinks()
    {
        foreach (var item in targets)
            item.ChildForceDown();
    }

    public void StopChainReaction()
    {
        ForceDownLinks();

        chainTimer = chainTime;
        chainReaction = false;

        foreach (var listener in listeners)
        {
            listener.ObserveChainDone();
        }

        if (!selfReset) return;

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
