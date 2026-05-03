using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class HitscanDetector : MonoBehaviour
{
    private InputAction _attack;
    private InputActionMap _playerMap;


    [SerializeField] private InputActionAsset _actionMap;
    [SerializeField] private string _actionMapName = "Player";

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _firePoint;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private float _laserDuration = 0.05f;

    public static event Action<int> OnEnemyHit;

    private bool isPaused;


    void Awake()
    {
        _playerMap = _actionMap.FindActionMap(_actionMapName);
        _attack = _actionMap.FindActionMap(_actionMapName).FindAction("Attack");
    }

    void Update()
    {
        if (isPaused == true)
        {
            return;
        }
        if (_attack.WasPressedThisFrame())
        {
            HandleHitscan();
        }
    }

    void HandleHitscan()
    {
        RaycastHit objectHit;

        // Start from fire point (gun barrel)
        Vector3 start = _firePoint.position;

        AudioManager.Instance.PlaySFXAtPosition(SFXType.LaserShot, start);

        // Default direction = camera forward
        Vector3 direction = _camera.transform.forward;

        // Ray from camera to determine exact aim point
        Ray camRay = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(camRay, out RaycastHit camHit, 2000f))
        {
            // Adjust direction so bullet goes from firepoint to where camera is aiming
            direction = (camHit.point - start).normalized;
        }

        Vector3 end = start + direction * 2000f;

        // Actual shot from firepoint
        if (Physics.Raycast(start, direction, out objectHit, 2000f))
        {
            end = objectHit.point;

            GameObject hitObject = objectHit.collider.gameObject;

            if (hitObject.CompareTag("WeakPoint"))
            {
                Destroy(hitObject.transform.parent.gameObject);
                OnEnemyHit?.Invoke(200);
                AudioManager.Instance.PlaySFXAtPosition(SFXType.EnemyKill, objectHit.point);
            }
            else
            {
                AudioManager.Instance.PlaySFXAtPosition(SFXType.BulletImpact, objectHit.point);
            }
        }

        StartCoroutine(ShowLaser(start, end));

    }

    IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        _laser.startWidth = 0.05f;
        _laser.endWidth = 0.05f;

        _laser.enabled = true;

        _laser.SetPosition(0, start);
        _laser.SetPosition(1, end);

        yield return new WaitForSeconds(_laserDuration);

        _laser.enabled = false;
    }

    private void OnEnable()
    {
        _playerMap.Enable();
        _attack.Enable();
        PauseMenuController.onPausedChanged += HandlePause;
    }

    private void OnDisable()
    {
        _playerMap.Disable();
        _attack.Disable();
        PauseMenuController.onPausedChanged -= HandlePause;

    }

    private void HandlePause(bool paused)
    {
        isPaused = paused;
        if (isPaused == true)
        {
            _playerMap.Disable();
        }
        else
        {
            _playerMap.Enable();
        }
    }
}