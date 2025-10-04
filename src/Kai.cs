using UnityEngine;

/// <summary>
/// Kai the Prophet (The Tactical Seer)
/// </summary>
public class Kai : Novamina
{
    void Awake()
    {
        CharacterName = "Kai the Prophet";
        Archetype = "Support / Information Gatherer / Tactical Seer";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Tactical Seer:
    // - "Prophetic Glimpse": Reveals enemy weaknesses or optimal paths.
    // - "Truth of Lîŋq": Illuminates hidden elements in the environment.

    // Support:
    // - "Insightful Aura": Enhances allies.

    void Update()
    {
        // Gameplay logic for Kai
    }
}