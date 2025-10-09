using UnityEngine;

/// <summary>
/// Represents Kai the Prophet, one of the Ɲōvəmîŋāđ.
/// Kai is a tactical seer whose abilities focus on gathering information,
/// revealing enemy weaknesses, and providing support to his allies.
/// </summary>
public class Kai : Novamina
{
    /// <summary>
    /// Initializes Kai's specific attributes, setting his name and archetype.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Kai the Prophet";
        Archetype = "Support / Information Gatherer / Tactical Seer";
    }

    // --- Key Abilities from GDD ---

    // Tactical Seer:
    // - "Prophetic Glimpse": Reveals enemy weaknesses or optimal paths.
    // - "Truth of Lîŋq": Illuminates hidden elements in the environment.

    // Support:
    // - "Insightful Aura": Enhances allies.

    /// <summary>
    /// Called every frame, this method will contain Kai's gameplay logic.
    /// </summary>
    void Update()
    {
        // Gameplay logic for Kai
    }
}