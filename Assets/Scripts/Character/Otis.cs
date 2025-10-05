using UnityEngine;

/// <summary>
/// Otis the Skywanderer (X)
/// </summary>
public class Otis : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Otis the Skywanderer";
        Archetype = "Agile DPS / Scout / Manipulator";
    }

    // --- Key Abilities from GDD ---

    // Agile DPS / Scout:
    // - "Void-Kissed Agility": Swift, strategic combat movements.

    // Manipulator:
    // - "Disruptive Tech": Deploys deceptive technology.
    // - Obscured memories allow for unpredictable tactics.

    void Update()
    {
        // Gameplay logic for Otis
    }
}