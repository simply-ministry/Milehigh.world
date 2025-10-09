using UnityEngine;

/// <summary>
/// Represents Zaia the Just, one of the Ɲōvəmîŋāđ.
/// Zaia is a swift and precise melee assassin, focused on exploiting enemy weaknesses
/// and executing high-priority targets.
/// </summary>
public class Zaia : Novamina
{
    /// <summary>
    /// Initializes Zaia's specific attributes, setting her name and archetype.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Zaia the Just";
        Archetype = "Melee DPS / Assassin";
    }

    // --- Key Abilities from GDD ---

    // Melee DPS:
    // - Focuses on critical weaknesses in enemy formations.
    // - "Justice's Edge": Grants bonus damage against corrupted or "unjust" targets.

    // Assassin:
    // - "Shadow Step": Allows for rapid repositioning.

    /// <summary>
    /// Called every frame, this method will contain Zaia's gameplay logic.
    /// </summary>
    void Update()
    {
        // Gameplay logic for Zaia
    }
}