using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a character's abilities, including cooldowns and resource costs.
/// This component should be attached to any character that can use abilities.
/// </summary>
public class AbilitySystem : MonoBehaviour
{
    [Tooltip("A list of abilities that this character possesses.")]
    public List<Ability> abilities;
    private Dictionary<Ability, float> abilityCooldowns = new Dictionary<Ability, float>();
    private Character character;

    /// <summary>
    /// Initializes the ability system, getting a reference to the character component
    /// and setting up the cooldown dictionary.
    /// </summary>
    void Awake()
    {
        character = GetComponent<Character>();
        // Initialize cooldowns
        foreach (var ability in abilities)
        {
            abilityCooldowns[ability] = 0f;
        }
    }

    /// <summary>
    /// Called every frame. Updates all active ability cooldowns.
    /// </summary>
    void Update()
    {
        // Reduce all active cooldowns over time
        var keys = new List<Ability>(abilityCooldowns.Keys);
        foreach (var ability in keys)
        {
            if (abilityCooldowns[ability] > 0)
            {
                abilityCooldowns[ability] -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Attempts to use an ability from the character's ability list.
    /// </summary>
    /// <param name="abilityIndex">The index of the ability to use in the `abilities` list.</param>
    /// <param name="target">The target of the ability.</param>
    public void UseAbility(int abilityIndex, Character target)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Count) return;

        Ability ability = abilities[abilityIndex];

        // Check if ability is off cooldown and character has enough mana
        if (abilityCooldowns[ability] <= 0 && character.mana >= ability.resourceCost)
        {
            character.mana -= ability.resourceCost;
            ability.Use(character, target);
            // Put the ability on cooldown
            abilityCooldowns[ability] = ability.cooldownDuration;
            Debug.Log($"{ability.abilityName} is now on cooldown for {ability.cooldownDuration}s.");
        }
        else
        {
            Debug.Log($"Cannot use {ability.abilityName}. Either on cooldown or not enough mana.");
        }
    }
}