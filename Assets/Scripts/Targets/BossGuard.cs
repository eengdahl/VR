using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuard : ShootableTarget
{
    [HideInInspector] public GuardedBoss boss;

    public override void OnHit()
    {
        boss.KidDied();
        base.OnHit();
    }
}
