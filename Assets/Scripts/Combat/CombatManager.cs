// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the state and flow of a turn-based combat encounter.
/// This class is a singleton.
/// </summary>
public class CombatManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static CombatManager Instance { get; private set; }

    // --- Combat State ---
    private List<Character> combatants = new List<Character>();
    private List<Character> playerPartyCache = new List<Character>(); // Cache for post-combat cleanup
    private int currentTurnIndex = 0;
    private bool isCombatActive = false;
    private bool isPlayerTurn = false;

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

        playerPartyCache.Clear();
        playerPartyCache.AddRange(playerParty);

        combatants.Clear();
        combatants.AddRange(playerParty);
        combatants.AddRange(enemyParty);

        // Optional: Sort combatants by a 'speed' attribute here if desired.
        // combatants = combatants.OrderByDescending(c => c.speed).ToList();

        currentTurnIndex = 0;
        isCombatActive = true;
        Debug.Log("===== COMBAT STARTED =====");

        // Setup the player's targeting system
        foreach (var player in playerParty)
        {
            var targetingSystem = player.GetComponent<TargetingSystem>();
            if (targetingSystem != null)
            {
                targetingSystem.ClearTargets();
                foreach (var enemy in enemyParty)
                {
                    targetingSystem.AddTarget(enemy);
                }
            }
        }

        // Initialize the combat UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.InitializeCombatUI(playerParty, enemyParty);
        }

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

                    // Display action buttons for the current player
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.DisplayPlayerActions(currentCharacter);
                    }

                    // The Combat Loop will now wait here until the player has made a move.
                    // The PlayerAction() method will set isPlayerTurn to false.
                    yield return new WaitUntil(() => !isPlayerTurn);
                }
                else // This is an AI's turn
                {
                    var aiController = currentCharacter.GetComponent<AIController>();
                    if (aiController != null)
                    {
                        // AI determines its action and executes it.
                        aiController.TakeTurn();
                    }
                    yield return new WaitForSeconds(1f); // A brief pause after the AI's move.
                }
            }

            // After a turn, check if any character's health dropped to zero.
            // We iterate backwards because we might remove items from the list.
            for (int i = combatants.Count - 1; i >= 0; i--)
            {
                if (!combatants[i].isAlive)
                {
                    HandleCharacterDeath(combatants[i]);
                }
            }

            if (CheckForEndOfCombat())
            {
                EndCombat();
                yield break; // Exit the coroutine.
            }

            // Move to the next combatant in the turn order.
            // We need to ensure the index is valid after potential removals.
            currentTurnIndex++;
            if (currentTurnIndex >= combatants.Count)
            {
                currentTurnIndex = 0;
            }
        }
    }

    /// <summary>
    /// Called by external components (like PlayerController or UI buttons) to execute a player's action.
    /// </summary>
    /// <param name="player">The character performing the action.</param>
    /// <param name="target">The target of the action.</param>
    /// <param name="ability">The ability being used.</param>
    public void PlayerAction(Character player, Character target, Ability ability)
    {
        if (!isPlayerTurn || player != combatants[currentTurnIndex] || ability == null) return;

        Debug.Log($"{player.characterName} uses {ability.abilityName} on {target.characterName}.");
        // The ability itself handles mana cost, damage calculation, etc.
        ability.Use(player, target);
        isPlayerTurn = false; // Signal that the player's turn is over.
    }

    /// <summary>
    /// Checks if all members of one party have been defeated.
    /// </summary>
    private bool CheckForEndOfCombat()
    {
        bool allPlayersDefeated = combatants.Where(c => c.CompareTag("Player")).All(c => !c.isAlive);
        bool allEnemiesDefeated = combatants.Where(c => c.CompareTag("Enemy")).All(c => !c.isAlive);

        if(allPlayersDefeated) Debug.Log("All players have been defeated.");
        if(allEnemiesDefeated) Debug.Log("All enemies have been defeated.");

        return allPlayersDefeated || allEnemiesDefeated;
    }

    /// <summary>
    /// Cleans up the combat state.
    /// </summary>
    private void EndCombat()
    {
        isCombatActive = false;
        Debug.Log("===== COMBAT ENDED =====");

        // Clear targets from the player's targeting system using the cached party list
        foreach (var player in playerPartyCache)
        {
            if (player != null) // The player's GameObject might have been destroyed
            {
                var targetingSystem = player.GetComponent<TargetingSystem>();
                if (targetingSystem != null)
                {
                    targetingSystem.ClearTargets();
                }
            }
        }
        playerPartyCache.Clear(); // Clean up the cache
        // Here you could add logic for awarding XP, loot, or loading a game over screen.
    }

    /// <summary>
    /// Handles the removal of a defeated character from combat.
    /// </summary>
    private void HandleCharacterDeath(Character deadCharacter)
    {
        Debug.Log($"{deadCharacter.characterName} has been defeated.");

        // Remove the character from every player's targeting system.
        var playerParty = combatants.Where(c => c.CompareTag("Player"));
        foreach (var player in playerParty)
        {
            var targetingSystem = player.GetComponent<TargetingSystem>();
            if (targetingSystem != null)
            {
                targetingSystem.RemoveTarget(deadCharacter);
            }
        }

        // Remove from the main combatant list.
        // This needs to be done carefully to not mess up the turn order index.
        int deadCharacterIndex = combatants.IndexOf(deadCharacter);
        if (deadCharacterIndex != -1)
        {
            combatants.RemoveAt(deadCharacterIndex);
            // If the dead character was earlier in the turn order than the current one,
            // we need to decrement the index to not skip the next person's turn.
            if (deadCharacterIndex < currentTurnIndex)
            {
                currentTurnIndex--;
            }
        }
    }

    /// <summary>
    /// Gets a read-only list of all combatants.
    /// </summary>
    public List<Character> GetCombatants()
    {
        return new List<Character>(combatants);
    }

    /// <summary>
    /// Checks if combat is currently active.
    /// </summary>
    public bool IsCombatActive()
    {
        return isCombatActive;
    }
}