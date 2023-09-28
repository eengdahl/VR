using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    private Transform player;
    public InputActionProperty YButton;
    bool lockScript = false;
    public Transform gamePosition;
    public Transform startPosition;
    public float speed;


    void Start()
    {

        //Unlock if we need the player to move
        lockScript = true;
        speed = 0;
        player = this.transform;
        player.position = startPosition.position;
    }
    private void FixedUpdate()
    {
        if (lockScript) return;

        // bool checker = YButton.action.WasPerformedThisFrame();

        //if (checker)
        //{
        //    TeleportToStart();
        //}

        //make slowdown
        //make accelerating in start 

        if (Vector3.Distance(transform.position, startPosition.position) < 1)
        {
            speed += Time.deltaTime * 1.3f;
        }

        player.position = Vector3.MoveTowards(player.position, gamePosition.position, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, gamePosition.position) < 1.5)
        {
            speed -= Time.deltaTime * 2f;
            if (speed < 0.5f)
            {
                speed = 0.5f;
            }
        }


        if (Vector3.Distance(transform.position, gamePosition.position) < 0.1f)
        {
            lockScript = true;
        }


    }



    private void TeleportToStart()
    {
        transform.position = gamePosition.position;
        lockScript = true;
    }

}
