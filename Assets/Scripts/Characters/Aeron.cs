using UnityEngine;

public class Aeron : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Aeron";
        maxHealth = 150;
        maxEnergy = 100;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Unleashes a powerful melee strike, clashing with an opponent.
    /// </summary>
    public void Clash(GameObject target)
    {
        currentState = HeroState.Attacking;
        Debug.Log($"{heroName} clashes with {target.name} in a fierce cry!");
        // TODO: Play clash animation, trigger impact VFX and SFX.
    }
}