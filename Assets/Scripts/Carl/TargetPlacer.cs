using System;
using System.Collections;
using System.Collections.Generic;
using Carl;
using UnityEngine;

public class TargetPlacer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    [SerializeField] private List<Transform> easyTargetPoints = new();
    [SerializeField] private List<Transform> normalTargetPoints = new();
    [SerializeField] private List<Transform> hardTargetPoints = new();
    
    [SerializeField] private ScriptableObject easySettings;
    [SerializeField] private ScriptableObject normalSettings;
    [SerializeField] private ScriptableObject hardSettings;

    public DifficultyController difficultyController;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PlaceTargets(Difficulty.Easy);
        }
    }

    public void PlaceTargets(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                foreach (var transform in easyTargetPoints)
                {
                    GameObject newTarget = Instantiate(target, transform.position, transform.rotation);
                    newTarget.GetComponent<ShootableMoving>().moveType = ShootableMoving.MoveType.FlipUp;
                    difficultyController.activeTargets.Add(newTarget);
                }
                break;
            case Difficulty.Normal:
                foreach (var transform in normalTargetPoints)
                {
                    GameObject newTarget = Instantiate(target, transform.position, transform.rotation);
                    difficultyController.activeTargets.Add(newTarget);
                }
                break;
            case Difficulty.Hard:
                foreach (var transform in normalTargetPoints)
                {
                    GameObject newTarget = Instantiate(target, transform.position, transform.rotation);
                    difficultyController.activeTargets.Add(newTarget);
                }
                break;
        }
    }
}
