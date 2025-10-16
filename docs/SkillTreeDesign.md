# Technical Design: Character Skill Tree System

## 1. Overview

This document outlines the technical design for a data-driven character skill tree system for Milehigh.world. The system will allow designers to create unique skill trees for different character archetypes, enabling players to customize their abilities and stats. The core design prioritizes flexibility, ease of use from the Unity Editor, and seamless integration with the existing `Character` and `Combat` systems.

## 2. Goals

*   **Designer-Friendly:** Allow game designers to create, view, and modify skill trees directly in the Unity Editor with minimal or no new code.
*   **Data-Driven:** Utilize `ScriptableObject`s to define skills and skill trees as reusable assets.
*   **Extensible:** The system should be easily extendable to support various types of skill effects (e.g., passive stat boosts, new abilities, ability modifications).
*   **Decoupled:** The skill system logic should be encapsulated in its own components, interacting with the `Character` class without requiring significant modifications to it.

## 3. Core Components (Data Layer)

The data layer is built around `ScriptableObject`s, allowing skill and tree definitions to be stored as assets in the project.

### 3.1. `Skill` (ScriptableObject)

Represents a single node in the skill tree.

**C# Definition:**
```csharp
// File: Assets/Scripts/Skills/Skill.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Skill", menuName = "Milehigh/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;

    public int skillPointCost = 1;

    public List<Skill> prerequisites = new List<Skill>();

    // The effects this skill grants when unlocked
    public List<SkillEffect> effects = new List<SkillEffect>();
}
```

**Inspector Fields:**
*   `Skill Name`: The display name of the skill.
*   `Description`: In-game description of the skill.
*   `Icon`: UI icon for the skill.
*   `Skill Point Cost`: How many points are required to unlock this skill.
*   `Prerequisites`: A list of other `Skill` assets that must be unlocked before this one.
*   `Effects`: A list of `SkillEffect` assets that are applied when the skill is learned.

### 3.2. `SkillEffect` (Abstract ScriptableObject)

A base class that defines what a skill does. Concrete implementations will define specific behaviors. This allows for creating new types of skill effects without changing the core system.

**C# Base Class Definition:**
```csharp
// File: Assets/Scripts/Skills/SkillEffect.cs
using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    // Applies the effect to the target character
    public abstract void Apply(GameObject character);

    // (Optional) Removes the effect from the target character
    public virtual void Remove(GameObject character) { }
}
```

**Example Implementations:**
*   `StatBoostEffect`: Increases a character's base stat (e.g., `maxHealth`, `attack`).
    ```csharp
    // Example: Increases a stat from the Character class by a flat amount
    public class StatBoostEffect : SkillEffect {
        public StatType statToBoost; // Enum: MaxHealth, Attack, Defense, etc.
        public int amount;
        public override void Apply(GameObject character) {
            var charStats = character.GetComponent<Character>();
            // Logic to increase the specified stat
        }
    }
    ```
*   `UnlockAbilityEffect`: Grants the character a new active or passive ability.
    ```csharp
    // Example: Adds a new Ability ScriptableObject to a character's known abilities
    public class UnlockAbilityEffect : SkillEffect {
        public Ability abilityToUnlock;
        public override void Apply(GameObject character) {
            var combatComponent = character.GetComponent<CharacterCombat>(); // Assumes a future combat component
            // Logic to add the ability
        }
    }
    ```

### 3.3. `SkillTree` (ScriptableObject)

Represents the complete skill tree for a character archetype (e.g., "Sky.ix's Voidmancer Tree"). It is essentially a collection of `Skill` assets.

**C# Definition:**
```csharp
// File: Assets/Scripts/Skills/SkillTree.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Skill Tree", menuName = "Milehigh/Skill Tree")]
public class SkillTree : ScriptableObject
{
    // A list of all skills contained in this tree.
    // The relationships/prerequisites are defined within each Skill asset.
    public List<Skill> skills = new List<Skill>();
}
```

## 4. Core Components (Logic Layer)

### 4.1. `CharacterSkillTree` (MonoBehaviour)

This component will be attached to any character `GameObject` that uses a skill tree. It manages the state of the tree for that specific character instance (e.g., which skills are unlocked, available skill points).

**C# Definition:**
```csharp
// File: Assets/Scripts/Character/CharacterSkillTree.cs
using UnityEngine;
using System.Collections.Generic;

public class CharacterSkillTree : MonoBehaviour
{
    public SkillTree skillTreeAsset;
    public int skillPoints;

    private HashSet<Skill> _unlockedSkills = new HashSet<Skill>();
    private Character _character;

    void Awake()
    {
        _character = GetComponent<Character>();
    }

    public bool IsSkillUnlocked(Skill skill)
    {
        return _unlockedSkills.Contains(skill);
    }

    public bool CanUnlock(Skill skill)
    {
        if (skillPoints < skill.skillPointCost) return false;
        if (IsSkillUnlocked(skill)) return false;

        foreach (var prereq in skill.prerequisites)
        {
            if (!_unlockedSkills.Contains(prereq))
            {
                return false;
            }
        }
        return true;
    }

    public bool TryUnlockSkill(Skill skill)
    {
        if (!CanUnlock(skill)) return false;

        skillPoints -= skill.skillPointCost;
        _unlockedSkills.Add(skill);

        // Apply all of the skill's effects
        foreach (var effect in skill.effects)
        {
            effect.Apply(gameObject);
        }

        Debug.Log($"Unlocked skill: {skill.skillName} for {gameObject.name}");
        return true;
    }
}
```

## 5. Integration with Existing Systems

*   **Character/Novamina:** The `CharacterSkillTree` component will be added to the `Novamina` prefabs. It will get the `Character` component on the same object to apply stat modifications.
*   **Combat System:** `UnlockAbilityEffect` will need to add `Ability` assets to a list managed by the character, which the `CombatManager` can then use. This may require adding an `unlockedAbilities` list to the `Character` class.
*   **UI:** The Skill Tree UI will read data from the `CharacterSkillTree` component. It will check `CanUnlock` and `IsSkillUnlocked` to determine the visual state of skill nodes (e.g., available, locked, unlocked) and will call `TryUnlockSkill` when the player clicks an unlock button.

## 6. Example Workflow

1.  **Create Skill Effects:** A developer creates a new `SkillEffect` script (e.g., `ModifyDamageTypeEffect`) inheriting from `SkillEffect`.
2.  **Create Effect Asset:** A designer right-clicks in the Project window (`Create > Milehigh > Skill Effects > Modify Damage Type`) to create an instance of that effect and configures its properties (e.g., "Increase Fire Damage by 10%").
3.  **Create a Skill:** The designer creates a new `Skill` asset (`Create > Milehigh > Skill`). They name it "Fire Mastery," assign an icon, link prerequisites, and add the `Modify Damage Type` effect asset to its `effects` list.
4.  **Add Skill to Tree:** The designer opens the `SkillTree` asset for a character and adds the new "Fire Mastery" `Skill` to its list of skills.
5.  **Assign Tree to Character:** The designer ensures the character's prefab has a `CharacterSkillTree` component and its `Skill Tree Asset` field references the correct `SkillTree`.
6.  **In-Game:** The player earns skill points, navigates the UI, and calls `TryUnlockSkill("Fire Mastery")`. The `CharacterSkillTree` component validates the request, deducts the points, and applies the `ModifyDamageTypeEffect`, which in turn modifies the character's combat stats.