using UnityEngine;

/// <summary>
/// Zaia the Just (The Swift Executioner)
/// </summary>
public class Zaia : Novamina
{
    void Awake()
    {
        CharacterName = "Zaia the Just";
        Archetype = "Melee DPS / Assassin";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Melee DPS:
    // - Focuses on critical weaknesses in enemy formations.
    // - "Justice's Edge": Grants bonus damage against corrupted or "unjust" targets.

    // Assassin:
    // - "Shadow Step": Allows for rapid repositioning.

    void Update()
    {
        // Gameplay logic for Zaia
    }
}