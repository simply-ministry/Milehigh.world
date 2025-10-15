using UnityEngine;

public class Nafaerius : ShadowSyndicateVillain
{
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