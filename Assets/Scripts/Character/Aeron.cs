using UnityEngine;

/// <summary>
/// Represents Aeron the Brave, one of the ten Ɲōvəmîŋāđ.
/// Aeron is a Tank/Melee DPS character with high health and defense.
/// </summary>
public class Aeron : Character
{
    /// <summary>
    /// Overrides the base Awake method to set Aeron's specific stats.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // Call the base class's Awake method first.

        // --- Aeron's Base Stats ---
        characterName = "Aeron the Brave";
        maxHealth = 200;    // High health for a tank
        currentHealth = maxHealth;
        attack = 60;        // Solid melee damage
        defense = 80;       // High defense for survivability
        speed = 40f;        // Average speed

        Debug.Log($"{characterName} has been initialized with specialized stats.");
    }

    /// <summary>
    /// Aeron's specialized death method. Can be expanded with unique effects.
    /// </summary>
    protected override void Die()
    {
        Debug.Log($"{characterName} has fallen, but his courage will be remembered.");
        // Future implementation: Trigger a "Last Stand" passive or other unique effect.
        base.Die();
    }

    // --- Future Aeron-Specific Abilities ---
    // Example:
    // public void ShieldBash()
    // {
    //     Debug.Log($"{characterName} performs a mighty Shield Bash!");
    // }
}