// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Represents Kai the Prophet, the lore-keeper and visionary of the Multiverse.
/// He provides cryptic but vital information about the consequences of the player's choices.
/// </summary>
public class Kai : NoveminaadHero
{
    [Header("Prophetic Insight")]
    [TextArea(3, 5)]
    public string currentProphecy;

    protected override void Awake()
    {
        base.Awake();
        heroName = "Kai the Prophet";
        maxHealth = 100; // Not a combatant
        maxEnergy = 300; // High energy for visions
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Gazes into the Void to see visions of what is to come.
    /// This is his primary "ability."
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        currentProphecy = "The paths diverge... One of steel, one of fire, one of balance. Your choice will shape the echo of reality.";
        Debug.Log($"{heroName} murmurs: '{currentProphecy}'");
    }

    /// <summary>
    /// Shares a vision with the player, directly communicating the consequences of their actions.
    /// In a real implementation, this would trigger a UI element or a cutscene.
    /// </summary>
    public void ShareVision()
    {
        Debug.Log($"{heroName} touches your forehead, and your mind is flooded with images of possible futures.");
        // TODO: Trigger a narrative event or display a UI panel with prophetic text/images.
    }

    // Override combat-oriented methods to reflect his non-combatant role.
    public override void UseSecondaryAbility()
    {
        Debug.Log($"{heroName} does not engage in combat, for his battle is with fate itself.");
    }

    public override void UseUltimateAbility()
    {
        Debug.Log($"{heroName}'s power is not for fighting, but for understanding.");
    }
}