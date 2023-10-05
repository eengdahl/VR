using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterspawnSound : MonoBehaviour
{

    public AudioClip skeletonClip;
    public AudioClip draculaClip;
    public AudioClip zombieClip;
    public AudioClip bossClip;
    public AudioClip werewolfClip;

    public AudioClip PlaySpawnSound(ShootableTarget.MonsterType monster, AudioSource ass)
    {
        ass.pitch = Random.Range(0.8f, 1.2f);
        if (monster == ShootableTarget.MonsterType.skeleton) return skeletonClip;
        if (monster == ShootableTarget.MonsterType.dracula) return draculaClip;
        if (monster == ShootableTarget.MonsterType.zombie) 
        {
            ass.volume = 0.4f;
                
            return zombieClip;
        } 

        if (monster == ShootableTarget.MonsterType.boss) return bossClip;
        if (monster == ShootableTarget.MonsterType.werewolf) return werewolfClip;
        else { return skeletonClip; }

    }
}
