using UnityEngine;

public class MoveOnClick : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private AttachManager attatchScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        attatchScript = gameObject.GetComponent<AttachManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Start Dragging
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    // Yousab - I added this part, but we should remove it when new movement is added.
                    if (attatchScript != null) {
                        attatchScript.StopFollowingEnemy();
                    }
                    TriangulationManager.Instance.SetDrawing(true);
                    offset = transform.position - hit.point;
                }
            }
        }
        
        // Stop Dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            TriangulationManager.Instance.SetDrawing(false);
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }
        }

    }
}
