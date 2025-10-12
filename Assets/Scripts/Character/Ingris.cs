using UnityEngine;

/// <summary>
/// Represents Ingris the Phoenix Warrior, one of the Ɲōvəmîŋāđ.
/// Her fighting style is a blend of aggressive melee combat and self-sustaining abilities,
/// embodying the spirit of rebirth and resilience.
/// </summary>
public class Ingris : Novamina
{
    /// <summary>
    /// Initializes Ingris's specific attributes, setting her name and archetype.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Ingris the Phoenix Warrior";
        Archetype = "Melee / AoE DPS / Self-Sustaining Bruiser";
    }

    // --- Key Abilities from GDD to be implemented ---
    // - Phoenix Fire (Combo)
    // - Shadow Strike
    // - Rebirth

    /// <summary>
    /// Unleashes a circular burst of fire, damaging all nearby enemies.
    /// Costs 40 mana.
    /// </summary>
    /// <param name="enemiesInRange">A list of all enemies within the ability's radius.</param>
    public void PhoenixWing(Character[] enemiesInRange)
    {
        if (!UseMana(40))
        {
            Debug.Log($"{characterName} attempts to use Phoenix Wing, but lacks the mana!");
            return;
        }

        Debug.Log($"{characterName} erupts with fiery energy, unleashing her Phoenix Wing for 40 mana!");
        foreach (var enemy in enemiesInRange)
        {
            Debug.Log($"...The flames scorch {enemy.characterName}!");
            // In a full implementation, we would call enemy.TakeDamage() here.
        }
    }
}