using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootableTarget : MonoBehaviour
{
    [Tooltip("For Testing Purposes! Click once and then turn false to test animations")]
    [SerializeField] bool hit; //for testing purposes

    [Tooltip("How long to wait before getting up again after being shot down")]
    [SerializeField] float downTime = 2f;

    Animator anim;
    ScoreController score;
    Collider collider;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<CapsuleCollider>();
        score = GameObject.FindWithTag("Score").GetComponent<ScoreController>();
        if (score == null) Debug.LogError("Cannot find ScoreController, is there one in the scene tagged 'Score'?");
    }

    // Update is called once per frame
    void Update()
    {
        //for testing purposes
        if (hit)
        {
            OnHit();
            hit = false;
        }
    }

    public void OnHit()
    {
        //Idk what to do here yet
        StartCoroutine(nameof(PlayHitAnim));
    }

    IEnumerator PlayHitAnim()
    {
        anim.SetTrigger("hit");
        collider.enabled = false;

        yield return new WaitForSeconds(downTime);

        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
        collider.enabled = true;
    }
}
