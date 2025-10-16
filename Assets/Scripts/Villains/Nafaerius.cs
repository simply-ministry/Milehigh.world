using UnityEngine;

/// <summary>
/// Represents Nafaerius, a powerful villain who wields shadowflame.
/// </summary>
public class Nafaerius : ShadowSyndicateVillain
{
    /// <summary>
    /// Initializes Nafaerius's attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        villainName = "Nafaerius";
        maxHealth = 1000;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Gathers shadowflame for a devastating attack.
    /// </summary>
    public override void UseUltimateAbility()
 {
 base.UseUltimateAbility();
 Debug.Log($"{villainName} draws power from the abyss, gathering shadowflame!");
 // TODO: Instantiate charging VFX, prepare for a large area-of-effect attack.
 }
}