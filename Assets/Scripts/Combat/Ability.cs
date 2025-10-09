using UnityEngine;

/// <summary>
/// A ScriptableObject that defines a character's ability or skill.
/// This can be used to create various attacks, spells, or other actions in the Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "New Ability", menuName = "Combat/Ability")]
public class Ability : ScriptableObject
{
    /// <summary>
    /// The name of the ability.
    /// </summary>
    public string abilityName;
    /// <summary>
    /// A detailed description of what the ability does.
    /// </summary>
    [TextArea]
    public string description;

    /// <summary>
    /// The type of damage the ability inflicts (e.g., Physical, Fire, Void).
    /// </summary>
    public DamageType damageType = DamageType.Physical;
    /// <summary>
    /// The base power or magnitude of the ability, used in damage calculations.
    /// </summary>
    public int power = 10;
    /// <summary>
    /// The chance (from 0.0 to 1.0) for this ability to be a critical hit.
    /// </summary>
    public float critChance = 0.05f;
    /// <summary>
    /// The multiplier applied to the ability's power on a critical hit.
    /// </summary>
    public float critMultiplier = 2.0f;
}