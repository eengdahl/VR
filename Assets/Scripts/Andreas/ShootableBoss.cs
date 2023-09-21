using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableBoss : ShootableTarget
{
    [SerializeField] int maxHealth = 4;
    public int currentHealth;

    private new void OnEnable()
    {
        currentHealth = maxHealth;
        base.OnEnable();
    }

    public override void OnHit()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            mover.StartHitFeedback();
            audSource.Play();
        }
        else
            base.OnHit();
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);
    }
}
