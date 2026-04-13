using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth { get; private set; }
    [field: SerializeField] public int maxHealth { get; private set; } = 100; //an editable variable in Inspector, {Can only be read in other scripts, only this script can change it}
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

    //Script Called when Enemy attacks
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
        // Destroy(gameObject);
        //Delete Player Object/ Return to Main Menu or to Death Screen
    }


}

