using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the player's health, implementing the IHealth interface.
/// Attach this component to the player GameObject.
/// </summary>
public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Events")]
    public UnityEvent OnPlayerDamaged;
    public UnityEvent OnPlayerDeath;

    // IHealth Interface Properties
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private bool isDead = false;

    void Awake()
    {
        // Initialize health when the game starts
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Implements the TakeDamage method from the IHealth interface.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    public void TakeDamage(float amount)
    {
        if (isDead) return; // Can't take damage if already dead

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        OnPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has been defeated.");
        OnPlayerDeath?.Invoke();

        // Add any death logic here, like playing an animation or showing a game over screen.
        // For this example, we'll just disable the component.
        this.enabled = false;
    }

    /// <summary>
    /// A public method to heal the player.
    /// </summary>
    /// <param name="amount">The amount to heal.</param>
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player healed for {amount}. Current health: {currentHealth}");
    }
}