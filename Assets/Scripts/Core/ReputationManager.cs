using UnityEngine;
using System.Collections.Generic;
using Milehigh.World.Core;

/// <summary>
/// A singleton manager for tracking the player's reputation with various factions.
/// </summary>
public class ReputationManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static ReputationManager Instance { get; private set; }

    // --- Reputation Data ---
    // A dictionary to store reputation values for each faction.
    private Dictionary<Faction, int> reputationData = new Dictionary<Faction, int>();

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeReputations();
        }
    }

    /// <summary>
    /// Initializes all factions with a starting reputation of 0.
    /// </summary>
    private void InitializeReputations()
    {
        foreach (Faction faction in System.Enum.GetValues(typeof(Faction)))
        {
            if (faction != Faction.None && !reputationData.ContainsKey(faction))
            {
                reputationData.Add(faction, 0);
            }
        }
    }

    /// <summary>
    /// Modifies the player's reputation with a specific faction.
    /// </summary>
    /// <param name="faction">The faction to modify reputation with.</param>
    /// <param name="amount">The amount to add or remove (if negative).</param>
    public void AddReputation(Faction faction, int amount)
    {
        if (faction == Faction.None) return;

        if (reputationData.ContainsKey(faction))
        {
            reputationData[faction] += amount;
            Debug.Log($"[ReputationManager] Reputation with {faction} changed by {amount}. New value: {reputationData[faction]}");
            // Here you would typically trigger an event for UI updates.
        }
        else
        {
            Debug.LogWarning($"[ReputationManager] Tried to modify reputation for an untracked faction: {faction}");
        }
    }

    /// <summary>
    /// Retrieves the current reputation value for a specific faction.
    /// </summary>
    /// <param name="faction">The faction to check.</param>
    /// <returns>The current reputation value, or 0 if the faction is not tracked.</returns>
    public int GetReputation(Faction faction)
    {
        return reputationData.TryGetValue(faction, out int value) ? value : 0;
    }
}
