using UnityEngine;

/// <summary>
/// Represents The Omen, a powerful and swift antagonist in the game world.
/// This class defines its formidable base stats, establishing it as a major threat.
/// </summary>
public class TheOmen : Character
{
    /// <summary>
    /// Initializes The Omen's specific attributes.
    /// Sets its name and high stats to establish it as a formidable foe.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "The Omen";
        maxHealth = 1300;
        currentHealth = maxHealth;
        attack = 160;
        defense = 50;
        speed = 80f;
    }
}