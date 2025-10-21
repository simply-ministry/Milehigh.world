// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Manages a character's experience points (XP) and leveling.
/// This component should be attached to any character that can gain experience.
/// </summary>
public class ExperienceHandler : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField]
    [Tooltip("The current experience points of the character.")]
    private int currentXP = 0;

    [Tooltip("The current level of the character.")]
    [SerializeField]
    private int currentLevel = 1;

    // Public properties to access private fields safely
    public int CurrentXP => currentXP;
    public int CurrentLevel => currentLevel;

    // Simple event to notify other systems when the level changes.
    public event System.Action<int> OnLevelUp;

    /// <summary>
    /// Adds a specified amount of XP to the character.
    /// </summary>
    /// <param name="amount">The amount of XP to add.</param>
    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;
        Debug.Log($"{gameObject.name} gained {amount} XP. Total XP: {currentXP}.");

        // In a real game, you would check against an XP curve to level up.
        // For now, we'll use a simple placeholder logic.
        // For example, level up every 1000 XP.
        if (currentXP >= currentLevel * 1000)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Handles the logic for leveling up the character.
    /// </summary>
    private void LevelUp()
    {
        currentLevel++;
        currentXP = 0; // Reset XP for the new level or carry over the remainder
        Debug.Log($"{gameObject.name} has reached Level {currentLevel}!");

        // Notify subscribers that a level up occurred.
        OnLevelUp?.Invoke(currentLevel);

        // Here you would typically increase character stats.
    }
}