using UnityEngine;

public class Cyrus : ShadowSyndicateVillain
{
    protected override void Awake()
    {
        base.Awake();
        villainName = "Cyrus";
        maxHealth = 400;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Unleashes a torrent of chaotic energy.
    /// </summary>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} unleashes a torrent of chaotic energy at {target.name}!");
        // TODO: Instantiate projectile or beam VFX, deal damage to the target.
    }
}