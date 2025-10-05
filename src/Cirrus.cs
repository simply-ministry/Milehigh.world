using UnityEngine;

/// <summary>
/// Cirrus the Dragon King (The Primal Scion)
/// </summary>
public class Cirrus : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Cirrus the Dragon King";
        Archetype = "Elemental Bruiser / Area Control";
    }

    // --- Key Abilities from GDD to be implemented ---
    // - Dragon Form Toggle
    // - Elemental Breath
    // - Draconic Fury
}