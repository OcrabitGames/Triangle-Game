using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float xVel, zVel;
    public float movementSpeed;
    public float speedLimit;
    public float friction;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        xVel = 0f;
        zVel = 0f;

        // Adjust speeds based on the player's scale
        movementSpeed *= Mathf.Abs(transform.localScale.x);
        speedLimit *= Mathf.Abs(transform.localScale.x);
    }

    // This method is now called externally to update movement based on input.
    public void MoveDirection(Vector3 input, float speed = -1f)
    {
        if (speed != -1f) movementSpeed = speed;
        
        // Horizontal movement
        if (input.x < 0)
        {
            xVel -= movementSpeed * Time.deltaTime;
            if (xVel < -speedLimit) xVel = -speedLimit;
        }
        else if (input.x > 0)
        {
            xVel += movementSpeed * Time.deltaTime;
            if (xVel > speedLimit) xVel = speedLimit;
        }
        else
        {
            // Apply friction if no input
            xVel *= friction * (1f - Time.deltaTime);
            if (Mathf.Abs(xVel) <= 0.01f) xVel = 0f;
        }

        // Vertical Movement
        if (input.z > 0)
        {
            zVel += movementSpeed * Time.deltaTime;
            if (zVel > speedLimit) zVel = speedLimit;
        }
        else if (input.z < 0)
        {
            zVel -= movementSpeed * Time.deltaTime;
            if (zVel < -speedLimit) zVel = -speedLimit;
        }
        else
        {
            // Apply friction if no input
            zVel *= friction * (1f - Time.deltaTime);
            if (Mathf.Abs(zVel) <= 0.01f) zVel = 0f;
        }
    }
    
    public void MoveToward(Vector3 targetPosition)
    {
        Vector3 delta = targetPosition - transform.position;
        delta.y = 0f;
        float speedOverride = delta.magnitude;
        MoveDirection(delta.normalized, speedOverride);
    }

    // Physics.
    private void FixedUpdate()
    {
        _rb.MovePosition(new Vector3(_rb.position.x + xVel, _rb.position.y, _rb.position.z + zVel));
    }
}
