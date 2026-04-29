using System;
using UnityEngine;

public class Pickup_Handler : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        Score,
        Key,
        Intel
    }

    public static event Action<int> OnHealthPickup;
    public static event Action<int> OnScorePickup;
    public static event Action OnKeyPickup;
    public static event Action OnIntelPickup;
    public Sprite IntelSprite;
    public Sprite HealthSprite;
    public Sprite KeySprite;
    public PickupType pickupType;
    
    [Header("Health")]
    public int healthValue = 20;

    [Header("Score")]
    public int scoreValue = 100;

    private SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        SetColorByType();
    }

    void SetColorByType()
    {
        if (rend == null) return;

        switch (pickupType)
        {
            case PickupType.Health:
                rend.color = Color.red;
                break;

            case PickupType.Score:
                rend.color = Color.cyan;
                break;

            case PickupType.Key:
                rend.sprite = KeySprite;
                break;

            case PickupType.Intel:
                rend.sprite = IntelSprite;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePickup();
            Destroy(gameObject);
        }
    }

    void HandlePickup()
    {
        switch (pickupType)
        {
            case PickupType.Health:
                OnHealthPickup?.Invoke(healthValue);
                Debug.Log("Health");
                break;

            case PickupType.Score:
                OnScorePickup?.Invoke(scoreValue);
                Debug.Log("Score");
                break;

            case PickupType.Key:
                OnKeyPickup?.Invoke();
                Debug.Log("Key");
                break;

            case PickupType.Intel:
                OnIntelPickup?.Invoke();
                Debug.Log("Intel");
                break;
        }

        AudioManager.Instance.PlaySFX(SFXType.Pickup);
    }
}