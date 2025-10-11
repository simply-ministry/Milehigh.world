using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enum to track leadership status within the group.
/// </summary>
public enum LeadershipStatus
{
    Member,
    ReluctantLeader,
    AcceptedLeader
}

/// <summary>
/// Represents Anastasia the Dreamer, a support mage who shapes reality through dreams.
/// She is a reluctant leader whose prophetic visions guide the Ɲōvəmîŋāđ.
/// </summary>
public class Anastasia : Novamina
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// Tracks her evolving role within the group of heroes.
    /// This can affect her abilities or how other characters interact with her.
    /// </summary>
    public LeadershipStatus LeaderRole { get; private set; }

    /// <summary>
    /// A list to store the insights gained from her prophetic visions.
    /// In a real game, this could be a list of custom "Prophecy" objects or simple strings.
    /// </summary>
    public List<string> Visions { get; private set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// The primary resource Anastasia uses for her dream-weaving abilities.
    /// </summary>
    public float DreamEnergy { get; set; }

    /// <summary>
    /// The maximum amount of Dream Energy she can hold.
    /// </summary>
    public float MaxDreamEnergy { get; set; }


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Anastasia the Dreamer";
        Archetype = "Support / Crowd Control (CC) Mage";
        maxHealth = 90; // As a mage, might be slightly less durable.
        currentHealth = maxHealth;
        MaxDreamEnergy = 200f;
        DreamEnergy = MaxDreamEnergy;

        LeaderRole = LeadershipStatus.Member; // Starts as a regular member.
        Visions = new List<string>();
    }


    // --- Narrative Methods ---

    /// <summary>
    /// A narrative method representing the moment Anastasia is thrust into leadership.
    /// This could be triggered by a specific plot point in the game.
    /// </summary>
    public void AssumeLeadershipRole()
    {
        if (LeaderRole == LeadershipStatus.Member)
        {
            Debug.Log($"[NARRATIVE] In a moment of crisis, all eyes turn to {characterName}. She reluctantly steps up to lead.");
            LeaderRole = LeadershipStatus.ReluctantLeader;
            // This change in status could unlock new "leadership" or "aura" type abilities.
        }
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// Placeholder for the "Dream Weaving" ability. Can be used to heal or create illusions.
    /// </summary>
    /// <param name="target">The character to be affected (ally or enemy).</param>
    /// <param name="isHealing">True to perform a heal, false to create a debuffing illusion.</param>
    public void WeaveDream(Character target, bool isHealing)
    {
        if (DreamEnergy >= 30)
        {
            DreamEnergy -= 30;
            if (isHealing)
            {
                // In-game logic to restore health to the target.
                Debug.Log($"{characterName} weaves a soothing dream, healing {target.characterName}.");
            }
            else
            {
                // In-game logic to apply a "confusion" or "fear" status effect to the enemy.
                Debug.Log($"{characterName} weaves a nightmare, manipulating the perceptions of {target.characterName}.");
            }
        }
    }

    /// <summary>
    /// Placeholder for the "Prophetic Visions" ability.
    /// In a game, this might reveal an enemy's weakness or warn of an incoming attack.
    /// </summary>
    public void GlimpseFuture()
    {
        // This is more of a utility/narrative skill.
        // It could provide a temporary buff to the party or reveal information.
        string newVision = "A vision reveals the enemy's next move!"; // Example vision text
        Visions.Add(newVision);
        Debug.Log($"[VISION] {characterName} sees a glimpse of the future: '{newVision}'");
    }

    /// <summary>
    /// Placeholder for the "Somnus Aura" ability.
    /// Creates a field that affects multiple characters in an area.
    /// </summary>
    /// <param name="targets">A list of all characters within the aura's range.</param>
    public void EmitSomnusAura(List<Character> targets)
    {
        if (DreamEnergy >= 50)
        {
            DreamEnergy -= 50;
            Debug.Log($"{characterName} radiates a calming aura...");
            // In-game logic would iterate through all targets in an area.
            foreach (var target in targets)
            {
                // This is a simplified check. A real game would use factions (player vs. enemy).
                if (target.characterName.Contains("Enemy")) // A simple way to differentiate for this example
                {
                    Debug.Log($"...the aura puts {target.characterName} into a daze, lowering their defenses.");
                }
                else
                {
                    Debug.Log($"...the aura soothes {target.characterName}, calming their nerves.");
                }
            }
        }
    }
}