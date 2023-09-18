using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrailsRenderer : MonoBehaviour
{
    private List<GameObject> activeTargets = new();
    private LineRenderer lineRenderer;
    private GameController gameController;
    
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        while (activeTargets.Count != 0 && gameController.currentGameState == GameState.inGame)
        {
            foreach (GameObject target in activeTargets)
            {
                
            }
        }
    }

    public void PopulateList(List<GameObject> newList)
    {
        activeTargets = newList;
    }
}