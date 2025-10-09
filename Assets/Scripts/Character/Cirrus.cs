using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// An enum to represent Cirrus's current form.
/// </summary>
public enum FormState
{
    Humanoid,
    Dragon
}

/// <summary>
/// Cirrus the Dragon King (The Primal Scion)
/// </summary>
public class Cirrus : Novamina
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// A reference to Cirrus's father from another dimension, Cyrus.
    /// This relationship is a source of major ideological and physical conflict.
    /// </summary>
    public Guid FathersId { get; set; }

    /// <summary>
    /// Represents Cirrus's current physical form, which he can change in battle.
    /// </summary>
    [field: SerializeField]
    public FormState CurrentForm { get; private set; }

    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource representing Cirrus's authority and control over the battlefield.
    /// It builds slowly over time and is expended on his most powerful abilities.
    /// </summary>
    [field: SerializeField]
    public float Sovereignty { get; private set; }

    /// <summary>
    /// The maximum amount of Sovereignty Cirrus can command.
    /// </summary>
    public float MaxSovereignty { get; private set; } = 100f;

    protected override void Awake()
    {
        base.Awake();
        characterName = "Cirrus the Dragon King";
        Archetype = "Elemental Bruiser / Area Control";

        // As a Dragon King, he is exceptionally powerful and durable.
        maxHealth = 250;
        currentHealth = maxHealth;

        // Starts in his humanoid form.
        CurrentForm = FormState.Humanoid;
        Sovereignty = 0f;
    }

    // --- Core Mechanic (Transformation) ---

    /// <summary>
    /// A powerful ultimate ability that transforms Cirrus into his true dragon form.
    /// This requires full Sovereignty and dramatically alters his other abilities.
    /// </summary>
    public void AssumeDragonForm()
    {
        if (Sovereignty >= MaxSovereignty)
        {
            Sovereignty = 0; // The transformation consumes all Sovereignty.
            CurrentForm = FormState.Dragon;
            Debug.Log($"{characterName} unleashes his true power, transforming into a magnificent dragon!");
            // In a real game, this would change his character model, stats, and ability set.
        }
    }

    /// <summary>
    /// Reverts Cirrus back to his humanoid form after a duration or when triggered.
    /// </summary>
    public void RevertToHumanoidForm()
    {
        if (CurrentForm == FormState.Dragon)
        {
            CurrentForm = FormState.Humanoid;
            Debug.Log($"{characterName} returns to his humanoid form.");
        }
    }

    // --- Abilities (Methods) ---

    /// <summary>
    /// A commanding ability that marks a target. Attacks against the marked target
    /// from allies are more effective.
    /// </summary>
    /// <param name="target">The enemy to mark.</param>
    public void KingsDecree(Character target)
    {
        // This ability's effect changes based on his form.
        switch (CurrentForm)
        {
            case FormState.Humanoid:
                Debug.Log($"{characterName}, in his kingly form, issues a decree against {target.characterName}, marking them as a priority target!");
                // In-game logic: apply a "vulnerability" debuff to the target.
                break;
            case FormState.Dragon:
                Debug.Log($"{characterName}, in his dragon form, roars a challenge at {target.characterName}, terrifying them!");
                // In-game logic: apply a "fear" or "defense reduction" debuff to the target.
                break;
        }
    }

    /// <summary>
    /// A powerful breath attack that differs based on his current form.
    /// </summary>
    /// <param name="enemiesInPath">A list of all enemies in a line in front of Cirrus.</param>
    public void DraconicBreath(List<Character> enemiesInPath)
    {
        // This ability is likely only usable or is much stronger in Dragon form.
        if (CurrentForm == FormState.Dragon)
        {
            Debug.Log($"{characterName} unleashes a torrent of dragon fire!");
            foreach (var enemy in enemiesInPath)
            {
                Debug.Log($"...The fire engulfs {enemy.characterName}!");
                // ... logic to apply heavy fire damage to all enemies in the cone ...
            }
        }
        else
        {
             // In humanoid form, it could be a lesser version.
            Debug.Log($"{characterName} exhales a spark of draconic energy at the nearest foe.");
        }
    }
}