using UnityEngine;

/// <summary>
/// A static class to manage combat-related calculations,
/// centralizing damage formulas and other combat logic.
/// </summary>
public static class CombatManager
{
    /// <summary>
    /// Defines the various formulas that can be used for damage calculation.
    /// </summary>
    public enum DamageFormula
    {
        Linear,
        RatioBased,
        PercentageReduction,
        Additive,
        Exponential
    }

    /// <summary>
    /// Calculates the final damage of an attack, considering the ability used,
    /// critical hits, a selected damage formula, and the defender's resistances.
    /// </summary>
    /// <param name="attacker">The character performing the attack.</param>
    /// <param name="defender">The character receiving the attack.</param>
    /// <param name="ability">The ability being used.</param>
    /// <param name="formula">The core damage formula to apply.</param>
    /// <param name="customMultiplier">A custom multiplier to apply to the final damage, for special abilities.</param>
    /// <returns>The final calculated damage amount.</returns>
    public static int CalculateDamage(Character attacker, Character defender, Ability ability, DamageFormula formula = DamageFormula.Linear, float customMultiplier = 1.0f)
    {
        // 1. Determine base power and check for critical hits
        bool isCrit = Random.value < ability.critChance;
        float critModifier = isCrit ? ability.critMultiplier : 1.0f;
        int attackPower = ability.power; // Use the ability's power as the base

        // 2. Calculate pre-mitigation damage using the selected formula
        int defense = defender.defense; // General defense
        int baseDamage = 0;
        switch (formula)
        {
            case DamageFormula.Linear:
                baseDamage = attackPower - defense;
                break;
            case DamageFormula.RatioBased:
                baseDamage = (defense > 0) ? (attackPower * attackPower) / defense : attackPower;
                break;
            case DamageFormula.PercentageReduction:
                float reduction = (float)defense / (defense + 100);
                baseDamage = Mathf.RoundToInt(attackPower * (1 - reduction));
                break;
            case DamageFormula.Additive:
                const int c = 50;
                baseDamage = Mathf.RoundToInt(c * (float)attackPower / (c + defense));
                break;
            case DamageFormula.Exponential:
                baseDamage = Mathf.RoundToInt(Mathf.Pow(1.1f, attackPower - defense));
                break;
        }

        // Ensure base damage is non-negative before applying multipliers
        baseDamage = Mathf.Max(0, baseDamage);

        // 3. Apply critical hit multiplier
        float damageAfterCrit = baseDamage * critModifier;
        if (isCrit) Debug.Log("Critical Hit!");

        // 4. Apply specific resistance to the ability's damage type
        int resistance = defender.GetResistanceValue(ability.damageType);
        float finalDamage = damageAfterCrit - resistance;

        // 5. Apply custom damage multiplier for special abilities
        finalDamage *= customMultiplier;

        // 6. Return final damage, ensuring it's at least 0
        return Mathf.Max(0, Mathf.RoundToInt(finalDamage));
    }
}