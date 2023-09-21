using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ShootableTarget : MonoBehaviour
{
    //----The Update for this no longer exists!
    //[Tooltip("For Testing Purposes! Click to test animations")]
    //[SerializeField] bool hit; //for testing purposes

    [Tooltip("How long to wait before getting up again after being shot down")]
    public float minDownTime = 2f;
    public float maxDownTime = 5f;
    private float downTime;

    [Tooltip("Check true to keep target on start. For testing purposes!")]
    [SerializeField] bool keepOnStart = false;

    public enum MonsterType { skeleton, dracula, zombie, boss }

    [SerializeField] public MonsterType monsterType;
    monsterspawnSound spawnSound;

    //components
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audSource;
    ScoreController score;
    Collider _collider;
    [HideInInspector] public ShootableMoving mover;

    private void Start()
    {
        spawnSound = FindAnyObjectByType<monsterspawnSound>();
        audSource = GetComponent<AudioSource>();
        _collider = GetComponent<CapsuleCollider>();
        mover = GetComponent<ShootableMoving>();

        score = GameObject.FindWithTag("Score").GetComponent<ScoreController>();
        if (score == null) Debug.LogError("Cannot find ScoreController, is there one in the scene tagged 'Score'?");
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        //anim.CrossFade("TargetShotdown", 0, 0);
    }

    public void OnEnable()
    {
        downTime = Random.Range(minDownTime, maxDownTime);
        //anim.CrossFade("UpState", 0, 0);
    }

    public virtual void OnHit()
    {
        audSource.Play();
        StartCoroutine(nameof(PlayHitAnim));
        mover.StartHitFeedback();
        mover.ManualChangeState(ShootableMoving.CurrentState.Idle);
    }

    public IEnumerator PlayHitAnim()
    {
        anim.SetTrigger("hit");
        _collider.enabled = false;
        //Debug.LogFormat("{0} has a downtime of:  {1}", gameObject.name, downTime);

        yield return new WaitForSeconds(downTime);

        downTime = Random.Range(minDownTime, maxDownTime);
        anim.ResetTrigger("hit");
        anim.SetTrigger("getUp");
        PlayAwakeSound();
        _collider.enabled = true;
        mover.ManualChangeState(ShootableMoving.CurrentState.Moving);
    }

    void PlayAwakeSound()
    {
        var temp = spawnSound.PlaySpawnSound(this.monsterType, this.audSource);
        audSource.PlayOneShot(temp);
    }

}
