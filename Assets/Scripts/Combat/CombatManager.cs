using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides centralized damage calculation logic. This class is a singleton.
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

    private bool isPlayerTurn = false;

    void Awake()
    void Awake()
/// Manages the state and flow of a turn-based combat encounter, and provides
/// centralized damage calculation logic. This class is a singleton.
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