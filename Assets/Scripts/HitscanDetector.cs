using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitscanDetector : MonoBehaviour
{

    private InputAction _attack;

    [SerializeField]
    [Tooltip("The actual action map to manage")]
    private InputActionAsset _actionMap;

    [SerializeField]
    [Tooltip("The name of the action map to manage")]
    private string _actionMapName = "Player";

    [SerializeField]
    [Tooltip("The camera of the player.")]
    private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _attack = _actionMap.FindActionMap(_actionMapName).FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (_attack.IsPressed())
        {
            HandleHitscan();
        }
    }

    void HandleHitscan()
    {
        RaycastHit objectHit;
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out objectHit, 1000);
        if (objectHit.collider != null)
        {
            if (objectHit.collider.CompareTag("Enemy"))
            {
                Destroy(objectHit.transform.gameObject);
            }
        }
        
    }

    private void OnEnable()
    {
        _attack.Enable();
    }

    private void OnDisable()
    {
        _attack.Disable();
    }
}
