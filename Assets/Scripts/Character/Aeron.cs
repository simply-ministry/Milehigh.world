using UnityEngine;

/// <summary>
/// Aeron the Brave (The Skyborn Sentinel)
/// </summary>
public class Aeron : Novamina
{
    protected override void Awake()
    {
        base.Awake(); // It's crucial to call the base class's Awake method
        characterName = "Aeron the Brave";
        Archetype = "Tank / Melee DPS";
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