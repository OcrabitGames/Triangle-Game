using UnityEngine;

public class MoveOnClick : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private PlayerFollow _followScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        _followScript = gameObject.GetComponent<PlayerFollow>();
        
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
                    // Marco - I updated the connections! Thanks for the heads-up. :)
                    if (_followScript) {
                        _followScript.StopFollowing();
                    }
                    offset = transform.position - hit.point;
                }
            }
        }
        
        // Stop Dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            _followScript.StartFollowing();
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
