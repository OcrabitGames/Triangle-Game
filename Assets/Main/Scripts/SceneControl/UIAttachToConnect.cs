using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIAttachToConnect : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField] private bool isLevelButton = false;
    [SerializeField] private Color disabledColor = new Color(164f/255f, 115f/255f, 114f/255f); // A47372
    [SerializeField] private Color enabledColor = new Color(114f/255f, 164f/255f, 139f/255f); // 72A48B
    [SerializeField] private int levelNum = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _levelManager = LevelManager.Instance;
        
        if (_levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene!");
        }

        if (isLevelButton)
        {
            UpdateLevelButtonState();
        }
    }

    public void UpdateLevelButtonState()
    {
        var maxUnlock = _levelManager.GetMaxUnlockedLevel();
        if (maxUnlock >= levelNum)
        {
            gameObject.GetComponent<Image>().color = enabledColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = disabledColor;
        }
    }
    
    public void GotoMenu()
    {
        _levelManager.GotoMenu();
    }

    public void SelectLevel()
    {
        if (isLevelButton)
        {
            _levelManager.SelectLevel(levelNum);
        }
    }
    
    public void ExitGame()
    {
        _levelManager.ExitGame();
    }
    
    public void CompleteLevel()
    {
        _levelManager.CompleteLevel();
    }
    
    public void GoToLevelSelect()
    {
        _levelManager.GoToLevelSelect();
    }
}
