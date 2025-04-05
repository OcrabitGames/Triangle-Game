using UnityEngine;

public class TriangularPlane : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;

    private float _timeRequired;
    private float _curTimeRequired;
    private bool _countdownActive;
    
    public float thickness = 0.05f;
    
    private TriangulationManager _triangulationManagerReference;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    public void Initialize(float timeRequired, TriangulationManager reference)
    {
        _timeRequired = timeRequired;
        _curTimeRequired = timeRequired;
        _triangulationManagerReference = reference;
    }

    public void UpdateTriangle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        BuildGeometry(pointA, pointB, pointC);
        
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
    
    public void Deactivate()
    {
        mesh.Clear();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_curTimeRequired <= 0f)
            {
                Destroy(other.gameObject);
                print("Captured Enemy!");
                // Eventually maybe do something with _triangulationManagerReference
            } else {
                _curTimeRequired -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _curTimeRequired = _timeRequired;
        }
    }
    
    private void BuildGeometry(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        // Get the norm
        Vector3 normal = Vector3.Cross(pointB - pointA, pointC - pointA).normalized;
        // Calc half the thickness to have it on both sides
        Vector3 halfOffset = normal * (thickness * 0.5f);

        // Create vertices
        Vector3 A1 = pointA - halfOffset;
        Vector3 B1 = pointB - halfOffset;
        Vector3 C1 = pointC - halfOffset;

        Vector3 A2 = pointA + halfOffset;
        Vector3 B2 = pointB + halfOffset;
        Vector3 C2 = pointC + halfOffset;

        // Merge vertices
        Vector3[] vertices = new Vector3[6]
        {
            // Bottom
            A1, B1, C1,
            // Top
            A2, B2, C2
        };

        // Defines the orientation of the faces of the triangle
        // - Bottom face (winding order for correct normal direction)
        // - Top face (winding order reversed so normals face outward)
        // - Side faces to connect the top and bottom faces
        int[] triangles = new int[]
        {
            // Bottom face
            0, 1, 2,

            // Top face
            3, 5, 4,

            // Side face 1
            0, 1, 4,
            0, 4, 3,

            // Side face 2
            1, 2, 5,
            1, 5, 4,

            // Side face 3
            2, 0, 3,
            2, 3, 5
        };
        
        // The section above, the triangle vertices merge, is due to the blessed work of the great GPT

        // Clear and update
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

}