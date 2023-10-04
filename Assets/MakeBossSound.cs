using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootableTarget;

public class MakeBossSound : MonoBehaviour
{
    public MonsterType monsterType;
    private monsterspawnSound spawnSound;
    private AudioSource audSource;

    private void Awake()
    {
        audSource = GetComponent<AudioSource>();

        spawnSound = FindAnyObjectByType<monsterspawnSound>();
        PlayAwakeSound();
    }
    void PlayAwakeSound()
    {
        var temp = spawnSound.PlaySpawnSound(this.monsterType, this.audSource);
        audSource.PlayOneShot(temp);
    }
}
