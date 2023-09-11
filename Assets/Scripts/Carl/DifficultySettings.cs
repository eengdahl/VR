using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Difficulty Settings/Difficulty")]
public class DifficultySettings : ScriptableObject
{
    public int totalTargets;
    public int movingTargets;
}