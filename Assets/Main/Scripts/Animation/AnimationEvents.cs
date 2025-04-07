using UnityEngine;

public class AnimationEvents : MonoBehaviour {
    private Animator anim;
    private GameObject character;
    public bool isFox;
    private FoxAnimation foxAnimation;
    private OwlAnimation owlAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        anim = GetComponent<Animator>();
        character = transform.root.gameObject;
        if (isFox) { foxAnimation = character.GetComponent<FoxAnimation>(); }
        else { owlAnimation = character.GetComponent<OwlAnimation>(); }
    }

    public void SwitchFrame(int frame) {
        if (isFox) { foxAnimation.Animate(gameObject.name, frame); } 
        else { owlAnimation.Animate(gameObject.name, frame); }
    }

    public void EndBlinking() {
        anim.SetBool("Blinking", false);
    }
}
