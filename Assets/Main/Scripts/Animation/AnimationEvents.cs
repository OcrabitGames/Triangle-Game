using UnityEngine;

public class AnimationEvents : MonoBehaviour {
    private Animator anim;
    private GameObject character;
    public bool isFox;
    private FoxAnimation foxAnimation;
    private NPCAnimations _npcAnimations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        anim = GetComponent<Animator>();
        character = transform.root.gameObject;
        if (isFox)
        {
            foxAnimation = character.GetComponent<FoxAnimation>();
        } else {
            _npcAnimations = character.GetComponent<NPCAnimations>();
        }
    }

    public void SwitchFrame(int frame) {
        if (isFox) { foxAnimation.Animate(gameObject.name, frame); } 
        else { _npcAnimations.Animate(gameObject.name, frame); }
    }

    public void EndBlinking() {
        anim.SetBool("Blinking", false);
    }
}
