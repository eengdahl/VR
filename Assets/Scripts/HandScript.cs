using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandScript : MonoBehaviour
{

    private GameObject hand_L;
    private GameObject hand_R;
    [SerializeField] private Animator animator;
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float speed = 8f;

    private float gripTarget = 0f;
    private float currentGrip = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gripTarget = controller.selectAction.action.ReadValue<float>();

        if (currentGrip != gripTarget)
        {
            currentGrip = Mathf.MoveTowards(currentGrip, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("Grip", currentGrip);
        }
    }
}
