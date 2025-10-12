using UnityEngine;

/// <summary>
/// Represents Kai the Prophet, one of the Ɲōvəmîŋāđ.
/// Kai is a tactical seer whose abilities focus on gathering information,
/// revealing enemy weaknesses, and providing support to his allies.
/// </summary>
public class Kai : Novamina
{
    // Placeholder for Kai's unique abilities.
    // In a full implementation, these methods would contain logic
    // to reveal information to the player or apply buffs.

    public void PropheticGlimpse(Character enemy)
    {
        Debug.Log($"{characterName} uses Prophetic Glimpse on {enemy.characterName}, revealing a weakness!");
    }

    public void TruthOfLinq()
    {
        Debug.Log($"{characterName} reveals the Truth of Lîŋq, illuminating hidden paths.");
    }

    // New property for ability cost
    private const int ResolveManaCost = 45;

    /// <summary>
    /// Kai's ultimate defensive ability.
    /// Creates a powerful defensive aura, significantly increasing Damage Reduction for a short duration
    /// for Kai and all allies within a 5-meter radius.
    /// </summary>
    /// <param name="duration">The time in seconds the buff will last.</param>
    /// <param name="reductionPercentage">The percentage of damage reduction granted (e.g., 0.3 for 30%).</param>
    public void AuraOfResolve(float duration = 8.0f, float reductionPercentage = 0.3f)
    {
        if (CurrentMana < ResolveManaCost)
        {
            Debug.Log($"[Kai] Fails to cast Aura of Resolve. Not enough Mana (Cost: {ResolveManaCost}).");
            return;
        }

        // Subtract Mana
        CurrentMana -= ResolveManaCost;

        // Placeholder logic for ability effect
        Debug.Log(
            $"[Kai] **Aura of Resolve** activated! Allies within 5m gain **{reductionPercentage:P0} Damage Reduction** for {duration} seconds. Mana remaining: {CurrentMana}."
        );

        // In a full implementation:
        // 1. Trigger the visual and audio effects for the aura.
        // 2. Use Physics.OverlapSphere to find allies.
        // 3. Apply a temporary 'DamageReductionBuff' component to all affected targets.
    }
}