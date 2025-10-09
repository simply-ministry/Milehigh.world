using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// A data structure to associate a DamageType with a resistance value.
/// Made serializable to be editable in the Unity Inspector.
/// </summary>
[System.Serializable]
public class DamageTypeResistance
{
    public DamageType damageType;
    [Tooltip("The flat value of resistance against this damage type.")]
    public int resistanceValue;
}

/// <summary>
/// Defines the possible animation states for a character.
/// </summary>
public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    Die
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AdvancedPhysics))]
public class Character : MonoBehaviour
{
    /// <summary>
    /// A Unique Identifier for each character instance, useful for tracking relationships.
    /// </summary>
    public Guid CharacterId { get; private set; } = Guid.NewGuid();
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;
    public int attack = 50;
    public int defense = 50;
    public float speed = 50f;

    [Header("Resistances")]
    public List<DamageTypeResistance> resistances = new List<DamageTypeResistance>();

    [Header("Abilities")]
    [Tooltip("The list of abilities this character can use.")]
    public List<Ability> abilities = new List<Ability>();

    [Header("Leveling")]
    [Tooltip("The character's current level.")]
    public int level = 1;
    [Tooltip("The character's current experience points.")]
    public int experiencePoints = 0;
    [Tooltip("The experience points needed to reach the next level.")]
    public int xpToNextLevel = 100;
    [Tooltip("The number of points available to spend on skills.")]
    public int skillPoints = 0;

    private Dictionary<DamageType, int> _resistanceMap;

    protected Animator animator;
    protected Rigidbody rb;


    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Initialize resistance map for O(1) lookup
        _resistanceMap = new Dictionary<DamageType, int>();
        foreach (var res in resistances)
        {
            if (!_resistanceMap.ContainsKey(res.damageType))
            {
                _resistanceMap.Add(res.damageType, res.resistanceValue);
            }
        }
    }

    protected virtual void Update()
    {
        // Basic movement check to switch between Idle and Walk
        if (rb.velocity.magnitude > 0.1f)
        {
            SetAnimationState(AnimationState.Walk);
        }
        else
        {
            SetAnimationState(AnimationState.Idle);
        }
    }

    /// <summary>
    /// Adds experience points to the character and checks for level up.
    /// </summary>
    /// <param name="amount">The amount of XP to gain.</param>
    public void GainXP(int amount)
    {
        experiencePoints += amount;
        Debug.Log($"{characterName} gained {amount} XP.");
        while (experiencePoints >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Handles the character's level-up logic.
    /// </summary>
    protected virtual void LevelUp()
    {
        experiencePoints -= xpToNextLevel;
        level++;
        skillPoints++; // Grant one skill point per level up
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Increase XP requirement for next level

        // Restore health and apply potential stat gains
        maxHealth += 10;
        currentHealth = maxHealth;
        attack += 5;
        defense += 5;

        Debug.Log($"*** {characterName} has reached Level {level}! ***");
        Debug.Log($"Health: {maxHealth}, Attack: {attack}, Defense: {defense}");
        Debug.Log($"Gained 1 Skill Point! Total Skill Points: {skillPoints}");
    }

    /// <summary>
    /// Sets the character's animation state.
    /// </summary>
    /// <param name="state">The animation state to set.</param>
    public void SetAnimationState(AnimationState state)
    {
        if (animator != null)
        {
            // Using an integer parameter "State" in the Animator to control states.
            animator.SetInteger("State", (int)state);
        }
    }

    /// <summary>
    /// Gets the resistance value for a specific damage type.
    /// </summary>
    /// <param name="damageType">The damage type to check.</param>
    /// <returns>The character's resistance value.</returns>
    public int GetResistanceValue(DamageType damageType)
    {
        _resistanceMap.TryGetValue(damageType, out int value);
        return value;
    }

    /// <summary>
    /// Initiates an attack on a target character using a specific ability.
    /// </summary>
    /// <param name="target">The character to attack.</param>
    /// <param name="ability">The ability to use.</param>
    /// <param name="formula">The damage formula for the calculation.</param>
    public void PerformAttack(Character target, Ability ability, CombatManager.DamageFormula formula = CombatManager.DamageFormula.Linear)
    {
        // Check if the character actually has this ability in their list
        if (!abilities.Contains(ability))
        {
            Debug.LogWarning($"{characterName} tried to use an ability they don't have: {ability.abilityName}.");
            return; // Exit the method if the character doesn't have the ability
        }

        SetAnimationState(AnimationState.Attack);
        Debug.Log($"{characterName} uses {ability.abilityName} on {target.characterName}!");
        target.TakeDamage(this, ability, formula);
    }

    /// <summary>
    /// Receives an attack, calculates damage via the CombatManager, and applies it.
    /// </summary>
    /// <param name="attacker">The character initiating the attack.</param>
    /// <param name="ability">The ability used in the attack.</param>
    /// <param name="formula">The damage formula to use.</param>
    /// <param name="customMultiplier">A custom multiplier to apply to the final damage.</param>
    public virtual void TakeDamage(Character attacker, Ability ability, CombatManager.DamageFormula formula = CombatManager.DamageFormula.Linear, float customMultiplier = 1.0f)
    {
        // Calculate final damage using the centralized CombatManager
        int damageTaken = CombatManager.CalculateDamage(attacker, this, ability, formula, customMultiplier);

        // Apply damage to health
        currentHealth = Mathf.Max(currentHealth - damageTaken, 0);
        Debug.Log($"{characterName} takes {damageTaken} damage from {attacker.characterName}'s {ability.abilityName}! Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"{characterName} heals for {amount} health!");
    }

    protected virtual void Die()
    {
        Debug.Log($"{characterName} has been defeated!");
        SetAnimationState(AnimationState.Die);
        // In a real game, you might disable the GameObject, play a death animation, etc.
        // Disabling the object is deferred to the animation event.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Makes the character "say" a line of dialogue.
    /// In a real game, this would integrate with a UI system.
    /// </summary>
    /// <param name="message">The dialogue to display.</param>
    public void Say(string message)
    {
        Debug.Log($"{characterName}: {message}");
    }
}