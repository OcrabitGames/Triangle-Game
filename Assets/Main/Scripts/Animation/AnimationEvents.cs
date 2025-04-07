using UnityEngine;

public class AnimationEvents : MonoBehaviour {
    private Animator anim;
    private GameObject character;
    public bool isFox;
    public string parentName = "Owl";
    private FoxAnimation foxAnimation;
    private NPCAnimation npcAnimations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        anim = GetComponent<Animator>();
        character = GetOwlParent(gameObject);
        if (isFox)
        {
            foxAnimation = character.GetComponent<FoxAnimation>();
        } else {
            npcAnimations = character.GetComponent<NPCAnimation>();
        }
    }

    GameObject GetOwlParent(GameObject obj)
    {
        var parent = obj.transform.parent;
        if (parent.name.Contains(parentName)) { return parent.gameObject; } 
        else { return GetOwlParent(parent.gameObject); }
    }

    public void SwitchFrame(int frame) {
        if (isFox) { foxAnimation.Animate(gameObject.name, frame); }
        else { npcAnimations.Animate(gameObject.name, frame); }
    }

    public void EndBlinking() {
        anim.SetBool("Blinking", false);
    }
}
