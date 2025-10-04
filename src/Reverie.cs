using UnityEngine;

/// <summary>
/// Reverie (The Arcane Weaver)
/// </summary>
public class Reverie : Novamina
{
    void Awake()
    {
        CharacterName = "Reverie";
        Archetype = "Controller / Elemental Mage";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Controller:
    // - Commands arcane elements to twist reality.
    // - Creates illusions and disorients enemies.

    // Elemental:
    // - "Elemental Infusion": Imbues attacks with diverse damage types.

    // Utility:
    // - "Arcane Symphony": Solves ancient puzzles or resonates with dormant powers.

    void Update()
    {
        // Gameplay logic for Reverie
    }
}