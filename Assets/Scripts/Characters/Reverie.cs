using UnityEngine;

public class Reverie : NoveminaadHero
{
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