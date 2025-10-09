using UnityEngine;

/// <summary>
/// Represents Lucent, a character within the game world.
/// This class defines Lucent's base stats and identity.
/// </summary>
public class Lucent : Character
{
    /// <summary>
    /// Initializes Lucent's specific attributes, setting the character's name and stats.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Lucent";
        maxHealth = 1200;
        currentHealth = maxHealth;
        attack = 80;
        defense = 60;
    }
}