using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuButton;
    public GameObject crosshair;
    public Player_Movement fpsController;
    public SceneLoader sceneLoader;

    private bool paused = false;

    void Start()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
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
        crosshair.SetActive(!paused);
        fpsController.enabled = !paused;



        Time.timeScale = paused ? 0f : 1f;

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

        public void ReturnToMainMenu()
    {
        sceneLoader.LoadSceneByName("MainMenu");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}