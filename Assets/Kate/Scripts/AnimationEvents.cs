using UnityEngine;

public class AnimationEvents : MonoBehaviour {
    private Animator anim;
    private GameObject character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        character = transform.root.gameObject;
    }

    public void SwitchFrame(int frame) {
        character.GetComponent<CharacterAnimation>().Animate(gameObject.name, frame);
    }

    public void EndBlinking() {
        anim.SetBool("Blinking", false);
    }
}
