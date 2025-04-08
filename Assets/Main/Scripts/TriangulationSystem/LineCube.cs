using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCube : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    
    private LineRenderer lineRenderer;

    public bool isActive;
    public Material dashedLineMaterial;

    public void Initialize(Vector3 start, Vector3 end, bool first = false)
    {
        pointA = start;
        pointB = end;
        isActive = true;

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.material = dashedLineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        if (first) return;

        UpdateLine();
    }

    public void UpdateEndpoints(Vector3 newStart, Vector3 newEnd)
    {
        pointA = newStart;
        pointB = newEnd;
        UpdateLine();
    }

    public void Deactivate()
    {
        isActive = false;
        lineRenderer.enabled = false;
    }

    private void UpdateLine()
    {
        if (!isActive) return;

        lineRenderer.enabled = true;
        
        Vector3 groundA = new Vector3(pointA.x, 0f, pointA.z);
        Vector3 groundB = new Vector3(pointB.x, 0f, pointB.z);

        lineRenderer.SetPosition(0, groundA);
        lineRenderer.SetPosition(1, groundB);
    }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
}