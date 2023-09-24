using System.Collections.Generic;
using UnityEngine;

public class TargetTrailsRenderer : MonoBehaviour
{
    [SerializeField] private GameObject standardTrailRendererPrefab;
    [SerializeField] private GameObject vampireTrail;
    private List<GameObject> activeTargets = new();
    private LineRenderer lineRenderer;
    private List<GameObject> activeLines = new ();
    
    private void ShowLines()
    {
        GameObject trail;
        
        foreach (GameObject target in activeTargets)
        {
            //Renders each line when the targets are placed
            ShootableTarget.MonsterType type = target.GetComponentInParent<ShootableTarget>().monsterType;

            switch (type)
            {
                case ShootableTarget.MonsterType.dracula:
                    trail = vampireTrail;
                    break;
                case ShootableTarget.MonsterType.skeleton:
                    trail = standardTrailRendererPrefab;
                    break;
                case ShootableTarget.MonsterType.zombie:
                    trail = standardTrailRendererPrefab;
                    break;
                case ShootableTarget.MonsterType.boss:
                    trail = standardTrailRendererPrefab;
                    break;
                default:
                    trail = standardTrailRendererPrefab;
                    break;
            }

            Vector3 targetPosition = new Vector3(target.transform.position.x, 0.4f, target.transform.position.z);
            GameObject newLine = Instantiate(trail, transform.position, Quaternion.identity);
            LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
            int maxIndex = lineRenderer.positionCount;
            int index = 0;
            Vector3 nextPos = new Vector3(targetPosition.x * 0.2f, targetPosition.y * 0.2f, targetPosition.z * 0.2f);
            lineRenderer.SetPosition(0, nextPos);
            
            while (index <= maxIndex)
            {
                nextPos = new Vector3(nextPos.x * 1.5f, nextPos.y * 1.5f, nextPos.z * .9f + Random.Range(-.5f, .5f));
                lineRenderer.SetPosition(index, nextPos);
                index++;
            }
            
            // lineRenderer.SetPosition(0, new Vector3(targetPosition.x * 0.2f, targetPosition.y * .2f, targetPosition.z * .2f));
            // lineRenderer.SetPosition(1, new Vector3(targetPosition.x * 0.25f, targetPosition.y * .25f, targetPosition.z * .25f + Random.Range(-.8f, 0.8f)));
            // lineRenderer.SetPosition(2, new Vector3(targetPosition.x * 0.4f, targetPosition.y * .4f, targetPosition.z * .4f + Random.Range(-0.8f, 0.8f)));
            // lineRenderer.SetPosition(3, new Vector3(targetPosition.x * 0.6f, targetPosition.y * .6f, targetPosition.z * .6f + Random.Range(-0.8f, 0.8f)));
            // lineRenderer.SetPosition(4, new Vector3(targetPosition.x * 0.75f, targetPosition.y * .75f, targetPosition.z * .75f));
            
            //lineRenderer.SetPosition(1, targetPosition * 0.25f);
            // lineRenderer.SetPosition(2, targetPosition * 0.4f);
            // lineRenderer.SetPosition(3, targetPosition * 0.6f);  
            // lineRenderer.SetPosition(4, targetPosition * 0.75f);
            activeLines.Add(newLine);
            
        }
    }

    public void PopulateList(List<GameObject> newList)
    {
        //Populates list with the currently active targets
        //Called from TargetPlacer
        activeTargets = newList;
        activeTargets.AddRange(newList);
        ShowLines();
    }

    public void DePopulateList()
    {
        //Depopulates list and removes the lines when targets are removed
        activeLines.Clear();
        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }
        activeTargets.Clear();
    }
}