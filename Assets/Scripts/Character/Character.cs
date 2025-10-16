using System;
using UnityEngine;

/// <summary>
/// An enum to represent the different states a character can be in.
/// </summary>
public enum CharacterState
{
    Idle,
    Walking,
    Attacking,
    VoidTransformed,
    UnifiedEntity,
    Dead
}

/// <summary>
/// The base class for all characters in Milehigh.World.
/// Contains core attributes, combat stats, and methods for health/mana management.
/// Implements an event-driven approach for state changes.
/// </summary>
public class Character : MonoBehaviour
{
    // Events for state changes
    public event Action<float, float> OnHealthChanged; // currentHealth, maxHealth
    public event Action<float, float> OnManaChanged;   // currentMana, maxMana
    public event Action<float> OnDamageTaken;          // damageAmount

    [Header("Core Identification")]
    [Tooltip("A unique identifier for this character instance.")]
    public Guid CharacterId { get; private set; }

    [Header("Core Attributes")]
    public string characterName = "Character";
    public float maxHealth = 100f;
    public float maxMana = 100f;
    public bool isAlive = true;

    // Encapsulated health and mana fields
    private float _currentHealth;
    public float Health
    {
        get => _currentHealth;
        private set
        {
            float clampedValue = Mathf.Clamp(value, 0, maxHealth);
            if (_currentHealth != clampedValue)
            {
                _currentHealth = clampedValue;
                OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            }
        }
    }

    private float _currentMana;
    public float Mana
    {
        get => _currentMana;
        private set
        {
            float clampedValue = Mathf.Clamp(value, 0, maxMana);
            if (_currentMana != clampedValue)
            {
                _currentMana = clampedValue;
                OnManaChanged?.Invoke(_currentMana, maxMana);
            }
        }
    }

    [Header("Combat Stats")]
    public int attack = 10;
    public int defense = 5;

    protected virtual void Awake()
    {
        CharacterId = Guid.NewGuid();
        // Set properties directly to trigger initial events
        Health = maxHealth;
        Mana = maxMana;
    }

    /// <summary>
    /// Applies damage to the character and invokes damage/health events.
    /// </summary>
    public virtual void TakeDamage(float amount)
    {
        if (!isAlive) return;

        float damageTaken = Mathf.Max(1f, amount - defense);
        Health -= damageTaken;

        Debug.Log($"{characterName} takes {damageTaken} damage.");
        OnDamageTaken?.Invoke(damageTaken);

        if (Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the character for a given amount.
    /// </summary>
    public void Heal(float amount)
    {
        if (!isAlive) return;
        Health += amount;
        Debug.Log($"{characterName} heals for {amount}.");
    }

    /// <summary>
    /// Reduces character's mana and returns true if successful.
    /// </summary>
    public bool UseMana(float amount)
    {
        if (Mana >= amount)
        {
            Mana -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the character's death logic.
    /// </summary>
    protected virtual void Die()
    {
        isAlive = false;
        Debug.Log($"{characterName} has been defeated.");
        // We don't disable the GameObject immediately to allow other scripts to react to the death event.
        // Consider a separate manager to handle object cleanup.
        // gameObject.SetActive(false);
    }
}