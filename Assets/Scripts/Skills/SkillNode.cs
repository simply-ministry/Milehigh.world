// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines a single skill or talent as a ScriptableObject.
/// This allows for easy creation and management of skills as assets in the Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill Node")]
public class SkillNode : ScriptableObject
{
    [Header("Skill Information")]
    [Tooltip("The name of the skill to be displayed in the UI.")]
    public string skillName;

    [Tooltip("A detailed description of what the skill does.")]
    [TextArea(3, 5)]
    public string description;

    [Header("Requirements")]
    [Tooltip("The character level required to unlock this skill.")]
    public int requiredLevel = 1;

    [Tooltip("How many skill points are needed to unlock this skill.")]
    public int skillPointCost = 1;

    [Tooltip("Other skills that must be unlocked before this one becomes available. (Optional)")]
    public List<SkillNode> prerequisites = new List<SkillNode>();

    [Header("Rewards")]
    [Tooltip("The new ability this skill grants to the character. (Optional)")]
    public Ability grantsAbility;

    [Tooltip("A list of permanent stat increases this skill provides.")]
    public List<StatBoost> statBoosts = new List<StatBoost>();
}