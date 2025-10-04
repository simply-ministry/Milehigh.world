using UnityEngine;

/// <summary>
/// Aeron the Brave (The Skyborn Sentinel)
/// </summary>
public class Aeron : Novamina
{
    void Awake()
    {
        CharacterName = "Aeron the Brave";
        Archetype = "Tank / Melee DPS";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Tanking:
    // - Excels in drawing enemy aggression.
    // - "Grizzled Hide" (Passive): Absorbs damage.

    // Melee DPS:
    // - "Sunder Strikes": Powerful winged assaults.
    // - Inspiring roars.

    void Update()
    {
        // Gameplay logic for Aeron
    }
}