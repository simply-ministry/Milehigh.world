using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents Cyrus, the tyrannical ruler from another dimension and father of Cirrus.
/// He is the primary antagonist who initiates the invasion of Mîlēhîgh.wørld,
/// driven by a destructive ideology and a mistaken vendetta.
/// </summary>
public class Cyrus : Character
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// A reference to Cyrus's son, Cirrus, with whom he has a major ideological conflict.
    /// </summary>
    public Guid SonId { get; set; }

    /// <summary>
    /// The source of Cyrus's ability to travel between dimensions, a key element of his power.
    /// </summary>
    public string PowerSource { get; private set; } = "The Onalym Nexus";


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource representing Cyrus's immense and overwhelming power.
    /// It is always full and does not need to be generated, symbolizing his relentless nature as an invader.
    /// It is consumed by his most powerful abilities, which may have long cooldowns instead.
    /// </summary>
    [field: SerializeField]
    public float Tyranny { get; private set; } = 100f;

    /// <summary>
    /// The maximum amount of Tyranny Cyrus can wield.
    /// </summary>
    [field: SerializeField]
    public float MaxTyranny { get; private set; } = 100f;


    // --- Initialization ---

    /// <summary>
    /// Initializes Cyrus's specific attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Cyrus";
        maxHealth = 300; // As a final boss-level character, his health is immense.
        currentHealth = maxHealth;
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// An ability that shatters the defenses of a target, representing his power to break worlds.
    /// </summary>
    /// <param name="target">The enemy to target.</param>
    public void WorldbreakerStrike(Character target)
    {
        Debug.Log($"{characterName} strikes with the force of a collapsing dimension, shattering {target.characterName}'s defenses!");
        // In-game logic would deal heavy damage and apply a significant defense-reduction debuff.
    }

    /// <summary>
    /// An ability that allows Cyrus to manipulate dimensional energy to create unstable,
    /// damaging rifts on the battlefield.
    /// </summary>
    public void DimensionalRift()
    {
        if (Tyranny >= 40)
        {
            Tyranny -= 40;
            Debug.Log($"{characterName} tears open a dimensional rift on the battlefield!");
            // In-game logic would create a hazardous area that deals damage over time to any character who enters it.
        }
    }

    /// <summary>
    /// An ultimate ability where Cyrus channels the Onalym Nexus to unleash a devastating
    /// beam of interdimensional energy, capable of wiping out multiple targets.
    /// </summary>
    /// <param name="targetsInLine">A list of all characters caught in the beam's path.</param>
    public void OnalymPurge(List<Character> targetsInLine)
    {
        if (Tyranny >= 100)
        {
            Tyranny = 0; // Consumes all power, likely has a very long cooldown.
            Debug.Log($"{characterName} channels the full power of the Onalym Nexus, unleashing a beam of pure destruction!");
            foreach (var target in targetsInLine)
            {
                 Debug.Log($"...The beam obliterates everything in its path, striking {target.characterName}!");
                 // ... logic for extremely high, likely lethal, damage ...
            }
        }
    }
}
