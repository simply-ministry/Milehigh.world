// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A MonoBehaviour component that manages a character's skill tree.
/// This component should be attached to the same GameObject as the Character script.
/// </summary>
[RequireComponent(typeof(Character))]
public class CharacterSkillTree : MonoBehaviour
{
    [Tooltip("The complete list of all skill nodes that make up this character's skill tree.")]
    public List<SkillNode> allSkills = new List<SkillNode>();

    [Tooltip("A list of skills that have been unlocked by the character.")]
    private HashSet<SkillNode> unlockedSkills = new HashSet<SkillNode>();

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    /// <summary>
    /// Attempts to unlock a specific skill for the character.
    /// </summary>
    /// <param name="skillToUnlock">The skill node to attempt to unlock.</param>
    public void UnlockSkill(SkillNode skillToUnlock)
    {
        // --- Pre-computation Checks ---
        if (skillToUnlock == null)
        {
            Debug.LogError("Cannot unlock a null skill.");
            return;
        }

        if (unlockedSkills.Contains(skillToUnlock))
        {
            Debug.LogWarning($"Skill '{skillToUnlock.skillName}' is already unlocked.");
            return;
        }

        // --- Requirement Checks ---
        if (character.level < skillToUnlock.requiredLevel)
        {
            Debug.Log($"Failed to unlock '{skillToUnlock.skillName}'. Required level: {skillToUnlock.requiredLevel}, Character level: {character.level}");
            return;
        }

        if (character.skillPoints < skillToUnlock.skillPointCost)
        {
            Debug.Log($"Failed to unlock '{skillToUnlock.skillName}'. Not enough skill points. Required: {skillToUnlock.skillPointCost}, Have: {character.skillPoints}");
            return;
        }

        foreach (var prerequisite in skillToUnlock.prerequisites)
        {
            if (!unlockedSkills.Contains(prerequisite))
            {
                Debug.Log($"Failed to unlock '{skillToUnlock.skillName}'. Prerequisite '{prerequisite.skillName}' is not unlocked.");
                return;
            }
        }

        // --- Unlock Process ---
        Debug.Log($"Unlocking skill: {skillToUnlock.skillName}");

        // 1. Deduct cost and mark as unlocked
        character.skillPoints -= skillToUnlock.skillPointCost;
        unlockedSkills.Add(skillToUnlock);

        // 2. Grant new ability, if any
        if (skillToUnlock.grantsAbility != null)
        {
            if (!character.abilities.Contains(skillToUnlock.grantsAbility))
            {
                character.abilities.Add(skillToUnlock.grantsAbility);
                Debug.Log($"Ability '{skillToUnlock.grantsAbility.abilityName}' granted.");
            }
        }

        // 3. Apply permanent stat boosts
        foreach (var boost in skillToUnlock.statBoosts)
        {
            ApplyStatBoost(boost);
        }

        Debug.Log($"Successfully unlocked '{skillToUnlock.skillName}'. {character.characterName} has {character.skillPoints} skill points remaining.");
    }

    /// <summary>
    /// Applies a single stat boost to the character.
    /// </summary>
    /// <param name="boost">The StatBoost to apply.</param>
    private void ApplyStatBoost(StatBoost boost)
    {
        switch (boost.stat)
        {
            case StatType.MaxHealth:
                character.maxHealth += boost.value;
                character.currentHealth += boost.value; // Also increase current health
                break;
            case StatType.Attack:
                character.attack += boost.value;
                break;
            case StatType.Defense:
                character.defense += boost.value;
                break;
            case StatType.Speed:
                character.speed += boost.value;
                break;
        }
        Debug.Log($"Applied stat boost: +{boost.value} {boost.stat}");
    }

    /// <summary>
    /// Checks if a specific skill has been unlocked.
    /// </summary>
    /// <param name="skill">The skill to check.</param>
    /// <returns>True if the skill is in the set of unlocked skills.</returns>
    public bool IsSkillUnlocked(SkillNode skill)
    {
        return unlockedSkills.Contains(skill);
    }
}