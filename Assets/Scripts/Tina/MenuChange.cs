using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void goToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
    public void goToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void goToLevelSelection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelection");
    }
    public void goToLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    public void goToLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
    public void goToLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
    public void goToLevel4()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level4");
    }
}
