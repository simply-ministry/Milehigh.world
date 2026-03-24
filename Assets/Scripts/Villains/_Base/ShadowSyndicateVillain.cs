using UnityEngine;

/// <summary>
/// The base class for all Shadow Syndicate villains.
/// Provides core attributes and AI state management.
/// </summary>
public abstract class ShadowSyndicateVillain : MonoBehaviour
{
    [Header("Villain Attributes")]
    /// <summary>The name of the villain.</summary>
    public string villainName;
    /// <summary>The maximum health of the villain.</summary>
    public int maxHealth = 200;
    /// <summary>The current health of the villain.</summary>
    public int currentHealth;

    /// <summary>An enum representing the villain's possible AI states.</summary>
    public enum VillainAIState { Idle, Patrolling, Chasing, Attacking, Casting, Dead }
    /// <summary>The current AI state of the villain.</summary>
    public VillainAIState currentState;

    /// <summary>
    /// Initializes the villain's default values when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentState = VillainAIState.Idle;
    }

    /// <summary>
    /// Reduces the villain's health by a specified amount.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take.</param>
    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{villainName} takes {damageAmount} damage. Health is now {currentHealth}/{maxHealth}.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Handles the villain's death.
    /// </summary>
    protected virtual void Die()
    {
        currentState = VillainAIState.Dead;
        Debug.Log($"{villainName} has been vanquished.");
        // TODO: Play death animation, trigger loot drop, etc.
    }

    // --- ABILITIES (to be overridden by child classes) ---

    /// <summary>
    /// A virtual method for the villain's primary ability.
    /// </summary>
    /// <param name="target">The target of the ability.</param>
    public virtual void UsePrimaryAbility(GameObject target)
    {
        Debug.Log($"{villainName} uses their Primary Ability on {target.name}.");
        currentState = VillainAIState.Attacking;
    }

    /// <summary>
    /// A virtual method for the villain's ultimate ability.
    /// </summary>
    public virtual void UseUltimateAbility()
    {
        Debug.Log($"{villainName} unleashes their Ultimate Ability!");
        currentState = VillainAIState.Casting;
    }
}
