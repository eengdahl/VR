using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardedBoss : ShootableBoss
{
    [SerializeField] List<BossGuard> childTargets;
    [SerializeField] int spawnedKid = 0;
    [SerializeField] int deadKids;

    [SerializeField] bool canBeHit = false;

    public override void OnEnable()
    {
        base.OnEnable();
        canBeHit = false;
        spawnedKid = 0;
        anim.SetTrigger("getUp");

        foreach (var item in childTargets)
        {
            if (item == null) break;

            item.boss = this;
            item.gameObject.SetActive(true);
            item.GetComponent<Collider>().enabled = false;
        }

        StartCoroutine(nameof(SpawnChildDelay));
    }

    void SpawnChild()
    {
        if (spawnedKid < childTargets.Count)
            for (int i = 0; i < spawnedKid + 1; i++)
            {
                childTargets[i].ManualSetUpTarget();
                childTargets[i].mover.shouldMove = true;
                childTargets[i].GetComponent<Collider>().enabled = true;
            }

        spawnedKid++;
        deadKids = spawnedKid;
    }

    public void KidDied()
    {
        deadKids--;
        if (deadKids != 0) return;

        if (spawnedKid < childTargets.Count)
            StartCoroutine(nameof(SpawnChildDelay));
        else
            canBeHit = true;
    }

    IEnumerator SpawnChildDelay()
    {
        yield return new WaitForSeconds(1f);

        SpawnChild();
    }

    void DeactivateKids()
    {
        foreach (var item in childTargets)
        {
            if (item == null) break;

            item.gameObject.SetActive(false);
        }
    }

    public override void OnHit()
    {
        if (!canBeHit) return;

        if (currentHealth > 0)
        {
            currentHealth--;
            StartHitFeedback();
            audSource.Play();
        }
        else
        {
            DeactivateKids();
            base.OnHit();
        }
    }

    private void OnDisable()
    {
        DeactivateKids();
    }
}
