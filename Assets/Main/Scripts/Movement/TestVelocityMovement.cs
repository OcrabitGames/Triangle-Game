using UnityEngine;

public class TestVelocityMovement : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed in the inspector.
    private Rigidbody _rb;

    // Called before the first frame update.
    void Start()
    {
        // Get the Rigidbody component attached to the GameObject.
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("No Rigidbody found! Please add a Rigidbody component to use velocity-based movement.");
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Get input from the WASD keys or arrow keys.
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys.
        float moveVertical = Input.GetAxis("Vertical");     // W/S or Up/Down arrow keys.

        // Create a movement vector based on input.
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // Set the Rigidbody's velocity based on the movement vector and speed.
        // The y component of the velocity is preserved (important if gravity is used).
        _rb.linearVelocity = movement * speed + new Vector3(0f, _rb.linearVelocity.y, 0f);
    }
}
