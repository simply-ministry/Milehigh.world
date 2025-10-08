using UnityEngine;

/// <summary>
/// Represents Sobieski, the Polish Paladin.
/// A stoic defender, blending high resilience with moderate offensive capabilities.
/// </summary>
public class Sobieski : Novamina
{
    /// <summary>
    /// Initializes Sobieski's stats and archetype.
    /// Overrides the base Awake method to set character-specific values.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // It's crucial to call the base class's Awake method.

        // Set the unique properties for Sobieski
        characterName = "Sobieski";
        Archetype = "Polish Paladin";

        // Assign stats reflecting a Paladin: high defense, moderate attack
        maxHealth = 200;
        currentHealth = maxHealth;
        attack = 60;
        defense = 120;

        Debug.Log($"{characterName}, the {Archetype}, has been initialized.");
    }
}