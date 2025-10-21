// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System;

/// <summary>
/// Represents Delilah the Desolate, a character corrupted by a powerful, dark entity.
/// Her abilities revolve around spreading blight and decay, using a unique resource.
/// </summary>
public class DelilahTheDesolate : Character
{
    /// <summary>
    /// A reference to her original, uncorrupted self, if applicable in the story.
    /// This could be used for narrative triggers or flashbacks.
    /// </summary>
    public Guid OriginalSelfId { get; set; }

    /// <summary>
    /// The source of Delilah's power, a core narrative element.
    /// </summary>
    public string PowerSource { get; private set; } = "The Omen";

    /// <summary>
    /// Blight is a resource she generates and consumes for her special abilities.
    /// </summary>
    [Header("Delilah's Unique Stats")]
    [Tooltip("Current Blight resource level.")]
    public float Blight { get; private set; }

    /// <summary>
    /// The maximum amount of Blight Delilah can accumulate.
    /// </summary>
    [Tooltip("Maximum Blight resource level.")]
    public float MaxBlight { get; private set; } = 100f;

    /// <summary>
    /// Initializes Delilah's specific attributes, setting her name, health, and starting Blight.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Delilah the Desolate";
        maxHealth = 160; // From the user's snippet
        currentHealth = maxHealth;
        Blight = 25f; // Initial Blight value

        // Keeping existing stats not specified in the snippet
        attack = 180;
        defense = 70;
    }

    /// <summary>
    /// An ability that damages a target and increases Delilah's Blight.
    /// </summary>
    /// <param name="target">The character to affect.</param>
    public void TouchOfDecay(Character target)
    {
        Debug.Log($"{characterName} touches {target.characterName}, afflicting them with a decaying curse.");
        // In a full implementation, this would apply a damage-over-time effect.
        // For now, we'll just log it.

        float blightGained = 5f;
        Blight = Mathf.Min(MaxBlight, Blight + blightGained);
        Debug.Log($"({characterName} gains {blightGained} Blight. Current Blight: {Blight})");
    }

    /// <summary>
    /// A powerful ability that consumes a large amount of Blight to summon a creature.
    /// </summary>
    /// <param name="target">The target for the summoned avatar.</param>
    public void SummonOmenAvatar(Character target)
    {
        if (Blight >= 60)
        {
            Blight -= 60;
            Debug.Log($"{characterName} spends {60} Blight to summon a terrifying avatar of The Omen...");
            // Game logic for summoning the avatar would go here.
            // This could involve instantiating a new GameObject.
        }
        else
        {
            Debug.Log($"{characterName} does not have enough Blight to summon the avatar.");
        }
    }

    /// <summary>
    /// An ultimate ability that creates a damaging zone.
    /// </summary>
    public void VoidblightZone()
    {
        if (Blight >= 90)
        {
            Blight -= 90;
            Debug.Log($"{characterName} spends {90} Blight to create a Voidblight Zone!");
            // Game logic for creating the damaging zone would go here.
        }
        else
        {
            Debug.Log($"{characterName} does not have enough Blight to create a Voidblight Zone.");
        }
    }
}