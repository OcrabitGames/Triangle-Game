using UnityEngine;

public class LineCubeDebug : MonoBehaviour
{
    private LineCube line;

    void Start()
    {
        line = GetComponent<LineCube>();

        Vector3 start = transform.position;
        Vector3 end = transform.position + new Vector3(2f, 0, 0);  // horizontal line to the right

        line.Initialize(start, end);
    }
}