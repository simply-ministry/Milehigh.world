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
}