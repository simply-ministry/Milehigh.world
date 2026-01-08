using UnityEngine;

/// <summary>
/// Represents Cirrus The Dragon King, an ancient, primordial Archon of the Void.
/// He is the personification of the "Extreme Chaos" ending, seeking to purge
/// technology and return the Multiverse to a state of natural energy flow.
/// </summary>
public class Cirrus : ShadowSyndicateVillain
{
    [Header("Dragon King's Wrath")]
    public int stormBreathDamage = 150;

    protected override void Awake()
    {
        base.Awake();
        villainName = "Cirrus The Dragon King";
        maxHealth = 5000; // Befitting an ancient being
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Unleashes a torrent of raw Void energy, devastating a single target.
    /// </summary>
    /// <param name="target">The hero to be targeted.</param>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} unleashes a torrent of Void energy at {target.name}!");
        // TODO: Implement a high-damage, single-target spell effect.
    }

    /// <summary>
    /// Breathes a storm of chaotic energy, hitting all heroes in a wide arc.
    /// This is his signature ultimate ability, a display of his raw power.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName} inhales, gathering the storm... then unleashes the Maelstrom!");
        // TODO: Implement a large, cone-shaped area-of-effect attack.
    }
}