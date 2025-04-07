using UnityEngine;

public class LineCube : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;

    public bool isActive;
    public bool isRotating = true;
    public float rotationSpeed = 30f;

    // Initialize the cube between two points
    public void Initialize(Vector3 start, Vector3 end, bool first=false)
    {
        gameObject.SetActive(true);
        pointA = start;
        pointB = end;
        isActive = true;
        isRotating = true;
        
        if (first) return;
        UpdateTransform();
    }

    // Redefine endpoints and update line
    public void UpdateEndpoints(Vector3 newStart, Vector3 newEnd)
    {
        pointA = newStart;
        pointB = newEnd;
        UpdateTransform();
    }

    // Deactivate this cube-line
    public void Deactivate()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    // Updates position, rotation, and scale to match endpoints
    private void UpdateTransform()
    {
        Vector3 direction = pointB - pointA;
        Vector3 midPoint = (pointA + pointB) / 2f;

        transform.position = midPoint;

        // Face the direction from pointA to pointB
        transform.rotation = Quaternion.LookRotation(direction);
        
        // Rotate to align thickness on Z-axis, then apply Z-axis rotation later
        transform.localScale = new Vector3(0.1f, 0.1f, direction.magnitude);
    }

    void Update()
    {
        if (!isActive) return;

        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}