using UnityEngine;

/// <summary>
/// Represents Kane, the Fallen Champion and rival brother to Aeron.
/// As an antagonist, he possesses formidable stats, making him a major threat.
/// </summary>
public class Kane : Novamina
{
    /// <summary>
    /// Initializes Kane's specific attributes.
    /// Sets his name, archetype, and powerful stats to establish him as a formidable foe.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Kane";
        Archetype = "Fallen Champion";
        maxHealth = 1700;
        currentHealth = maxHealth;
        attack = 190;
        defense = 90;
    }
}