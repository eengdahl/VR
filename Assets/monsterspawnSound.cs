using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterspawnSound : MonoBehaviour
{

    public AudioClip skeletonClip;
    public AudioClip draculaClip;
    public AudioClip zombieClip;
    public AudioClip bossClip;
    public AudioClip PlaySpawnSound(ShootableTarget.MonsterType monster)
    {
        if (monster == ShootableTarget.MonsterType.skeleton) return skeletonClip;
        if (monster == ShootableTarget.MonsterType.dracula) return draculaClip;
        if (monster == ShootableTarget.MonsterType.zombie) return zombieClip;
        if (monster == ShootableTarget.MonsterType.boss) return bossClip;
        else { return skeletonClip; }
    }
}
