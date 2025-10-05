using UnityEngine;

/// <summary>
/// Ingris the Phoenix Warrior (The Spirit of Rebirth)
/// </summary>
public class Ingris : Novamina
{
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