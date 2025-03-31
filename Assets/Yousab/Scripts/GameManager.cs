using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int selectedLevel;
    public int highestUnlockedLevel = 1;
    public static GameManager Instance {get; private set;}
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void SetLevel(int level)
    {
        selectedLevel = level;
        Debug.Log("Level " + selectedLevel);
    }

    public void CompleteLevel(int level)
{
    if (level >= highestUnlockedLevel)
    {
        highestUnlockedLevel = level + 1;
    }
}
}
