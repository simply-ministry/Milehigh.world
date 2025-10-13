using UnityEngine;

public class Zaia : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Zaia";
        maxHealth = 100;
        maxEnergy = 120;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Performs a swift, precise attack.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} strikes with blinding speed!");
        // TODO: Play quick dash/attack animation, deal damage to a target.
    }
}