using UnityEngine;

public class VFXBehavior : MonoBehaviour {
    public GameObject parent;

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Vector3 direction = parent.GetComponent<Rigidbody>().linearVelocity.normalized;
        targetPosition = transform.position + -direction;
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }

    public void DestroyParticle() {
        Destroy(gameObject);
    }
}
