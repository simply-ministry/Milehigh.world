using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the combo system for all characters in the game.
/// It tracks consecutive hits, calculates damage multipliers, and handles combo resets.
/// This class is implemented as a singleton.
/// </summary>
public class ComboManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static ComboManager Instance { get; private set; }

    [Header("Combo Configuration")]
    [Tooltip("The time in seconds a player has to land another hit before the combo resets.")]
    [SerializeField] private float comboResetTime = 2.0f;

    [Tooltip("The damage multiplier for each successive hit in a combo. The index corresponds to the hit number (e.g., index 0 is the 2nd hit).")]
    [SerializeField] private float[] damageMultipliers = { 1.1f, 1.25f, 1.5f, 2.0f }; // Example: 2nd hit, 3rd, 4th, 5th+

    // --- State ---
    private Dictionary<int, ComboState> characterComboStates = new Dictionary<int, ComboState>();

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
    /// Registers an attack from a specific character, updating their combo state.
    /// </summary>
    /// <param name="character">The character who performed the attack.</param>
    public void RegisterAttack(Character character)
    {
        int charId = character.GetInstanceID();
        if (!characterComboStates.ContainsKey(charId))
        {
            characterComboStates[charId] = new ComboState();
        }

        ComboState state = characterComboStates[charId];

        // If a reset coroutine is running, stop it because a new hit has landed.
        if (state.ResetCoroutine != null)
        {
            StopCoroutine(state.ResetCoroutine);
        }

        // Update the combo state
        state.HitCount++;
        state.LastHitTime = Time.time;
        Debug.Log($"{character.characterName} is on a {state.HitCount}-hit combo!");

        // Start a new coroutine to reset the combo after the window expires.
        state.ResetCoroutine = StartCoroutine(ComboResetTimer(charId));
    }

    /// <summary>
    /// Gets the current damage multiplier for a character based on their combo count.
    /// </summary>
    /// <param name="character">The character whose multiplier is requested.</param>
    /// <returns>The damage multiplier. Returns 1.0 if not in a combo.</returns>
    public float GetDamageMultiplier(Character character)
    {
        int charId = character.GetInstanceID();
        if (!characterComboStates.ContainsKey(charId) || characterComboStates[charId].HitCount <= 1)
        {
            return 1.0f; // No multiplier for the first hit
        }

        ComboState state = characterComboStates[charId];
        int multiplierIndex = state.HitCount - 2; // -1 for count -> index, -1 because 2nd hit is first multiplier

        if (multiplierIndex < 0) return 1.0f;

        // Use the last defined multiplier if the combo exceeds the array size
        if (multiplierIndex >= damageMultipliers.Length)
        {
            multiplierIndex = damageMultipliers.Length - 1;
        }

        return damageMultipliers[multiplierIndex];
    }

    /// <summary>
    /// Coroutine that waits for the combo window to expire and then resets the combo.
    /// </summary>
    private IEnumerator ComboResetTimer(int characterId)
    {
        yield return new WaitForSeconds(comboResetTime);

        // If the combo state still exists, reset it.
        if (characterComboStates.ContainsKey(characterId))
        {
            Debug.Log($"Combo for character {characterId} has reset.");
            characterComboStates.Remove(characterId);
        }
    }
}