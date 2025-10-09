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
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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