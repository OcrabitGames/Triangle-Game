using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private KeyCode left, right, up, down;
    private Rigidbody rb;
    public float xVel, zVel;

    public float movementSpeed;
    public float speedLimit;
    public float friction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;

        rb = GetComponent<Rigidbody>();

        xVel = 0f;
        zVel = 0f;
    }

    // Update is called once per frame
    void Update() {
        // Movement input
        if (Input.GetKey(left) && !Input.GetKey(right)) {
            xVel -= movementSpeed * Time.deltaTime;
            if (xVel < -speedLimit) {
                xVel = -speedLimit;
            }
        }
        else if (Input.GetKey(right) && !Input.GetKey(left)) {
            xVel += movementSpeed * Time.deltaTime;
            if (xVel > speedLimit) {
                xVel = speedLimit;
            }
        }
        else {
            xVel *= friction * (1f - Time.deltaTime);
            if (Mathf.Abs(xVel) <= 0.01f) {
                xVel = 0f;
            }
        }

        if (Input.GetKey(up) && !Input.GetKey(down)) {
            zVel += movementSpeed * Time.deltaTime;
            if (zVel > speedLimit) {
                zVel = speedLimit;
            }
        }
        else if (Input.GetKey(down) && !Input.GetKey(up)) {
            zVel -= movementSpeed * Time.deltaTime;
            if (zVel < -speedLimit) {
                zVel = -speedLimit;
            }
        }
        else {
            zVel *= friction * (1f - Time.deltaTime);
            if (Mathf.Abs(zVel) <= 0.01f) {
                zVel = 0f;
            }
        }
    }

    // Physics calculations
    private void FixedUpdate() {
        rb.MovePosition(new Vector3(rb.position.x + xVel, rb.position.y, rb.position.z + zVel));
    }
}
