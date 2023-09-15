using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootableTarget : MonoBehaviour
{
    //[Tooltip("For Testing Purposes! Click to test animations")]
    //[SerializeField] bool hit; //for testing purposes

    [Tooltip("How long to wait before getting up again after being shot down")]
    [SerializeField] float downTime = 2f;

    [Tooltip("Check true to keep target on start. For testing purposes!")]
    [SerializeField] bool keepOnStart = false;

    //components
    [HideInInspector] public Animator anim;
    AudioSource aS;
    ScoreController score;
    Collider _collider;
    ShootableMoving mover;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        mover = GetComponent<ShootableMoving>();

        score = GameObject.FindWithTag("Score").GetComponent<ScoreController>();
        if (score == null) Debug.LogError("Cannot find ScoreController, is there one in the scene tagged 'Score'?");

        if (!keepOnStart)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //for testing purposes
        //if (hit)
        //{
        //    OnHit();
        //    hit = false;
        //}
    }

    public void OnHit()
    {
        //print("A Target Has Been hit in Lego City!");
        //Idk what to do here yet
        aS.Play();
        StartCoroutine(nameof(PlayHitAnim));

        mover.ManualChangeState(ShootableMoving.CurrentState.Idle);
    }

    IEnumerator PlayHitAnim()
    {
        //print("Playing Hit Anim");
        anim.SetTrigger("hit");
        _collider.enabled = false;

        yield return new WaitForSeconds(downTime);

        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
        _collider.enabled = true;
        mover.ManualChangeState(ShootableMoving.CurrentState.Moving);
    }
}
