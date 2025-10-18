using UnityEngine;

/// <summary>
/// Represents Zia the Just, a diplomatic and moral voice who represents the
/// common people caught in the war between technology and mysticism.
/// </summary>
public class Zia : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Zia the Just";
        maxHealth = 120;
        maxEnergy = 180; // High energy for supportive abilities
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Unleashes a wave of righteous energy, healing allies and damaging unjust foes.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} releases a wave of righteous energy, bolstering the worthy!");
        // TODO: Implement a heal-over-time for allies and/or minor damage to enemies.
    }

    /// <summary>
    /// Issues a rallying cry, inspiring allies and boosting their resolve.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        Debug.Log($"{heroName}'s voice rings with conviction, inspiring her allies!");
        // TODO: Apply a temporary buff (e.g., attack or defense up) to nearby allies.
    }

    /// <summary>
    /// Creates a Sanctuary of peace, protecting those within from harm for a short time.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        Debug.Log($"{heroName} creates a Sanctuary, a bastion of hope in the chaos!");
        // TODO: Create a protective zone that grants invulnerability or high damage reduction.
    }
}