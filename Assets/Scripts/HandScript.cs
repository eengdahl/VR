using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandScript : MonoBehaviour
{

    private GameObject hand_L;
    private GameObject hand_R;
    private Animator animator;
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float speed = 0f;

    private float gripTarget = 0f;
    private float curGrip = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gripTarget = controller.selectAction.action.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curGrip != gripTarget)
        {
            curGrip = Mathf.MoveTowards(curGrip, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("Grip", curGrip);
        }
    }
}
