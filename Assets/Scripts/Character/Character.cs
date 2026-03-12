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
/// This class manages core attributes like health and mana, handles combat calculations,
/// and provides an event-driven system for other game systems to subscribe to.
/// It is intended to be inherited by more specific character types like Novamina (players) or Villains.
/// </summary>
public abstract class Character : MonoBehaviour
{
    // --- Events ---

    /// <summary>
    /// Fired when the character's health changes.
    /// Provides the current and maximum health values.
    /// </summary>
    public event Action<float, float> OnHealthChanged;

    /// <summary>
    /// Fired when the character's mana changes.
    /// Provides the current and maximum mana values.
    /// </summary>
    public event Action<float, float> OnManaChanged;

    /// <summary>
    /// Fired when the character takes damage.
    /// Provides the final damage amount taken after calculations.
    /// </summary>
    public event Action<float> OnDamageTaken;

    // --- Core Identification ---
    // Events for state changes
    public event Action<float, float> OnHealthChanged; // currentHealth, maxHealth
    public event Action<float, float> OnManaChanged;   // currentMana, maxMana
    public event Action<float, float> OnStaminaChanged; // currentStamina, maxStamina
    public event Action<float> OnDamageTaken;          // damageAmount
    public event Action OnDie;

    [Header("Core Identification")]
    [Tooltip("A unique identifier for this character instance, assigned at runtime.")]
    public Guid CharacterId { get; private set; }

    // --- Core Attributes ---

    [Header("Core Attributes")]
    [Tooltip("The display name of the character.")]
    public string characterName = "Character";

    [Tooltip("The maximum health of the character.")]
    public float maxHealth = 100f;

    [Tooltip("The maximum mana or resource of the character.")]
    public float maxMana = 100f;

    [Tooltip("Indicates whether the character is currently alive.")]
    public float maxStamina = 100f;
    public bool isAlive = true;
    public bool isCorporeal = true;

    // --- Health & Mana Properties ---

    private float _currentHealth;
    /// <summary>
    /// The current health of the character. Setting this value is clamped between 0 and maxHealth.
    /// Triggers the OnHealthChanged event when its value changes.
    /// </summary>
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
    /// <summary>
    /// The current mana of the character. Setting this value is clamped between 0 and maxMana.
    /// Triggers the OnManaChanged event when its value changes.
    /// </summary>
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

    // --- Combat Stats ---
    private float _currentStamina;
    public float Stamina
    {
        get => _currentStamina;
        private set
        {
            float clampedValue = Mathf.Clamp(value, 0, maxStamina);
            if (_currentStamina != clampedValue)
            {
                _currentStamina = clampedValue;
                OnStaminaChanged?.Invoke(_currentStamina, maxStamina);
            }
        }
    }

    [Header("Combat Stats")]
    [Tooltip("The base attack power of the character.")]
    public int attack = 10;

    [Tooltip("The base defense value of the character, used to reduce incoming damage.")]
    public int defense = 5;

    /// <summary>
    /// Initializes the character's stats and unique ID when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        CharacterId = Guid.NewGuid();
        // Set properties directly to trigger initial events for UI, etc.
        Health = maxHealth;
        Mana = maxMana;
        Stamina = maxStamina;
    }

    // --- Public Methods ---

    /// <summary>
    /// Applies a specified amount of damage to the character after factoring in defense.
    /// Triggers the OnDamageTaken event and handles death if health reaches zero.
    /// </summary>
    /// <param name="amount">The incoming amount of damage before defense reduction.</param>
    public virtual void TakeDamage(float amount)
    /// Reduces character's stamina and returns true if successful.
    /// </summary>
    public bool UseStamina(float amount)
    {
        if (Stamina >= amount)
        {
            Stamina -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Restores character's stamina.
    /// </summary>
    public void RestoreStamina(float amount)
    {
        if (!isAlive) return;
        Stamina += amount;
    }

    /// <summary>
    /// Applies damage to the character and invokes damage/health events.
    /// Integrates with the ComboManager to apply damage multipliers.
    /// </summary>
    public virtual void TakeDamage(float amount, Character instigator = null)
    {
        if (!isAlive || !isCorporeal) return;

        // If there is an instigator, register the attack for combo tracking.
        if (instigator != null && ComboManager.Instance != null)
        {
            ComboManager.Instance.RegisterAttack(instigator);
            float multiplier = ComboManager.Instance.GetDamageMultiplier(instigator);
            amount *= multiplier;
            if (multiplier > 1.0f)
            {
                Debug.Log($"Damage multiplied by {multiplier}x!");
            }
        }

        // Ensure damage is at least 1 after defense reduction.
        float damageTaken = Mathf.Max(1f, amount - defense);
        Health -= damageTaken;

        Debug.Log($"{characterName} takes {damageTaken} damage.");
        OnDamageTaken?.Invoke(damageTaken);

        if (Health <= 0)
        {
            Die(instigator);
        }
    }

    /// <summary>
    /// Heals the character for a given amount, up to their maximum health.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(float amount)
    {
        if (!isAlive) return;
        Health += amount;
        Debug.Log($"{characterName} heals for {amount}.");
    }

    /// <summary>
    /// Attempts to consume a specified amount of mana.
    /// </summary>
    /// <param name="amount">The amount of mana to use.</param>
    /// <returns>True if the character had enough mana and it was consumed, false otherwise.</returns>
    public bool UseMana(float amount)
    {
        if (Mana >= amount)
        {
            Mana -= amount;
            return true;
        }
        return false;
    }

    // --- Protected Methods ---

    /// <summary>
    /// Handles the character's death logic. Sets the character's state to not alive.
    /// This method is virtual so that subclasses can add custom death behaviors (e.g., animations, loot drops).
    /// </summary>
    protected virtual void Die(Character killer)
    {
        isAlive = false;
        OnDie?.Invoke();
        Debug.Log($"{characterName} has been defeated.");
        // We don't disable the GameObject immediately to allow other scripts (like an encounter manager)
        // to react to the death event before the object is cleaned up.
    }
}