using UnityEngine;

/// <summary>
/// The base class for all Ɲōvəmîŋāđ characters.
/// This abstract class provides the foundational properties and methods
/// that all playable characters in Milehigh.World will share.
/// </summary>
public abstract class Novamina : MonoBehaviour
{
    [Header("Character Details")]
    public string CharacterName;
    public string Archetype;

    /// <summary>
    /// Initialization method for the character.
    /// </summary>
    public virtual void Initialize()
    {
        Debug.Log($"Character Initialized: {CharacterName} ({Archetype})");
    }

    // Common methods and properties for all Novamina can be added here in the future.
    // For example: Health, Stats, Abilities, etc.
}