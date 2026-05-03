using System;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static event Action<bool> onPausedChanged;
    public GameObject PauseMenu;
    public CanvasGroup PauseMenuGroup;
    public GameObject confirmQuitCanvas;
    public GameObject crosshair;
    public Player_Movement fpsController;

    private bool paused = false;

    void Start()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
        confirmQuitCanvas.SetActive(false);
        SetPauseState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonClick2);
            SetPauseState(!paused);
        }
    }
    public void SetPauseState(bool value)
    {

        paused = value;

        PauseMenu.SetActive(paused);
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

    /*public void TogglePause()
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
        
    }*/
    public void ReturnToMainMenu()
    {
        SetPauseState(true);
        SceneLoader.LoadSceneByName("MainMenu");
    }

    public void ConfirmQuit()
    {
        PauseMenuGroup.alpha = 1.0f;
        PauseMenuGroup.interactable = false;
        PauseMenuGroup.blocksRaycasts = false;
        confirmQuitCanvas.SetActive(true);
    }

    public void NoQuit()
    {
        confirmQuitCanvas.SetActive(false);
        PauseMenuGroup.interactable = true;
        PauseMenuGroup.blocksRaycasts = true;
    }
}