using UnityEngine;

/// <summary>
/// Cirrus the Dragon King (The Primal Scion)
/// </summary>
public class Cirrus : Novamina
{
    void Awake()
    {
        CharacterName = "Cirrus the Dragon King";
        Archetype = "Elemental Bruiser / Area Control";
        Initialize();
    }

    // --- Key Abilities from GDD ---

    // Elemental Bruiser:
    // - "Draconic Breath": Unleashes powerful elemental breath attacks.
    // - "Primal Fury": Enters a temporary berserk state.

    // Area Control:
    // - "Winged Dominion": Uses wings for powerful aerial maneuvers and ground slams.
    // - "Terrifying Roar": Debuffs enemies.

    // Passive:
    // - "Scaled Hide": Inherent resistance to physical and elemental damage.

    void Update()
    {
        // Gameplay logic for Cirrus
    }
}