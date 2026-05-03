using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string previousScene;
    public void LoadSceneByName(string sceneName)
    {
        if(sceneName == "MainMenu")
        {
            Time.timeScale = 1.0f;
        }
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadPreviousScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(previousScene);
    }

    public void EndGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
