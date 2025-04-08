using UnityEngine;

public class SoundFXManager : MonoBehaviour {
    private AudioSource[] sources;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        sources = GetComponents<AudioSource>();
    }

    public void PlaySound(AudioClip sound) {
        sources[0].PlayOneShot(sound);
    }

    public void StartCapture(AudioClip sound) {
        sources[1].clip = sound;
        sources[1].Play();
    }

    public void StopCapture() {
        sources[1].Stop();
    }
}
