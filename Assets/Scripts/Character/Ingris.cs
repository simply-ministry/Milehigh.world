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
}