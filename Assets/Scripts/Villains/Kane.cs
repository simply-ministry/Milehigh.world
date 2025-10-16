using UnityEngine;

/// <summary>
/// Represents Kane, a straightforward and brutal melee villain.
/// </summary>
public class Kane : ShadowSyndicateVillain
{
    /// <summary>
    /// Initializes Kane's attributes.
    /// </summary>
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
    /// <param name="target">The target of the attack.</param>
    public override void UsePrimaryAbility(GameObject target)
 {
 base.UsePrimaryAbility(target);
 Debug.Log($"{villainName} lunges forward with a brutal strike aimed at {target.name}!");
 // TODO: Play attack animation, apply damage to the target on hit.
 }
}