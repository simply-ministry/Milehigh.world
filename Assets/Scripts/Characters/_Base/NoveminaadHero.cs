using UnityEngine;

/// <summary>
/// The base class for all Noveminaad hero archetypes.
/// It provides core attributes like health, energy, and virtual methods for abilities
/// that can be overridden by specific hero implementations.
/// </summary>
public abstract class NoveminaadHero : MonoBehaviour
{
    [Header("Hero Attributes")]
    public string heroName;
    public int maxHealth = 100;
    public int currentHealth;
    public int maxEnergy = 100; // For abilities
    public int currentEnergy;

    public enum HeroState { Idle, Moving, Attacking, Defending, Dead }
    public HeroState currentState;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        currentState = HeroState.Idle;
    }

    /// <summary>
    /// Reduces the hero's health by a specified amount.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take.</param>
    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{heroName} takes {damageAmount} damage. Health is now {currentHealth}/{maxHealth}.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Handles the hero's death.
    /// </summary>
    protected virtual void Die()
    {
        currentState = HeroState.Dead;
        Debug.Log($"{heroName} has been defeated.");
        // TODO: Play death animation, disable controller, etc.
    }

    // --- ABILITIES (to be overridden by child classes) ---

    /// <summary>
    /// A virtual method for the hero's primary ability.
    /// To be called from the EpicBattleScene or a PlayerController.
    /// </summary>
    public virtual void UsePrimaryAbility()
    {
        Debug.Log($"{heroName} uses their Primary Ability.");
        currentState = HeroState.Attacking;
    }

    /// <summary>
    /// A virtual method for the hero's secondary or defensive ability.
    /// </summary>
    public virtual void UseSecondaryAbility()
    {
        Debug.Log($"{heroName} uses their Secondary Ability.");
    }

    /// <summary>
    /// A virtual method for the hero's ultimate ability.
    /// </summary>
    public virtual void UseUltimateAbility()
    {
        Debug.Log($"{heroName} uses their Ultimate Ability!");
    }
}