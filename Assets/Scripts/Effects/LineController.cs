using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame

    public void DrawLine(Vector3 gunTip, Vector3 hitPoint)
    {
        Vector3[] linePoints = new Vector3[] { hitPoint, gunTip };
        lineRenderer.SetPositions(linePoints);
    }
}
