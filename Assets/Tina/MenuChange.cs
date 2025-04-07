using UnityEngine;
using System.Collections;

public class MenuChange : MonoBehaviour {

    public int selectedLevel;
    public int highestUnlockedLevel = 1;
    public static MenuChange Instance { get; private set; }
    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    public void SetLevel(int level){
        selectedLevel = level;
        Debug.Log("Level " + selectedLevel);
    }
    public void CompleteLevel(int level){
        if (level >= highestUnlockedLevel){
            highestUnlockedLevel = level + 1;
        }
    }
    private IEnumerator WaitForSoundAndTransition(string sceneName){
        AudioSource source = GetComponentInChildren<AudioSource>();
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void goToGame(){
        StartCoroutine(WaitForSoundAndTransition("MarcosWorld"));
        UnityEngine.SceneManagement.SceneManager.LoadScene("MarcosWorld");
    }
    public void goToMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void goToLevelSelection(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelection");
    }
    public void goToLevel1(){
        GameManager.Instance.SetLevel(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    public void goToLevel2(){
        GameManager.Instance.SetLevel(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
    public void goToLevel3(){
        GameManager.Instance.SetLevel(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
    public void goToLevel4(){
        GameManager.Instance.SetLevel(4);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level4");
    }
    public void goToLevel5(){
        GameManager.Instance.SetLevel(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level5");
    }
    public void QuitGame(){
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}
