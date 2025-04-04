using UnityEngine;

public class FaceCamera : MonoBehaviour {
    private GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
    }
}
