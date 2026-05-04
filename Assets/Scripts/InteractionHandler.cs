using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class InteractionHandler : MonoBehaviour
{
    public static event Action onLevelComplete;

    private InputAction _interact;

    [SerializeField] private InputActionAsset _actionMap;
    [SerializeField] private string _actionMapName = "Player";

    [SerializeField] private Camera _camera;

    private bool hasIntel = false;
    private bool hasKey = false;

    void OnEnable()
    {
        Pickup_Handler.OnIntelPickup += OnIntelPickup;
        Pickup_Handler.OnKeyPickup += OnKeyPickup;
    }

    void OnDisable()
    {
        Pickup_Handler.OnIntelPickup -= OnIntelPickup;
        Pickup_Handler.OnKeyPickup -= OnKeyPickup;
    }

    private void OnIntelPickup()
    {
        hasIntel = true;
    }

    private void OnKeyPickup()
    {
        hasKey = true;
    }

    void Start()
    {
        _interact = _actionMap.FindActionMap(_actionMapName).FindAction("Attack");
    }

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
                if(!hasIntel) return;
                onLevelComplete?.Invoke();
                Debug.Log("Terminal Hit");
                if (hasKey)
                {
                    SceneLoader.LoadSceneByName(SceneManager.GetActiveScene().name);
                }
                else
                {
                    SceneLoader.LoadSceneByName("Puzzle");
                }
            }
    }
}
