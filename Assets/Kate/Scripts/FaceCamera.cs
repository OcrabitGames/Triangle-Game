using UnityEngine;

public class FaceCamera : MonoBehaviour {
    private GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update() {
        Vector3 targetPos = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        transform.rotation = Quaternion.LookRotation(-targetPos + transform.position, Vector3.up);
    }
}
