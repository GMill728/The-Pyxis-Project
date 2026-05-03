using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string previousScene;
    public static void LoadSceneByName(string sceneName)
    {
        if(sceneName == "MainMenu")
        {
            Time.timeScale = 1.0f;
        }
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadSceneByIndex(int sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public static void LoadPreviousScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    public static void EndGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
