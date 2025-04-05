using UnityEngine;

public class VFXBehavior : MonoBehaviour {
    public GameObject parent;

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        transform.localScale = new Vector3 (Mathf.Abs(parent.transform.localScale.x), parent.transform.localScale.y, parent.transform.localScale.z);

        Vector3 direction = parent.GetComponent<Rigidbody>().linearVelocity.normalized;
        targetPosition = transform.position - direction;
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f * transform.localScale.x);
    }

    public void DestroyParticle() {
        Destroy(gameObject);
    }
}
