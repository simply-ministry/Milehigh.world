using UnityEngine;

/// <summary>
/// Otis the Skywanderer (X)
/// </summary>
public class Otis : Novamina
{
    void Awake()
    {
        CharacterName = "Otis the Skywanderer";
        Archetype = "Agile DPS / Scout / Manipulator";
        Initialize();
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