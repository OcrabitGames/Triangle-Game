using UnityEngine;

public class LevelManagerOld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(int level)
    {
        if (GameManager.Instance.highestUnlockedLevel >= level) {
            Debug.Log(GameManager.Instance.highestUnlockedLevel);
            GameManager.Instance.SetLevel(level);
        } else {
            Debug.Log("Level not unlocked");
        }
    }
}
