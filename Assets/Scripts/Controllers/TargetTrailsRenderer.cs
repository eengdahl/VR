using System.Collections.Generic;
using UnityEngine;

public class TargetTrailsRenderer : MonoBehaviour
{
    [SerializeField] private GameObject standardTrailRendererPrefab;
    [SerializeField] private GameObject vampireTrail;
    [SerializeField] private float amplitude = 1.0f;
    [SerializeField] private int waveCount = 5;
    private List<GameObject> activeTargets = new();
    private LineRenderer lineRenderer;
    public List<GameObject> activeLines = new();

    // private void SetUpLines()
    // {
    //     foreach (var target in activeTargets)
    //     {
    //         Vector3 startPoisiton = Vector3.Lerp(transform.position, target.transform.position, .15f);
    //         Vector3 endPosition = Vector3.Lerp(transform.position, target.transform.position, .8f);
    //         target.GetComponent<ShootableTarget>().LineSetUp(startPoisiton, endPosition);
    //     }
    // }

    public void ShowLine(GameObject target, ShootableTarget.MonsterType type)
    {
        GameObject trail;
        waveCount += Random.Range(-1, 1);
        amplitude += Random.Range(-.015f, .015f);

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

        Vector3 startPoisiton = Vector3.Lerp(transform.position, target.transform.position, .15f);
        Vector3 endPosition = Vector3.Lerp(transform.position, target.transform.position, .8f);

        //Comment the below line to make trails "fly" to target instead of snaking along the ground
        endPosition = new Vector3(endPosition.x, .4f, endPosition.z);
        GameObject newLine = Instantiate(trail, startPoisiton, Quaternion.identity);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        int maxIndex = lineRenderer.positionCount;

        Vector3[] points = new Vector3[maxIndex];

        for (int i = 0; i < maxIndex; i++)
        {
            float t = i / (float)(maxIndex - 1);
            Vector3 linePoint = Vector3.Lerp(startPoisiton, endPosition, t);

            float offset = 0f;

            for (int waveIndex = 0; waveIndex < waveCount; waveIndex++)
            {
                float waveOffset = Mathf.Sin(t * Mathf.PI * waveCount * waveIndex * Mathf.PI) * amplitude;

                offset += waveOffset;
            }

            if (i % 2 != 0)
                linePoint += transform.right * offset;
            else
                linePoint += -transform.right * offset;
            //Comment out the below line to make lines "fly"
            linePoint.y = 0.4f;
            points[i] = linePoint;
        }

        lineRenderer.SetPositions(points);
        activeLines.Add(newLine);
        target.GetComponent<ShootableTarget>().line = newLine;
    }

    private void SetupLines()
    {
        // GameObject trail;
        // waveCount += Random.Range(-1, 1);
        // amplitude += Random.Range(-.015f, .015f);
        foreach (GameObject target in activeTargets)
        {
            target.GetComponent<ShootableTarget>().trailRenderer = this;
        }

        // foreach (GameObject target in activeTargets)
        // {
        //     //Renders each line when the targets are placed
        //     ShootableTarget.MonsterType type = target.GetComponentInParent<ShootableTarget>().monsterType;
        //
        //     //Decides look of the trail (just color for now)
        //     switch (type)
        //     {
        //         case ShootableTarget.MonsterType.dracula:
        //             trail = vampireTrail;
        //             break;
        //         case ShootableTarget.MonsterType.skeleton:
        //             trail = standardTrailRendererPrefab;
        //             break;
        //         case ShootableTarget.MonsterType.zombie:
        //             trail = standardTrailRendererPrefab;
        //             break;
        //         case ShootableTarget.MonsterType.boss:
        //             trail = standardTrailRendererPrefab;
        //             break;
        //         default:
        //             trail = standardTrailRendererPrefab;
        //             break;
        //     }
        //
        //     Vector3 startPoisiton = Vector3.Lerp(transform.position, target.transform.position, .15f);
        //     Vector3 endPosition = Vector3.Lerp(transform.position, target.transform.position, .8f);
        //     
        //     //Comment the below line to make trails "fly" to target instead of snaking along the ground
        //     endPosition = new Vector3(endPosition.x, .4f, endPosition.z);
        //     GameObject newLine = Instantiate(trail, startPoisiton, Quaternion.identity);
        //     LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        //     int maxIndex = lineRenderer.positionCount;
        //
        //     Vector3[] points = new Vector3[maxIndex];
        //
        //     for (int i = 0; i < maxIndex; i++)
        //     {
        //         float t = i / (float)(maxIndex - 1);
        //         Vector3 linePoint = Vector3.Lerp(startPoisiton, endPosition, t);
        //
        //         float offset = 0f;
        //
        //         for (int waveIndex = 0; waveIndex < waveCount; waveIndex++)
        //         {
        //             float waveOffset = Mathf.Sin(t * Mathf.PI * waveCount * waveIndex * Mathf.PI) * amplitude;
        //
        //             offset += waveOffset;
        //         }
        //
        //         if (i % 2 != 0)
        //             linePoint += transform.right * offset;
        //         else
        //             linePoint += -transform.right * offset;
        //         //Comment out the below line to make lines "fly"
        //         linePoint.y = 0.4f;
        //         points[i] = linePoint;
        //     }
        //     lineRenderer.SetPositions(points);
        //     activeLines.Add(newLine);
        //
        // }
    }

    public void PopulateList(List<GameObject> newList)
    {
        //Populates list with the currently active targets
        //Called from TargetPlacer
        activeTargets = newList;
        SetupLines();
    }

    public void DePopulateList()
    {
        //Depopulates list and removes the lines when targets are removed
        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }

        foreach (GameObject target in activeTargets)
        {
            target.GetComponent<ShootableTarget>().line = null;
        }

        activeTargets.Clear();
        activeLines.Clear();
    }

    public void RemoveLine(GameObject lineToKill)
    {
        int i = activeLines.IndexOf(lineToKill);
        Destroy(activeLines[i]);
        if (activeLines[i] != null)
            activeLines.Remove(lineToKill);
    }
}