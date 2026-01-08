using UnityEngine;

/// <summary>
/// Represents Sky.ix, a powerful, sentient Artificial Intelligence.
/// It embodies the philosophy that pure computation can solve all problems,
/// acting as a key technological antagonist.
/// </summary>
public class Skyix : ShadowSyndicateVillain
{
    [Header("AI-Specific Logic")]
    public int processingPower = 1000; // Represents computational strength

    protected override void Awake()
    {
        base.Awake();
        villainName = "Sky.ix";
        maxHealth = 2000; // More fragile than a juggernaut, but still formidable
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Launches a data spike at a target, disrupting their systems (energy).
    /// </summary>
    /// <param name="target">The hero to be targeted.</param>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} launches a data-spike at {target.name}, attempting to corrupt their energy field!");
        // TODO: Implement an attack that drains the target's energy/mana.
    }

    /// <summary>
    /// Initiates a system-wide cascade failure, attempting to reboot all hostile
    /// technological systems (and heroes) in the area.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName} broadcasts a system-wide reboot command: 'CASCADE_FAILURE.EXE'");
        // TODO: Implement a powerful stun or debuff on all heroes, especially effective against tech-based ones.
    }
}