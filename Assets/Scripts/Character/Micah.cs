using UnityEngine;

/// <summary>
/// Represents Micah the Unbreakable, one of the Ɲōvəmîŋāđ.
/// Micah is a powerful tank and defensive specialist, capable of shaping the earth
/// to protect his allies and control the battlefield.
/// </summary>
public class Micah : Novamina
{
    /// <summary>
    /// Initializes Micah's specific attributes, setting his name and archetype.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Micah the Unbreakable";
        Archetype = "Tank / Defensive Specialist";
    }

    // --- Key Abilities from GDD ---

    // Defensive:
    // - "Stone Aegis": Summons temporary, dense stone barriers or covers his body in a hardened earth shell.
    // - "Earthshaper": Erects temporary stone barriers, manipulates terrain.

    // Offensive:
    // - "Seismic Slam": Generates powerful shockwaves.

    // Passive:
    // - "Immovable Object": Highly resistant to crowd control effects.
    // - "Grounding Aura": Lessens the impact of chaotic energies.

    /// <summary>
    /// Called every frame, this method will contain Micah's gameplay logic.
    /// </summary>
    void Update()
    {
        // Gameplay logic for Micah
    }
}