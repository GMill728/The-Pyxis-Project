using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    private InputAction _interact;

    [SerializeField] private InputActionAsset _actionMap;
    [SerializeField] private string _actionMapName = "Player";

    [SerializeField] private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _interact = _actionMap.FindActionMap(_actionMapName).FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (_interact.WasPressedThisFrame())
        {
            Debug.Log("Key Pressed");
            HandleTerminal();
        }
    }

    void HandleTerminal()
    {
        RaycastHit terminalHit;
        Debug.Log("Terminal Checked");
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out terminalHit, 10))
            if (terminalHit.transform.tag == "Terminal")
            {
                Debug.Log("Terminal Hit");
                SceneLoader.LoadSceneByName("Puzzle");
            }
    }
}
