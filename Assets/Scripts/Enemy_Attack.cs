using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Attack : MonoBehaviour
{
    [Header("Combat")]
    public float range = 15f;
    public float fireRate = 1.2f;
    public float laserDuration = 0.1f;
    public int damage = 10;
    public float baseSpread = 0.02f;

    [Header("Reference")]
    public Transform firePoint;

    private Transform player;
    private PlayerHealth playerHealth;

    private Renderer rend;
    private LineRenderer line;

    private float fireTimer;
    private float laserTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        rend = GetComponent<Renderer>();
        line = GetComponent<LineRenderer>();

        line.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        fireTimer -= Time.deltaTime;

        // Aim fire point at player
        if (firePoint != null)
        {
            firePoint.LookAt(player);
        }

        if (rend.isVisible && InRange())
        {
            if (fireTimer <= 0f)
            {
                FireLaser();
                fireTimer = fireRate;
            }
        }

        HandleLaserVisual();
    }

    bool InRange()
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    void FireLaser()
    {
        Vector3 start = firePoint.position;

        // Direction toward player
        Vector3 direction = (player.position - start).normalized;

        // Add spread based on distance
        float dist = Vector3.Distance(start, player.position);
        float spreadAmount = baseSpread + (dist * 0.002f);

        direction += Random.insideUnitSphere * spreadAmount;
        direction.Normalize();

        RaycastHit hit;
        Vector3 end = start + direction * range;

        if (Physics.Raycast(start, direction, out hit, range))
        {
            end = hit.point;

            // Handles player with multiple colliders
            if (hit.collider.transform.root.CompareTag("Player"))
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }

        // Draw laser
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        line.enabled = true;
        laserTimer = laserDuration;
    }

    void HandleLaserVisual()
    {
        if (!line.enabled) return;

        laserTimer -= Time.deltaTime;

        if (laserTimer <= 0f)
        {
            line.enabled = false;
        }
    }
}