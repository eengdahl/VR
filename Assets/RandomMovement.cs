using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{

    public List<Transform> patrolPositions;
    Transform firstPosition;
    public Transform player;
    float speed = 1.5f;
    public int index;

    private void Start()
    {
        firstPosition = this.gameObject.transform;
        index = Random.Range(0, patrolPositions.Count - 1);

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(this.transform.position, patrolPositions[index].position) < 0.01f)
        {
            index ++;
            LookAtPlayer();
            if (index >= patrolPositions.Count - 1)
            {
                index = 0;
            }
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, patrolPositions[index].position, speed * Time.deltaTime);
    }
    void LookAtPlayer()
    {

        transform.LookAt(player);
    }

}
