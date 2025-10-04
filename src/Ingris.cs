using UnityEngine;

/// <summary>
/// Ingris the Phoenix Warrior (The Spirit of Rebirth)
/// </summary>
public class Ingris : Novamina
{
    void Awake()
    {
        CharacterName = "Ingris the Phoenix Warrior";
        Archetype = "Melee / AoE DPS / Self-Sustaining Bruiser";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Melee / AoE DPS:
    // - "Phoenix Fire": Unleashes devastating flame attacks.

    // Self-Sustaining:
    // - "Rebirth Protocol" (Passive): Temporarily returns from fatal damage with a portion of health and increased power.

    void Update()
    {
        // Gameplay logic for Ingris
    }
}