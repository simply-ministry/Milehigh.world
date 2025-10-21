// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// The base class for all Ɲōvəmîŋāđ characters.
/// This abstract class provides the foundational properties and methods
/// that all playable characters in Milehigh.World will share.
/// </summary>
public abstract class Novamina : Character
{
    [Header("Novamina Details")]
    public string Archetype;

    /// <summary>
    /// Initialization method for the character.
    /// Calls the base class's Awake method and initializes Novamina-specific details.
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); // Call the base Character class's Awake method
        Debug.Log($"Novamina Initialized: {characterName} ({Archetype})");
    }

    // Common methods and properties for all Novamina can be added here in the future.
    // For example: Special abilities, resource pools, etc.
}