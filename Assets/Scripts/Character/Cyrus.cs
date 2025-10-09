using UnityEngine;

/// <summary>
/// Represents Cyrus, the powerful antagonist from another dimension.
/// He is the father of Cirrus, and their conflict is a central part of the story.
/// </summary>
public class Cyrus : Character
{
    /// <summary>
    /// Initializes the character with Cyrus's specific attributes.
    /// Sets his name and exceptionally high stats, establishing him as a formidable foe.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Cyrus";
        maxHealth = 1800;
        currentHealth = maxHealth;
        attack = 200;
        defense = 80;
    }
}