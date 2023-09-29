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
    public bool lockScript = false;
    public Transform gamePosition;
    public Transform startPosition;
    public float speed;
    public float sin;
    public bool lockMusic;
    float timer;
    public List<Transform> credit = new List<Transform>();

    void Start()
    {
        foreach (var item in credit)
        {
            item.transform.LookAt(Vector3.zero);

        }
        timer = 0;
        //Unlock if we need the player to move
        lockScript = true;
        speed = 0;
        player = this.transform;
        player.position = startPosition.position;
    }
    private void FixedUpdate()
    {
        if (lockScript) return;

        bool TriggerVaulue = YButton.action.IsPressed();
        if (TriggerVaulue)
        {

            timer += Time.deltaTime;
            if (timer > 2)
            {
                TeleportToStart();
            }
        }
        else
        {
            timer = 0;
        }

        if (Vector3.Distance(transform.position, startPosition.position) < 0.8f)
        {
            speed += Time.deltaTime * 1.3f;
            player.position = Vector3.MoveTowards(player.position, gamePosition.position, Time.deltaTime * speed);
            return;
        }


        if (Vector3.Distance(transform.position, gamePosition.position) < 1.5)
        {
            speed -= Time.deltaTime * 2f;
            if (speed < 0.5f)
            {
                speed = 0.5f;
            }
            player.position = Vector3.MoveTowards(player.position, gamePosition.position, Time.deltaTime * speed);
            return;
        }

        sin = Mathf.Sin(1 * Time.time);
        sin = sin * 0.1f;

        float totalSpeed = sin + speed;


        player.position = Vector3.MoveTowards(player.position, gamePosition.position, Time.deltaTime * totalSpeed);

        if (Vector3.Distance(transform.position, gamePosition.position) < 0.1f)
        {
            var aS = GetComponent<AudioSource>();
            aS.Stop();
            AtStart();
            lockScript = true;
        }
    }



    private void TeleportToStart()
    {
        var aS = GetComponent<AudioSource>();
        aS.Stop();
        transform.position = gamePosition.position;
        AtStart();
        lockScript = true;
    }
    public void StartMusic()
    {
        if (lockMusic) return;
        var aS = GetComponent<AudioSource>();
        aS.Play();
        lockMusic = true;
    }

    public void StartWalking()
    {
        lockScript = false;
        StartMusic();
    }
    void AtStart()
    {
        foreach (var item in credit)
        {
            item.transform.LookAt(Vector3.zero, Vector3.right);

        }
    }


}
