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

        combatants.Clear();
        combatants.AddRange(playerParty);
        combatants.AddRange(enemyParty);

        // Optional: Sort combatants by a 'speed' attribute here if desired.
        // combatants = combatants.OrderByDescending(c => c.speed).ToList();

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

            if (CheckForEndOfCombat())
            {
                EndCombat();
                yield break; // Exit the coroutine.
            }

            // Move to the next combatant in the turn order.
            currentTurnIndex = (currentTurnIndex + 1) % combatants.Count;
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
        // Here you could add logic for awarding XP, loot, or loading a game over screen.
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