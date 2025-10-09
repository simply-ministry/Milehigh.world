using UnityEngine;

/// <summary>
/// A test script to demonstrate and verify the combat system.
/// This script creates two characters and has one attack the other
/// to test damage calculations, abilities, and resistances.
/// </summary>
public class CombatTest : MonoBehaviour
{
    /// <summary>
    /// Sets up and executes a combat scenario when the script starts.
    /// This method creates an attacker, a defender with specific resistances,
    /// and an ability, then simulates an attack to test the combat calculations.
    /// </summary>
    void Start()
    {
        // --- Test Setup ---

        // 1. Create Attacker (Hero)
        GameObject heroGO = new GameObject("Hero");
        Character hero = heroGO.AddComponent<Character>();
        hero.characterName = "Sir Gideon";
        hero.attack = 60; // General attack stat (used by some formulas)
        hero.defense = 30;

        // 2. Create Defender (Goblin)
        GameObject goblinGO = new GameObject("Goblin");
        Character goblin = goblinGO.AddComponent<Character>();
        goblin.characterName = "Grimgut";
        goblin.defense = 20;
        // Give the goblin a resistance to fire
        goblin.resistances.Add(new DamageTypeResistance { damageType = DamageType.Fire, resistanceValue = 15 });
        // Manually call Awake to initialize the resistance map for this test
        goblin.SendMessage("Awake");


        // 3. Create an Ability (Fireball)
        Ability fireball = ScriptableObject.CreateInstance<Ability>();
        fireball.abilityName = "Fireball";
        fireball.power = 40; // Base power of the spell
        fireball.damageType = DamageType.Fire;
        fireball.critChance = 0.2f; // 20% crit chance for testing
        fireball.critMultiplier = 2.5f;


        // --- Combat Execution ---
        Debug.Log("--- Starting Combat Test ---");
        Debug.Log($"{hero.characterName} HP: {hero.currentHealth}, {goblin.characterName} HP: {goblin.currentHealth}");
        Debug.Log($"{goblin.characterName} has {goblin.GetResistanceValue(DamageType.Fire)} resistance to Fire.");

        // Hero attacks Goblin with Fireball using the PercentageReduction formula
        hero.PerformAttack(goblin, fireball, CombatManager.DamageFormula.PercentageReduction);

        Debug.Log("--- Combat Test Finished ---");
    }
}