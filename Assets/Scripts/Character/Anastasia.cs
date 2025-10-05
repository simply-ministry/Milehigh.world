using UnityEngine;

/// <summary>
/// Anastasia the Dreamer (The Ethereal Guardian)
/// </summary>
public class Anastasia : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Anastasia the Dreamer";
        Archetype = "Support / Crowd Control (CC) Mage";
    }

    // --- Key Abilities from GDD to be implemented ---
    // - Dream Weaving (Heal/Sleep)
    // - Astral Projection
}