using UnityEngine;

public class Kael : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Kael";
        maxHealth = 80;
        maxEnergy = 250;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Bends time, slowing enemies or projectiles in an area.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} bends time, slowing the chaotic energies!");
        // TODO: Apply a slow-motion effect to targets within a certain radius.
        // This could be done by modifying Time.timeScale or enemy movement speeds.
    }
}