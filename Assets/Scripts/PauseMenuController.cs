using System;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static event Action<bool> onPausedChanged;
    public GameObject pauseCanvas;
    public GameObject crosshair;
    public Player_Movement fpsController;
    public SceneLoader sceneLoader;

    private bool paused = false;

    void Start()
    {
        SetPauseState(false);
        /*
        fpsController.enabled = true;
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
    }
    public void SetPauseState(bool value)
    {

        paused = value;

        pauseCanvas.SetActive(paused);
        crosshair.SetActive(!paused);
        fpsController.SetPause(paused);

        Time.timeScale = paused ? 0f : 1f;


        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
        if (!paused)
        {
            Input.ResetInputAxes();
        }

        onPausedChanged?.Invoke(paused);


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
             TogglePause();
    }

   
    public void TogglePause()
    {
        SetPauseState(!paused);
        /*
        paused = !paused;

        pauseCanvas.SetActive(paused);
        crosshair.SetActive(!paused);
        fpsController.enabled = !paused;



        Time.timeScale = paused ? 0f : 1f;

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;

        onPausedChanged?.Invoke(paused);
        */
    }

        public void ReturnToMainMenu()
    {
        sceneLoader.LoadSceneByName("MainMenu");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}