using UnityEngine;

/// <summary>
/// Represents Reverie, a hero who specializes in illusions and reality manipulation.
/// This class defines her unique abilities and initial attributes.
/// </summary>
public class Reverie : NoveminaadHero
{
    /// <summary>
    /// Initializes Reverie's specific attributes.
    /// Overrides the base Awake method to set her name, health, and energy.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        heroName = "Reverie";
        maxHealth = 90;
        maxEnergy = 180;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Creates disorienting illusions.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} casts a disorienting illusion!");
        // TODO: Apply 'Confuse' status effect to enemies, spawn illusion VFX.
    }

    /// <summary>
    /// Temporarily fades into the Dreamscape, becoming invulnerable.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        Debug.Log($"{heroName} fades into the Dreamscape, becoming untouchable.");
        // TODO: Implement a short-duration invulnerability or phasing effect.
    }

    /// <summary>
    /// Weaves a waking nightmare, trapping enemies in a psychic prison.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        Debug.Log($"{heroName} weaves a 'Waking Nightmare', trapping enemies in fear!");
        // TODO: Implement an area-of-effect (AoE) stun or fear ability.
    }
}