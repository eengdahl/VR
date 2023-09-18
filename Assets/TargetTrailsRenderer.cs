using System.Collections.Generic;
using UnityEngine;

public class TargetTrailsRenderer : MonoBehaviour
{
    [SerializeField] private GameObject trailRendererPrefab;
    private List<GameObject> activeTargets = new();
    private LineRenderer lineRenderer;
    private List<GameObject> activeLines = new ();
    
    private void ShowLines()
    {
        foreach (GameObject target in activeTargets)
        {
            //Renders each line when the targets are placed
            Vector3 targetPosition = new Vector3(target.transform.position.x, 0.4f, target.transform.position.z);
            GameObject newLine = Instantiate(trailRendererPrefab, transform.position, Quaternion.identity);
            LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, targetPosition * 0.2f);
            lineRenderer.SetPosition(1, targetPosition * 0.25f);
            lineRenderer.SetPosition(2, targetPosition * 0.4f);
            lineRenderer.SetPosition(3, targetPosition * 0.6f);
            lineRenderer.SetPosition(4, targetPosition * 0.75f);
            activeLines.Add(newLine);
        }
    }

    public void PopulateList(List<GameObject> newList)
    {
        activeTargets = newList;
        activeTargets.AddRange(newList);
        ShowLines();
    }

    public void DePopulateList()
    {
        activeLines.Clear();
        activeTargets.Clear();
    }
}