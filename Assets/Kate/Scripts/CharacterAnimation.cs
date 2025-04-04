using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    public GameObject head, body, tail;
    public GameObject dustVFX;
    public string type;

    private GameObject cam;
    private GameObject textureManager;
    private Texture2D[] runCycle;
    private float dustTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Initialize variables
        cam = Camera.main.gameObject;
        textureManager = GameObject.FindGameObjectWithTag("Texture Manager");
        runCycle = new Texture2D[9];
        dustTimer = 0f;

        // Set the type of character and get necessary textures
        if (type == "Crow") {
            head.GetComponent<Animator>().SetInteger("Type", 1);
            body.GetComponent<Animator>().SetInteger("Type", 1);
            tail.GetComponent<Animator>().SetInteger("Type", 1);
            runCycle = textureManager.GetComponent<TextureManager>().crowRunCycle;
        }
        else if (type == "Fox") {
            head.GetComponent<Animator>().SetInteger("Type", 2);
            body.GetComponent<Animator>().SetInteger("Type", 2);
            tail.GetComponent<Animator>().SetInteger("Type", 2);
            runCycle = textureManager.GetComponent<TextureManager>().foxRunCycle;
        }
    }

    // Update is called once per frame
    void Update() {
        // Animate running
        if (GetComponent<PlayerMovement>().xVel != 0f || GetComponent<PlayerMovement>().zVel != 0f) {
            head.GetComponent<Animator>().SetBool("Idle", false);
            body.GetComponent<Animator>().SetBool("Idle", false);
            tail.GetComponent<Animator>().SetBool("Idle", false);
        }
        else {
            dustTimer = 0f;
            head.GetComponent<Animator>().SetBool("Idle", true);
            body.GetComponent<Animator>().SetBool("Idle", true);
            tail.GetComponent<Animator>().SetBool("Idle", true);
        }

        if (GetComponent<PlayerMovement>().xVel < 0f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (GetComponent<PlayerMovement>().xVel > 0f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // Dust VFX
        dustTimer -= Time.deltaTime;
        if (dustTimer <= 0f && (GetComponent<PlayerMovement>().xVel != 0f || GetComponent<PlayerMovement>().zVel != 0f)) {
            GameObject dust = Instantiate(dustVFX, new Vector3(transform.position.x, 0.78f, transform.position.z), Quaternion.identity);
            dust.GetComponent<VFXBehavior>().parent = gameObject;
            dustTimer = 0.25f;
        }
    }

    // Run cycle
    public void AnimateRunCycle(string runType, int runFrame) {
        if (runType == "Head") {
            head.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", runCycle[runFrame]);
        }
        else if (runType == "Body") {
            body.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", runCycle[runFrame]);
        }
        else if (runType == "Tail") {
            tail.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", runCycle[runFrame]);
        }
    }
}
