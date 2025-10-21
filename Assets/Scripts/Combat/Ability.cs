// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// A ScriptableObject that defines the properties of a combat ability.
/// This allows for data-driven design of character skills.
/// </summary>
[CreateAssetMenu(fileName = "New Ability", menuName = "Milehigh.World/Ability")]
public class Ability : ScriptableObject
{
    [Tooltip("The name of the ability.")]
    public string abilityName = "New Ability";
    [Tooltip("A detailed description of what the ability does.")]
    [TextArea(3, 5)]
    public string description = "Ability Description";
    [Tooltip("The amount of resource (e.g., mana) this ability costs to use.")]
    public float resourceCost = 10f;
    [Tooltip("The time in seconds before this ability can be used again.")]
    public float cooldownDuration = 1.0f;

    [Header("Damage Properties")]
    [Tooltip("The base power of the ability, used in damage calculations.")]
    public int power = 10;
    [Tooltip("The chance (0.0 to 1.0) for this ability to be a critical hit.")]
    public float critChance = 0.05f;
    [Tooltip("The multiplier applied to the damage if it is a critical hit.")]
    public float critMultiplier = 2.0f;

    /// <summary>
    /// Executes the ability's logic.
    /// </summary>
    /// <param name="caster">The character using the ability.</param>
    /// <param name="target">The character targeted by the ability.</param>
    public virtual void Use(Character caster, Character target)
    {
        if (target == null) return;

        float totalDamage = CombatManager.CalculateDamage(caster, target, this);
        target.TakeDamage(totalDamage);

        Debug.Log($"{caster.characterName} uses {abilityName} on {target.characterName}, dealing {totalDamage} damage.");
    }
}