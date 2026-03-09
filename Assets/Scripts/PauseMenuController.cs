using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuButton;
    public Player_Movement fpsController;

    private bool paused = false;

    void Start()
    {
        fpsController.enabled = true;
        menuButton.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;

        menuButton.SetActive(paused);
        fpsController.enabled = !paused;

        Time.timeScale = paused ? 0f : 1f;

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

        public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}