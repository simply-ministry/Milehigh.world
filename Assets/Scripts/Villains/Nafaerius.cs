using UnityEngine;
using System;

/// <summary>
/// Represents Nafaerius, the ruler of The Shadow Dominion and the original invader
/// responsible for the fragmentation of Mîlēhîgh.wørld. He is a primordial force
/// of darkness, seeking to extend his control over all realities.
/// </summary>
public class Nafaerius : Character
{
    // --- Narrative Properties ---

    /// <summary>
    /// The source of Nafaerius's power and the dimension he rules.
    /// </summary>
    public string Realm { get; private set; } = "The Shadow Dominion";


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource representing Nafaerius's control over the battlefield.
    /// It increases for each enemy currently affected by his shadow abilities.
    /// </summary>
    [field: SerializeField]
    public int Dominion { get; private set; }

    /// <summary>
    /// The maximum Dominion Nafaerius can exert.
    /// </summary>
    [field: SerializeField]
    public int MaxDominion { get; private set; } = 100;


    // --- Initialization ---

    /// <summary>
    /// Initializes Nafaerius's specific attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Nafaerius, Ruler of The Shadow Dominion";
        // As the original, ultimate antagonist, his power level is immense.
        maxHealth = 10000; // Increased health for a final boss
        currentHealth = maxHealth;
        Dominion = 0;
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// An ability that tethers a target with living shadows. The tether slows the
    /// target and deals damage over time. Each tethered enemy increases Dominion.
    /// </summary>
    /// <param name="target">The enemy to ensnare.</param>
    public void ShadowTether(Character target)
    {
        Debug.Log($"{characterName} extends a tendril of living shadow, tethering {target.characterName}!");
        // In-game logic would apply a "slow" and "damage-over-time" debuff.
        // For each active tether, Nafaerius's Dominion would passively increase.
        Dominion = Mathf.Min(MaxDominion, Dominion + 5); // Example gain
    }

    /// <summary>
    /// Consumes Dominion to create a perfect clone of an enemy out of shadow,
    /// forcing them to fight their own dark reflection.
    /// </summary>
    /// <param name="target">The character to clone.</param>
    public void CreateUmbralClone(Character target)
    {
        if (Dominion >= 60)
        {
            Dominion -= 60;
            Debug.Log($"{characterName} spends Dominion to weave a perfect shadow clone of {target.characterName}!");
            // In-game logic to spawn a temporary, hostile copy of the target character.
        }
        else
        {
            Debug.Log($"{characterName} does not have enough Dominion to create an Umbral Clone.");
        }
    }

    /// <summary>
    /// The ultimate ability. Nafaerius attempts to extinguish all light on the battlefield,
    /// plunging it into absolute darkness. While active, enemies are blinded, and
    /// Nafaerius's power is greatly increased.
    /// </summary>
    public void WorldlessChasm()
    {
        if (Dominion >= 100)
        {
            Dominion = 0;
            Debug.Log($"{characterName} unleashes his ultimate power, plunging the world into a Worldless Chasm!");
            Debug.Log("...All light is extinguished, and only the shadows remain!");
            // In-game logic for a powerful, battlefield-wide debuff that severely hinders the heroes
            // while providing a massive buff to Nafaerius for a limited time.
        }
        else
        {
            Debug.Log($"{characterName}'s Dominion is not yet at its peak. The Worldless Chasm cannot be summoned.");
        }
    }
}
