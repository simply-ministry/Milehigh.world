using UnityEngine;

/// <summary>
/// Anastasia the Dreamer (The Ethereal Guardian)
/// </summary>
public class Anastasia : Novamina
{
    void Awake()
    {
        CharacterName = "Anastasia the Dreamer";
        Archetype = "Support / Crowd Control (CC) Mage";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Supportive:
    // - Manipulates Dreamscape energies for healing and debuff removal.
    // - Creates protective barriers.

    // Crowd Control:
    // - Subtly influences enemy aggression.

    // Passive:
    // - "Memory Echoes": Offers tactical insights.
    // - "Reality Anchor": Provides brief moments of stability.

    void Update()
    {
        // Gameplay logic for Anastasia
    }
}