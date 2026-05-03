using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    private Image image;

    void Awake()
    {
        Instance = this;
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void LoadScene(string sceneName, float delay = 0f)
    {
        StartCoroutine(LoadAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadAfterDelay(string sceneName, float delay)
    {
        image.enabled = true;

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.allowSceneActivation = false;

        // preload in background while image is visible
        while (load.progress < 0.9f)
            yield return null;

        // preload done, now wait for delay
        yield return new WaitForSeconds(delay);

        load.allowSceneActivation = true;
    }
}