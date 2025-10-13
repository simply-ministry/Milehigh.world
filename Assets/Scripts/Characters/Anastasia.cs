using UnityEngine;

public class Anastasia : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Anastasia";
        maxHealth = 120;
        maxEnergy = 200;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Conjures a shimmering barrier to deflect attacks.
    /// This corresponds to the action in EpicBattleScene.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility(); // Sets state to Attacking/Casting
        Debug.Log($"{heroName} conjures a shimmering barrier!");
        // TODO: Instantiate barrier VFX, apply defensive buffs.
    }
}