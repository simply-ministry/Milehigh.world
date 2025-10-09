using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides centralized damage calculation logic. This class is a singleton.
/// </summary>
public class CombatManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static CombatManager Instance { get; private set; }

    // --- Damage Calculation ---
    /// <summary>
    /// Defines the various formulas that can be used for damage calculation.
    /// </summary>
    public enum DamageFormula
    {
        Linear,
        RatioBased,
        PercentageReduction
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Calculates the final damage of an attack. This is a static utility function
    /// that can be called from anywhere without needing an instance of CombatManager.
    /// </summary>
    public static int CalculateDamage(Character attacker, Character defender, Ability ability, DamageFormula formula = DamageFormula.Linear, float customMultiplier = 1.0f)
    {
        // 1. Determine base power and check for critical hits
        bool isCrit = Random.value < ability.critChance;
        float critModifier = isCrit ? ability.critMultiplier : 1.0f;
        int attackPower = ability.power;

        // 2. Calculate pre-mitigation damage using the selected formula
        int defense = defender.defense;
        int baseDamage = 0;
        switch (formula)
        {
            case DamageFormula.Linear:
                baseDamage = attacker.attack + attackPower;
                break;
            case DamageFormula.RatioBased:
                baseDamage = Mathf.RoundToInt((float)(attacker.attack + attackPower) / (1 + (float)defense / 50));
                break;
            case DamageFormula.PercentageReduction:
                 float reduction = (float)defense / (defense + 100);
                 baseDamage = Mathf.RoundToInt((attacker.attack + attackPower) * (1 - reduction));
                break;
        }

        // 3. Apply critical hit and custom multipliers
        int finalDamage = Mathf.RoundToInt(baseDamage * critModifier * customMultiplier);

        if (isCrit)
        {
            Debug.Log("CRITICAL HIT!");
        }

        return finalDamage;
    }

    // The old turn-based logic has been removed.
    // The manager can be expanded with methods to check for combat state,
    // like checking if all enemies or players are defeated.
}