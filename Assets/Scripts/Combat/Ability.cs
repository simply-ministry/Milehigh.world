using UnityEngine;

/// <summary>
/// Represents a character's ability or skill.
/// This can be used to define attacks, spells, or other actions.
/// </summary>
[CreateAssetMenu(fileName = "New Ability", menuName = "Combat/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string description;

    public DamageType damageType = DamageType.Physical;
    public int power = 10; // The base power of the ability
    public float critChance = 0.05f; // 5% critical hit chance
    public float critMultiplier = 2.0f; // Double damage on crit
}