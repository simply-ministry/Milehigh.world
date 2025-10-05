using UnityEngine;

/// <summary>
/// Micah the Unbreakable (The Earthshaper Bulwark)
/// </summary>
public class Micah : Novamina
{
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

    void Update()
    {
        // Gameplay logic for Micah
    }
}