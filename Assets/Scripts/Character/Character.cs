using UnityEngine;
using System.Collections.Generic;

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

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AdvancedPhysics))]
public class Character : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;
    public int attack = 50;
    public int defense = 50;
    public float speed = 50f;

    [Header("Resistances")]
    public List<DamageTypeResistance> resistances = new List<DamageTypeResistance>();
    private Dictionary<DamageType, int> _resistanceMap;


    protected virtual void Awake()
    {
        currentHealth = maxHealth;

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
        Debug.Log($"{characterName} uses {ability.abilityName} on {target.characterName}!");
        target.TakeDamage(this, ability, formula);
    }

    /// <summary>
    /// Receives an attack, calculates damage via the CombatManager, and applies it.
    /// </summary>
    /// <param name="attacker">The character initiating the attack.</param>
    /// <param name="ability">The ability used in the attack.</param>
    /// <param name="formula">The damage formula to use.</param>
    public virtual void TakeDamage(Character attacker, Ability ability, CombatManager.DamageFormula formula = CombatManager.DamageFormula.Linear)
    {
        // Calculate final damage using the centralized CombatManager
        int damageTaken = CombatManager.CalculateDamage(attacker, this, ability, formula);

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
        // In a real game, you might disable the GameObject, play a death animation, etc.
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