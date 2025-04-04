using UnityEngine;

public class CameraScript : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        gameObject.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
