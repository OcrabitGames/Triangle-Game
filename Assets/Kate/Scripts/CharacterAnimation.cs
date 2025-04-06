using UnityEngine;

public class CharacterAnimation : MonoBehaviour {
    public GameObject head, eyes, body, tail;
    public GameObject dustVFX;
    public string type;

    private GameObject textureManager;
    private Texture2D[] runCycle, outlineRunCycle, blinkCycle;
    private float dustTimer, blinkTimer, blinkTimerThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Initialize variables
        textureManager = GameObject.FindGameObjectWithTag("Texture Manager");
        runCycle = new Texture2D[9];
        outlineRunCycle = new Texture2D[9];
        blinkCycle = new Texture2D[5];
        dustTimer = 0f;
        blinkTimer = 0f;
        blinkTimerThreshold = Random.Range(1f, 3f);

        // Set the type of character and get necessary textures
        if (type == "Owl") {
            runCycle = textureManager.GetComponent<TextureManager>().owlRunCycle;
            blinkCycle = textureManager.GetComponent<TextureManager>().owlBlinkCycle;
        }
        else if (type == "Purple Fox") {
            runCycle = textureManager.GetComponent<TextureManager>().purpleFoxRunCycle;
            blinkCycle = textureManager.GetComponent<TextureManager>().foxBlinkCycle;
        }
        else if (type == "Pink Fox") {
            runCycle = textureManager.GetComponent<TextureManager>().pinkFoxRunCycle;
            blinkCycle = textureManager.GetComponent<TextureManager>().foxBlinkCycle;
        }
        else if (type == "Blue Fox") {
            runCycle = textureManager.GetComponent<TextureManager>().blueFoxRunCycle;
            blinkCycle = textureManager.GetComponent<TextureManager>().foxBlinkCycle;
        }

        // Set character outline
        outlineRunCycle = textureManager.GetComponent<TextureManager>().outlineRunCycle;
        body.transform.GetChild(0).GetComponent<Animator>().SetBool("Outline", true);
    }

    // Update is called once per frame
    void Update() {
        // Animate running
        if ((gameObject.CompareTag("Player") && (GetComponent<PlayerMovement>().xVel != 0f || GetComponent<PlayerMovement>().zVel != 0f)) || (!gameObject.CompareTag("Player") && (GetComponent<NPCMovement>().xVel != 0f || GetComponent<NPCMovement>().zVel != 0f))) {
            head.GetComponent<Animator>().SetBool("Idle", false);
            body.GetComponent<Animator>().SetBool("Idle", false);
            tail.GetComponent<Animator>().SetBool("Idle", false);
            body.transform.GetChild(0).GetComponent<Animator>().SetBool("Idle", false);
        }
        else {
            dustTimer = 0f;
            head.GetComponent<Animator>().SetBool("Idle", true);
            body.GetComponent<Animator>().SetBool("Idle", true);
            tail.GetComponent<Animator>().SetBool("Idle", true);
            body.transform.GetChild(0).GetComponent<Animator>().SetBool("Idle", true);
        }

        if ((gameObject.CompareTag("Player") && GetComponent<PlayerMovement>().xVel < 0f) || (!gameObject.CompareTag("Player") && GetComponent<NPCMovement>().xVel < 0f)) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if ((gameObject.CompareTag("Player") && GetComponent<PlayerMovement>().xVel > 0f) || (!gameObject.CompareTag("Player") && GetComponent<NPCMovement>().xVel > 0f)) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Animate blinking
        if (!eyes.GetComponent<Animator>().GetBool("Blinking")) {
            blinkTimer += Time.deltaTime;
        }
        
        if (blinkTimer >= blinkTimerThreshold) {
            blinkTimerThreshold = Random.Range(1f, 3f);
            blinkTimer = 0f;
            eyes.GetComponent<Animator>().SetBool("Blinking", true);
        }

        // Dust VFX
        dustTimer -= Time.deltaTime;
        if ((gameObject.CompareTag("Player") && dustTimer <= 0f && (GetComponent<PlayerMovement>().xVel != 0f || GetComponent<PlayerMovement>().zVel != 0f)) || (!gameObject.CompareTag("Player") && dustTimer <= 0f && (GetComponent<NPCMovement>().xVel != 0f || GetComponent<NPCMovement>().zVel != 0f))) {
            GameObject dust = Instantiate(dustVFX, new Vector3(transform.position.x, 0.78f, transform.position.z), Quaternion.identity);
            dust.GetComponent<VFXBehavior>().parent = gameObject;
            dustTimer = 0.25f;
        }
    }

    // Run cycle
    public void Animate(string spriteType, int spriteFrame) {
        if (spriteType == "Body") {
            body.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", runCycle[spriteFrame]);
        }
        else if (spriteType == "Outline") {
            body.transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", outlineRunCycle[spriteFrame]);
        }
        else if (spriteType == "Eyes") {
            eyes.GetComponent<MeshRenderer>().materials[0].SetTexture("_BaseMap", blinkCycle[spriteFrame]);
        }
    }
}