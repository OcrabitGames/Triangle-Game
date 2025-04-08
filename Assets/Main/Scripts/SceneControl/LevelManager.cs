using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // an Instance that doesn't get destroyed so this works on all scenes
    public static LevelManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    [SerializeField] private int numberOfLevels = 5;
    [SerializeField] private int currentLevel; // Note 0 means menu
    [SerializeField] private int highestUnlockedLevel = 1;
    
    // Reference Scripts
    private SoundFXManager _soundFXManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = 0;
        _soundFXManager = SoundFXManager.Instance;
        if (_soundFXManager == null)
        {
            print("No sound FX Manager");
        }
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SelectLevel(int level)
    {
        
        if (highestUnlockedLevel >= level) {
            print($"Level Set to {level}");
            currentLevel = level;
            UpdateLevelScene();
        } else {
            print("Level not unlocked");
        }
    }

    private void NextLevel()
    {
        SelectLevel(currentLevel + 1);
    }
    
    
    // Make a play to highest level button
    public void CompleteLevel()
    {
        print($"Level Complete {currentLevel}");
        if (currentLevel >= highestUnlockedLevel)
        {
            print($"Increamenting Highest Level {highestUnlockedLevel}");
            highestUnlockedLevel++;
        }
        NextLevel();
    }

    public void UpdateLevelScene()
    {
        if (currentLevel <= 5)
        {
            SceneManager.LoadScene($"Level{currentLevel}");
            _soundFXManager.onMenuScreen = false;
            _soundFXManager.SetMainAudio();
        } else {
            print("This should never happen");
        }
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
        currentLevel = 0;
    }
    
    public void GotoMenu()
    {
        SceneManager.LoadScene("MenuScreen");
        currentLevel = 0;
        _soundFXManager.onMenuScreen = true;
        _soundFXManager.SetMainAudio();
    }

    public int GetMaxUnlockedLevel()
    {
        return highestUnlockedLevel;
    }
}
