using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClipPair
{
    public string name;
    public AudioClip clip;
}

public class SoundFXManager : MonoBehaviour {
    // an Instance that doesn't get destroyed so this works on all scenes
    public static SoundFXManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    private AudioSource _mainAudioSource;
    private AudioSource _sideAudioSource;
    public List<SoundClipPair> ClipList;
    private Dictionary<string, AudioClip> ClipDictionary;
    public bool onMenuScreen = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        var audioSources = GetComponents<AudioSource>();
        _mainAudioSource = audioSources[0];
        _sideAudioSource = audioSources[1];
        
        ClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var pair in ClipList)
        {
            if (!ClipDictionary.ContainsKey(pair.name))
            {
                ClipDictionary[pair.name] = pair.clip;
            }
        }

        SetMainAudio();
    }

    public void SetMainAudio()
    {
        _mainAudioSource.Stop();
        _mainAudioSource.loop = true;
        if (onMenuScreen)
        {
            _mainAudioSource.clip = GenericGetSound("MenuMusic");
            _mainAudioSource.volume = 1f;
        } else {
            _mainAudioSource.clip = GenericGetSound("GameMusic");
            _mainAudioSource.volume = .9f;
        }
        _mainAudioSource.Play();
    }
    
    public void PlaySound(AudioClip sound) {
        _sideAudioSource.PlayOneShot(sound);
    }

    public void StartCapture() {
        _sideAudioSource.clip = GenericGetSound("Capturing");
        _sideAudioSource.Play();
    }

    public void StopCapture() {
        _sideAudioSource.Stop();
    }
    
    public void FinishCapture() {
        _sideAudioSource.Stop();
        GenericPlaySound("Capture Success");
    }
    
    public void PlayStep()
    {
        GenericPlaySound("Step");
    }

    public void PlayBackButton()
    {
        GenericPlaySound("Back");
    }

    public void PlayPressButton()
    {
        GenericPlaySound("Press");
    }

    public void PlayFormLink()
    {
        GenericPlaySound("Form Link");
    }
    
    public void PlayLoseLink()
    {
        GenericPlaySound("Lose Link");
    }
    
    public void PlayPlace()
    {
        GenericPlaySound("Place");
    }

    public void PlaySelect()
    {
        GenericPlaySound("Select");
    }

    private void GenericPlaySound(string soundName)
    {
        if (ClipDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            _sideAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Clip '{soundName}' not found in ClipDictionary.");
        }
    }

    private AudioClip GenericGetSound(string soundName)
    {
        if (ClipDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning($"Clip '{soundName}' not found in ClipDictionary.");
            return null;
        }
    }
}
