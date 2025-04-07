using UnityEngine;

public class TextureManager : MonoBehaviour {
    // an Instance that doesn't get destroyed so this works on all scenes
    public static TextureManager Instance { get; private set; }
    
    public Texture2D[] outlineRunCycle;

    public Texture2D[] purpleFoxRunCycle;
    public Texture2D[] pinkFoxRunCycle;
    public Texture2D[] blueFoxRunCycle;
    public Texture2D[] foxBlinkCycle;

    public Texture2D[] owlRunCycle;
    public Texture2D[] owlBlinkCycle;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
