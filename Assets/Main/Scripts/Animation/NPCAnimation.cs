using UnityEngine;

public class NPCAnimation : MonoBehaviour {
    // Cache PropertyId
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
    public GameObject head, eyes, body, tail;
    public GameObject dustVFX;
    public string type;
    
    // Reference Vars
    private TextureManager _textureManager;
    private NPCMovement _npcMovement;
    
    // Animator Vars
    private Animator _headAnimator;
    private Animator _eyesAnimator;
    private Animator _bodyAnimator;
    private Animator _bodyOutlineAnimator;
    private Animator _tailAnimator;
    
    // Mesh Renderer Vars
    private MeshRenderer _bodyMesh;
    private MeshRenderer _bodyOutlineMesh;
    private MeshRenderer _eyesMesh;
    
    [SerializeField] private Texture2D[] runCycle, outlineRunCycle, blinkCycle;
    private float dustTimer, blinkTimer, blinkTimerThreshold;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Initialize variables
        _textureManager = TextureManager.Instance;
        _npcMovement = GetComponent<NPCMovement>();
        
        // Initialize Animator Vars
        _headAnimator = head.GetComponent<Animator>();
        _eyesAnimator = eyes.GetComponent<Animator>();
        _tailAnimator = tail.GetComponent<Animator>();
        _bodyAnimator = body.GetComponent<Animator>(); 
        _bodyOutlineAnimator = body.transform.GetChild(0).GetComponent<Animator>(); 
        
        // Initialize Mesh Renderers
        _bodyMesh = body.GetComponent<MeshRenderer>();
        _bodyOutlineMesh = body.transform.GetChild(0).GetComponent<MeshRenderer>();
        _eyesMesh = eyes.GetComponent<MeshRenderer>();
        
        runCycle = new Texture2D[9];
        outlineRunCycle = new Texture2D[9];
        blinkCycle = new Texture2D[5];
        dustTimer = 0f;
        blinkTimer = 0f;
        blinkTimerThreshold = Random.Range(1f, 3f);

        // Set the type of character and get necessary textures
        if (type == "Owl") {
            runCycle = _textureManager.owlRunCycle;
            blinkCycle = _textureManager.owlBlinkCycle;
        }

        // Set character outline
        outlineRunCycle = _textureManager.outlineRunCycle;
        _bodyOutlineAnimator.SetBool("Outline", true);
    }

    // Update is called once per frame
    void Update() {
        // Animate running
        if (_npcMovement.xVel != 0f || _npcMovement.zVel != 0f) {
            _headAnimator.SetBool("Idle", false);
            _bodyAnimator.SetBool("Idle", false);
            _tailAnimator.SetBool("Idle", false);
            _bodyOutlineAnimator.SetBool("Idle", false);
        }
        else {
            dustTimer = 0f;
            _headAnimator.SetBool("Idle", true);
            _bodyAnimator.SetBool("Idle", true);
            _tailAnimator.SetBool("Idle", true);
            _bodyOutlineAnimator.SetBool("Idle", true);
        }

        if (_npcMovement.xVel < 0f) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (_npcMovement.xVel > 0f) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Animate blinking
        if (!_eyesAnimator.GetBool("Blinking")) {
            blinkTimer += Time.deltaTime;
        }
        
        if (blinkTimer >= blinkTimerThreshold) {
            blinkTimerThreshold = Random.Range(1f, 3f);
            blinkTimer = 0f;
            _eyesAnimator.SetBool("Blinking", true);
        }

        // Dust VFX
        dustTimer -= Time.deltaTime;
        if (dustTimer <= 0f && (_npcMovement.xVel != 0f || _npcMovement.zVel != 0f)) {
            GameObject dust = Instantiate(dustVFX, new Vector3(transform.position.x, 0.78f, transform.position.z), Quaternion.identity);
            dust.GetComponent<VFXBehavior>().parent = gameObject;
            dustTimer = 0.25f;
        }
    }

    // Run cycle
    public void Animate(string spriteType, int spriteFrame) {
        if (spriteType == "Body") {
            _bodyMesh.materials[0].SetTexture(BaseMap, runCycle[spriteFrame]);
        }
        else if (spriteType == "Outline") {
            _bodyOutlineMesh.materials[0].SetTexture(BaseMap, outlineRunCycle[spriteFrame]);
        }
        else if (spriteType == "Eyes") {
            _eyesMesh.materials[0].SetTexture(BaseMap, blinkCycle[spriteFrame]);
        }
    }
}