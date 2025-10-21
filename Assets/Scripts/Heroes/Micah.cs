// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Represents Micah the Unbreakable, a high-ranking Vanguard General
/// who champions technological supremacy over the Void.
/// He acts as a primary antagonist representing the "Extreme Order" ending.
/// </summary>
public class Micah : ShadowSyndicateVillain
{
    [Header("Micah's Tech")]
    public int suppressionFieldPower = 50;

    protected override void Awake()
    {
        base.Awake();
        villainName = "Micah the Unbreakable";
        maxHealth = 3000; // Befitting a Juggernaut
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Fires a high-energy kinetic slug from his power armor's railgun.
    /// Represents his primary offensive capability.
    /// </summary>
    /// <param name="target">The hero to be targeted.</param>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} fires his Railgun at {target.name}!");
        // TODO: Implement projectile logic and high-impact damage.
    }

    /// <summary>
    /// Activates a Void-Suppressing Reality Field, weakening mystical abilities.
    /// This is his signature ultimate ability, reflecting his core philosophy.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName} activates his Void-Suppressing Reality Engine!");
        // TODO: Apply a "Silence" or "Energy Drain" debuff to all heroes.
    }

    /// <summary>
    /// Enters a defensive stance, hardening his armor to absorb damage.
    /// </summary>
    public void HardenArmor()
    {
        currentState = VillainAIState.Idle; // Or a new "Defending" state if added to base
        Debug.Log($"{villainName}'s armor hardens, deflecting incoming attacks!");
        // TODO: Activate defensive VFX, apply a damage reduction buff.
    }
}