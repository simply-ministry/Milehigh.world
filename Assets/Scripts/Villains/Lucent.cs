// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System;

/// <summary>
/// Represents Lucent, a manipulative and cunning antagonist.
/// He operates from the shadows, controlling the powerful entity Era
/// and orchestrating events based on a mistaken vendetta.
/// </summary>
public class Lucent : Character
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// A reference to the powerful entity of the Void that Lucent manipulates.
    /// </summary>
    public Guid ControlledEntityId { get; set; }

    /// <summary>
    /// The mistaken belief that drives his actions, serving as his primary motivation.
    /// </summary>
    public string CoreMotivation { get; private set; } = "Mistakenly believes Cirrus is a dragon slayer who must be punished.";


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource representing Lucent's control over the minds of his enemies.
    /// It is generated whenever an enemy is afflicted by one of his debuffs.
    /// </summary>
    [field: SerializeField]
    public int Influence { get; private set; }

    /// <summary>
    /// The maximum amount of Influence Lucent can exert.
    /// </summary>
    [field: SerializeField]
    public int MaxInfluence { get; private set; } = 100;


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Lucent";
        maxHealth = 1400; // Not a frontline fighter; relies on cunning.
        currentHealth = maxHealth;
        Influence = 0;
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// An ability that whispers falsehoods to a target, causing them to doubt their allies.
    /// Generates Influence.
    /// </summary>
    /// <param name="target">The enemy to affect.</param>
    public void WhisperOfDoubt(Character target)
    {
        int influenceGained = 15;
        Influence = Mathf.Min(MaxInfluence, Influence + influenceGained);

        Debug.Log($"{characterName} whispers insidious lies to {target.characterName}, clouding their judgment. (+{influenceGained} Influence)");
        // In-game logic would apply a "confusion" debuff, possibly causing the target to attack a random character.
    }

    /// <summary>
    /// An ability that places a "puppet" debuff on an enemy. While the debuff is active,
    /// a portion of the damage dealt to the puppet is also dealt to one of their allies.
    /// Spends Influence.
    /// </summary>
    /// <param name="puppet">The enemy to place the debuff on.</param>
    /// <param name="linkedAlly">The ally who will also take damage.</param>
    public void MiseryPuppet(Character puppet, Character linkedAlly)
    {
        if (Influence >= 40)
        {
            Influence -= 40;
            Debug.Log($"{characterName} spends Influence to turn {puppet.characterName} into a Misery Puppet, linking their pain to {linkedAlly.characterName}!");
            // In-game logic to apply a status effect that redirects a percentage of damage.
        }
    }

    /// <summary>
    /// An ultimate ability where Lucent exerts his full control, temporarily
    /// taking direct command of an enemy character.
    /// Spends all Influence.
    /// </summary>
    /// <param name="target">The enemy to control.</param>
    public void AbsoluteControl(Character target)
    {
        if (Influence >= 100)
        {
            Influence = 0;
            Debug.Log($"{characterName} focuses his will, seizing complete control of {target.characterName}'s mind!");
            // In a real game, this would be a powerful "charm" or "mind control" effect,
            // allowing the AI (or player) to control the target for a short duration.
        }
    }
}