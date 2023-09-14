using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiverScript : MonoBehaviour
{

    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform spawnPoint;
    

    public void ArrowTaken()
    {
        Instantiate(ammoPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
