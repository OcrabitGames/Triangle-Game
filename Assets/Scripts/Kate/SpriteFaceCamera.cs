using UnityEngine;

public class SpriteFaceCamera : MonoBehaviour {
    public GameObject head, body;
    private GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update() {
        head.transform.LookAt(cam.transform);
        head.transform.rotation = Quaternion.Euler(0f, head.transform.rotation.y, 0f);

        body.transform.LookAt(cam.transform);
        body.transform.rotation = Quaternion.Euler(0f, body.transform.rotation.y, 0f);
    }
}
