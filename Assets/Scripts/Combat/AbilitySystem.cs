using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public List<Ability> abilities;
    private Dictionary<Ability, float> abilityCooldowns = new Dictionary<Ability, float>();
    private Character character;

    void Awake()
    {
        character = GetComponent<Character>();
        // Initialize cooldowns
        foreach (var ability in abilities)
        {
            abilityCooldowns[ability] = 0f;
        }
    }

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