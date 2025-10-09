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

    // A new variable to check if the Combat Manager is waiting for player input
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

                if (currentCharacter.CompareTag("Player"))
                {
                    isPlayerTurn = true;
                    // The Combat Loop will now wait until the player has made a move
                    yield return new WaitUntil(() => !isPlayerTurn);
                }
                else // This is an AI's turn
                {
                    // This assumes you have an AIController attached to your enemy prefabs
                    var aiController = currentCharacter.GetComponent<AIController>();
                    if (aiController != null)
                    {
                        aiController.TakeTurn();
                    }
                    yield return new WaitForSeconds(1f); // Wait a moment after the AI's move
                }
            }

            if (CheckForEndOfCombat())
            {
                EndCombat();
                yield break;
            }

            currentTurnIndex = (currentTurnIndex + 1) % combatants.Count;
        }
    }

    // This new public method will be called by the UI Buttons
    public void PlayerAction(Character player, Character target, Ability ability)
    {
        if (!isPlayerTurn || player != combatants[currentTurnIndex]) return;

        ability.Use(player, target);
        isPlayerTurn = false; // The player's turn is now over
    }

    private bool CheckForEndOfCombat()
    {
        bool allPlayersDefeated = combatants.Where(c => c.CompareTag("Player")).All(c => !c.isAlive);
        bool allEnemiesDefeated = combatants.Where(c => c.CompareTag("Enemy")).All(c => !c.isAlive);

        return allPlayersDefeated || allEnemiesDefeated;
    }

    private void EndCombat()
    {
        isCombatActive = false;
        Debug.Log("===== COMBAT ENDED =====");
    }

    /// <summary>
    /// Gets a list of all combatants.
    /// </summary>
    /// <returns>A new list containing all characters currently in combat.</returns>
    public List<Character> GetCombatants()
    {
        return combatants.ToList();
    }
}