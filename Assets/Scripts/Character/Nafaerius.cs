using UnityEngine;

/// <summary>
/// Represents Nafaerius, a powerful character in the game world.
/// This class defines Nafaerius's base stats, establishing him as a formidable entity.
/// </summary>
public class Nafaerius : Character
{
    /// <summary>
    /// Initializes Nafaerius's specific attributes.
    /// Sets the character's name and exceptionally high stats.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Nafaerius";
        maxHealth = 2000;
        currentHealth = maxHealth;
        attack = 150;
        defense = 100;
    }
}