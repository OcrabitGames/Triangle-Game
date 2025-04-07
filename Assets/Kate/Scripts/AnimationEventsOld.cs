using UnityEngine;

public class AnimationEventsOld : MonoBehaviour {
    private Animator anim;
    private GameObject character;
    private CharacterAnimation characterAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        anim = GetComponent<Animator>();
        character = transform.root.gameObject;
        characterAnimation =  character.GetComponent<CharacterAnimation>();
    }

    public void SwitchFrame(int frame) {
        characterAnimation.Animate(gameObject.name, frame);
    }

    public void EndBlinking() {
        anim.SetBool("Blinking", false);
    }
}
