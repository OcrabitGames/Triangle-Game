using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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
    [SerializeField] private GameObject levelTextPrefab;

    // Reference Scripts
    private SoundFXManager _soundFXManager;

    // Some funcs to get and tack on the text appearing when the scene loads
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level") && currentLevel != 0)
        {
            if (currentLevel > numberOfLevels) {ShowLevelText($"Game Complete!!!"); return; }
            ShowLevelText($"Level {currentLevel}");
        }
    }
    
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
            if (currentLevel > numberOfLevels) {ShowLevelText($"Game Complete!!!");}
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
        //print($"Level Complete {currentLevel}");
        if (currentLevel >= highestUnlockedLevel)
        {
            //print($"Incrementing Highest Level {highestUnlockedLevel}");
            highestUnlockedLevel++;
        }
        NextLevel();
    }

    public void GoToHighestUnlockedLevel() {
        SelectLevel(highestUnlockedLevel);
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

    public void ShowLevelText(string text, float duration = 2f)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogWarning("Canvas not found in the scene.");
            return;
        }

        if (levelTextPrefab == null)
        {
            Debug.LogWarning("Level text prefab not assigned.");
            return;
        }

        GameObject levelTextInstance = Instantiate(levelTextPrefab, canvas.transform);
        TMP_Text tmpText = levelTextInstance.GetComponent<TMP_Text>();

        if (tmpText)
        {
            tmpText.text = text;
            StartCoroutine(AnimateAndDestroyLevelText(levelTextInstance, tmpText, duration));
        }
    }

    private IEnumerator AnimateAndDestroyLevelText(GameObject instance, TMP_Text tmpText, float duration)
    {
        if (!instance)
            yield break;
        
        float timer = 0f;
        Vector3 originalScale = instance.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        while (timer < duration)
        {
            if (!instance)
                yield break;

            float t = timer / duration;
            instance.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            timer += Time.deltaTime;
            yield return null;
        }

        if (instance)
            Destroy(instance);
    }
}
