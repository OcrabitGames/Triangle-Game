using UnityEngine;

public class NPCMovement : MonoBehaviour {
    public float xVel, zVel;
    public float movementSpeed;
    public float speedLimit;
    public float friction;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody>();

        xVel = 0f;
        zVel = 0f;

        movementSpeed *= Mathf.Abs(transform.localScale.x);
        speedLimit *= Mathf.Abs(transform.localScale.x);
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Physics calculations
    private void FixedUpdate() {
        rb.MovePosition(new Vector3(rb.position.x + xVel, rb.position.y, rb.position.z + zVel));
    }
}
