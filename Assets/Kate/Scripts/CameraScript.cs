using UnityEngine;

public class CameraScript : MonoBehaviour {
    public GameObject target;
    public Vector3 camOffset;
    public float camSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        gameObject.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }

    // Update is called once per frame
    void Update() {
        Move(target.transform.position, camOffset, camSpeed);
    }

    private void Move(Vector3 targetPos, Vector3 offset, float speed) {
        transform.position = Vector3.Lerp(transform.position, targetPos + offset, speed);
    }
}
