// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages all enemy characters in the scene.
/// Provides a centralized way to access and manipulate enemies.
/// This class is a singleton.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static EnemyManager Instance { get; private set; }

    // --- Enemy Tracking ---
    private List<Character> enemies = new List<Character>();

    void Awake()
    {
        // --- Singleton Setup ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // --- Find and Register Enemies ---
        // Find all active GameObjects with the "Enemy" tag.
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            Character enemyCharacter = enemyObject.GetComponent<Character>();
            if (enemyCharacter != null)
            {
                enemies.Add(enemyCharacter);
            }
            else
            {
                Debug.LogWarning($"EnemyManager: GameObject '{enemyObject.name}' has 'Enemy' tag but no Character component.");
            }
        }

        Debug.Log($"EnemyManager: Found and registered {enemies.Count} enemies.");
    }

    /// <summary>
    /// Gets a read-only list of all registered enemies.
    /// </summary>
    public IReadOnlyList<Character> GetAllEnemies()
    {
        return enemies.AsReadOnly();
    }

    /// <summary>
    /// Sets the corporeal state of all registered enemies.
    /// This affects their vulnerability, AI, and appearance.
    /// </summary>
    /// <param name="isCorporeal">True to make enemies solid and active, false to make them ethereal and passive.</param>
    public void SetEnemiesCorporeal(bool isCorporeal)
    {
        foreach (Character enemy in enemies)
        {
            if (enemy == null) continue;

            // Set the corporeal state on the Character script
            enemy.isCorporeal = isCorporeal;

            // Enable or disable the AI
            AIController aiController = enemy.GetComponent<AIController>();
            if (aiController != null)
            {
                aiController.enabled = isCorporeal;
            }

            // Adjust the sprite's alpha for a visual cue
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = isCorporeal ? 1.0f : 0.5f; // Full alpha when corporeal, half when not
                spriteRenderer.color = color;
            }
        }

        Debug.Log($"All enemies have been set to {(isCorporeal ? "Corporeal" : "Incorporeal")}.");
    }
}