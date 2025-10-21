using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Era, the personification of the corrupted Void.
/// She is a powerful and chaotic entity manipulated by Lucent, and her destiny
/// is intertwined with Sky.ix, with whom she may ultimately merge to restore balance.
/// </summary>
public class Era : Character
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// A reference to the entity that manipulates her actions.
    /// </summary>
    [field: SerializeField]
    public Guid ManipulatorId { get; set; }

    /// <summary>
    /// A reference to the entity she is destined to merge with to find balance.
    /// </summary>
    [field: SerializeField]
    public Guid DestinyId { get; set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A passive property that represents Era's corrupting influence on the world.
    /// This value might increase over the course of a battle, making the environment
    /// more hazardous for her enemies.
    /// </summary>
    [field: SerializeField]
    public float BattlefieldCorruption { get; private set; }


    // --- Initialization ---

    /// <summary>
    /// Initializes Era's specific attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Era, the Corrupted Void";
        // As a personified force, her health might be exceptionally high or work in unique phases.
        maxHealth = 500;
        currentHealth = maxHealth;
        BattlefieldCorruption = 0f;
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// An ability where Era lashes out with raw chaotic energy from the Void.
    /// </summary>
    /// <param name="target">The enemy to strike.</param>
    public void ChaoticOutburst(Character target)
    {
        BattlefieldCorruption += 10;
        Debug.Log($"{characterName} unleashes a blast of pure chaos at {target.characterName}!");
        // In-game logic would deal unpredictable damage (e.g., a random value within a range).
    }

    /// <summary>
    /// An ability that corrupts a section of the battlefield, creating a permanent
    /// hazardous zone that empowers Era and harms her enemies.
    /// </summary>
    public void SpreadTheVoid()
    {
        BattlefieldCorruption += 25;
        Debug.Log($"{characterName}'s presence deepens, spreading the Void's corruption across the ground.");
        // Logic to create a persistent AoE that debuffs heroes and might buff Era herself.
    }

    [Header("Abilities")]
    [Tooltip("The ScriptableObject defining the 'Inevitable Collapse' ability.")]
    /// <summary>The ScriptableObject defining the 'Inevitable Collapse' ability.</summary>
    public Ability inevitableCollapseAbility;

    /// <summary>
    /// An ultimate ability representing the Void attempting to consume everything.
    /// It deals massive damage to all enemies, with the damage increasing based
    /// on the level of BattlefieldCorruption.
    /// </summary>
    /// <param name="allEnemies">A list of all enemy characters.</param>
    public void InevitableCollapse(List<Character> allEnemies)
    {
        if (inevitableCollapseAbility == null)
        {
            Debug.LogError("'Inevitable Collapse' ability is not assigned in the inspector!");
            return;
        }

        Debug.Log($"{characterName} channels the full, unmaking power of the Void!");
        float damageMultiplier = 1.0f + (BattlefieldCorruption / 100f);

        foreach (var enemy in allEnemies)
        {
            Debug.Log($"The Void surges towards {enemy.characterName}, amplifying damage by {damageMultiplier:P0}!");
            // NOTE: This assumes a 'TakeDamage' overload that accepts a damage multiplier.
            // Directly call TakeDamage on each enemy, passing the custom multiplier.
            enemy.TakeDamage(this, inevitableCollapseAbility, CombatManager.DamageFormula.Linear, damageMultiplier);
        }
    }
}
