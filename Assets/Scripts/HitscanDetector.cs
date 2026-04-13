using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class HitscanDetector : MonoBehaviour
{
    private InputAction _attack;

    [SerializeField]
    private InputActionAsset _actionMap;

    [SerializeField]
    private string _actionMapName = "Player";

    [SerializeField]
    private Camera _camera;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private float _laserDuration = 0.05f;

    public static event Action<int> OnEnemyHit;

    void Awake()
    {
        _attack = _actionMap.FindActionMap(_actionMapName).FindAction("Attack");
    }

    void Update()
    {
        if (_attack.WasPressedThisFrame())
        {
            HandleHitscan();
        }
    }

    void HandleHitscan()
    {
        RaycastHit objectHit;

        Vector3 start = _camera.transform.position;
        Vector3 direction = _camera.transform.forward;
        Vector3 end = start + direction * 2000f;

        if (Physics.Raycast(start, direction, out objectHit, 2000))
        {
            end = objectHit.point;

            GameObject hitObject = objectHit.collider.gameObject;

            // Weakpoint hit
            if (hitObject.CompareTag("WeakPoint"))
            {
                Destroy(hitObject.transform.parent.gameObject);
                OnEnemyHit?.Invoke(200); // stronger hit for weakpoint
            }
            // Normal enemy hit (optional fallback if body has Enemy tag)
            else if (hitObject.CompareTag("Enemy"))
            {
                OnEnemyHit?.Invoke(100);
                Destroy(hitObject);
            }
        }

        StartCoroutine(ShowLaser(start, end));
    }

    IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        _laser.enabled = true;

        _laser.SetPosition(0, start);
        _laser.SetPosition(1, end);

        yield return new WaitForSeconds(_laserDuration);

        _laser.enabled = false;
    }

    private void OnEnable() => _attack.Enable();
    private void OnDisable() => _attack.Disable();
}