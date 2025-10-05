using UnityEngine;

/// <summary>
/// Reverie (The Arcane Weaver)
/// </summary>
public class Reverie : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Reverie";
        Archetype = "Controller / Elemental Mage";
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