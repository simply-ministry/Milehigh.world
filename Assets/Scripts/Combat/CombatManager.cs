using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the state and flow of a turn-based combat encounter.
/// </summary>
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private List<Character> combatants = new List<Character>();
    private int currentTurnIndex = 0;
    private bool isCombatActive = false;

    void Awake()
/// Manages the state and flow of a turn-based combat encounter, and provides
/// centralized damage calculation logic. This class is a singleton.
/// </summary>
public class CombatManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static CombatManager Instance { get; private set; }

    // --- State Management ---
    private List<Character> combatants = new List<Character>();
    private int currentTurnIndex = 0;
    private bool isCombatActive = false;
    private bool isPlayerTurn = false;

    // --- Damage Calculation ---
    /// <summary>
    /// Defines the various formulas that can be used for damage calculation.
    /// </summary>
    public enum DamageFormula
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
    /// Starts a new combat encounter with the given parties.
    /// </summary>
    /// <param name="playerParty">List of player-controlled characters.</param>
    /// <param name="enemyParty">List of AI-controlled characters.</param>
    public void StartCombat(List<Character> playerParty, List<Character> enemyParty)
    {
        if (isCombatActive) return;

        combatants.Clear();
        combatants.AddRange(playerParty);
        combatants.AddRange(enemyParty);
        // Optional: Sort combatants by a speed or initiative stat here

        currentTurnIndex = 0;
        isCombatActive = true;
        Debug.Log("===== COMBAT STARTED =====");
        StartCoroutine(CombatLoop());
    }

    /// <summary>
    /// The main loop that processes turns until combat ends.
    /// </summary>
    private IEnumerator CombatLoop()
    {
        while (isCombatActive)
        {
            Character currentCharacter = combatants[currentTurnIndex];

            if (currentCharacter.isAlive)
            {
                Debug.Log($"--- {currentCharacter.characterName}'s Turn ---");

                if (currentCharacter.CompareTag("Player"))
                {
                    isPlayerTurn = true;
                    // Wait here until the player performs an action
                    yield return new WaitUntil(() => !isPlayerTurn);
                }
                else // AI's turn
                {
                    // Placeholder for AI logic. In a real game, this would call an AIController.
                    Debug.Log($"{currentCharacter.characterName} is thinking...");
                    yield return new WaitForSeconds(1f); // Simulate AI thinking time
                }
            }

            if (CheckForEndOfCombat())
            {
                EndCombat();
                yield break; // Exit the coroutine
            }

            // Move to the next combatant
            currentTurnIndex = (currentTurnIndex + 1) % combatants.Count;
        }
    }

    /// <summary>
    /// Called by UI or player input handlers to perform an action.
    /// </summary>
    /// <param name="player">The player character performing the action.</param>
    /// <param name="target">The target of the action.</param>
    /// <param name="ability">The ability to use.</param>
    public void PlayerAction(Character player, Character target, Ability ability)
    {
        if (!isPlayerTurn || player != combatants[currentTurnIndex]) return;

        Debug.Log($"{player.characterName} uses {ability.abilityName} on {target.characterName}.");
        player.PerformAttack(target, ability); // Assumes PerformAttack handles damage calculation
        isPlayerTurn = false; // Signal that the player's turn is over
    }

    /// <summary>
    /// Checks if the combat should end (one side is defeated).
    /// </summary>
    private bool CheckForEndOfCombat()
    {
        bool allPlayersDefeated = combatants.Where(c => c.CompareTag("Player")).All(c => !c.isAlive);
        bool allEnemiesDefeated = combatants.Where(c => c.CompareTag("Enemy")).All(c => !c.isAlive);

        if (allPlayersDefeated) Debug.Log("All players have been defeated. GAME OVER.");
        if (allEnemiesDefeated) Debug.Log("All enemies have been defeated. VICTORY!");

        return allPlayersDefeated || allEnemiesDefeated;
    }

    /// <summary>
    /// Cleans up and marks combat as inactive.
    /// </summary>
    private void EndCombat()
    {
        isCombatActive = false;
        Debug.Log("===== COMBAT ENDED =====");
        // Add any additional cleanup logic here (e.g., awarding XP, loot).
    }

    /// <summary>
    /// Gets a read-only list of all combatants.
    /// </summary>
    /// <returns>A new list containing all characters currently in combat.</returns>
    public List<Character> GetCombatants()
    {
        return new List<Character>(combatants);
    }

    /// <summary>
    /// Starts a new combat encounter.
    /// </summary>
    /// <param name="playerParty">List of player-controlled characters.</param>
    /// <param name="enemyParty">List of AI-controlled characters.</param>
    public void StartCombat(List<Character> playerParty, List<Character> enemyParty)
    {
        if (isCombatActive) return;

        combatants.Clear();
        combatants.AddRange(playerParty);
        combatants.AddRange(enemyParty);

        // Simple turn order: sort by a 'speed' stat if it exists, otherwise default order.
        // combatants = combatants.OrderByDescending(c => c.speed).ToList();

        currentTurnIndex = 0;
        isCombatActive = true;
        Debug.Log("===== COMBAT STARTED =====");
        StartCoroutine(CombatLoop());
    }

    private IEnumerator CombatLoop()
    {
        while (isCombatActive)
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
            Character currentCharacter = combatants[currentTurnIndex];

            if (currentCharacter.isAlive)
            {
                Debug.Log($"--- {currentCharacter.characterName}'s Turn ---");

                // If this is a player character, wait for player input.
                // If it's an AI, execute its turn.
                // For now, we'll just log the turn and advance.
                // A full implementation would involve a state machine to wait for actions.
                yield return new WaitForSeconds(1f); // Placeholder for action duration
            }

            // Check for victory/defeat conditions
            if (CheckForEndOfCombat())
            {
                EndCombat();
                yield break; // Exit the loop
            }

            // Advance to the next turn
            currentTurnIndex = (currentTurnIndex + 1) % combatants.Count;
        }
    }

    /// <summary>
    /// Checks if the combat has ended (i.e., one party has been defeated).
    /// </summary>
    /// <returns>True if combat should end, false otherwise.</returns>
    private bool CheckForEndOfCombat()
    {
        // Example logic:
        bool playersAlive = combatants.Any(c => c.isPlayer && c.isAlive);
        bool enemiesAlive = combatants.Any(c => !c.isPlayer && c.isAlive);

        return !playersAlive || !enemiesAlive;
    }

    /// <summary>
    /// Cleans up and ends the current combat encounter.
    /// </summary>
    private void EndCombat()
    {
        isCombatActive = false;
        bool playersWon = combatants.Any(c => c.isPlayer && c.isAlive);
        if (playersWon)
        {
            Debug.Log("===== COMBAT ENDED: VICTORY! =====");
        }
        else
        {
            Debug.Log("===== COMBAT ENDED: DEFEAT! =====");
        }
        // Here you would typically transition back to the main game state,
        // award XP, drops, etc.
    }
}