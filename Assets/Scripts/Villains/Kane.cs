using UnityEngine;

public class Kane : ShadowSyndicateVillain
{
    protected override void Awake()
    {
        base.Awake();
        villainName = "Kane";
        maxHealth = 350;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// A straightforward and brutal melee strike.
    /// </summary>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} lunges forward with a brutal strike aimed at {target.name}!");
        // TODO: Play attack animation, apply damage to the target on hit.
    }
}