using UnityEngine;

public class Micah : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Micah the Unbreakable";
        maxHealth = 250;
        maxEnergy = 80;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Enters a defensive stance, radiating resilience and absorbing damage.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        currentState = HeroState.Defending;
        Debug.Log($"{heroName} stands firm, his form radiating resilience!");
        // TODO: Activate defensive VFX, apply a damage reduction buff.
    }
}