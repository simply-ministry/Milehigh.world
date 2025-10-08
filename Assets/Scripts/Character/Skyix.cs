using UnityEngine;

/// <summary>
/// Sky.ix the Bionic Goddess (The Ascendant Weaver)
/// </summary>
public class Skyix : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Sky.ix the Bionic Goddess";
        Archetype = "Ranged DPS / Support Caster";
    }

    // --- Key Abilities from GDD ---

    // Offensive:
    // - Advanced technology and manipulated Void energy for offensive blasts.
    // - Area-of-effect control abilities.

    // Supportive:
    // - Supportive energy shields.

    // Passive:
    // - "Unified Destiny": Ability to neutralize corrupted energy.

    void Update()
    {
        // Gameplay logic for Sky.ix
    }
}