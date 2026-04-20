using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth { get; private set; }
    [field: SerializeField] public int maxHealth { get; private set; } = 100;

    public Action<int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        Pickup_Handler.OnHealthPickup += Heal;
    }

    private void OnDisable()
    {
        Pickup_Handler.OnHealthPickup -= Heal;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    void Die()
    {
        //reload scene for testing:
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene("GameOver");
    }
}