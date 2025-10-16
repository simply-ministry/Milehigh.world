using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the state of all quests in the game. It tracks active quests,
/// checks for objective completion, and grants rewards. This is a singleton.
/// </summary>
public class QuestManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the QuestManager.
    /// </summary>
    public static QuestManager Instance { get; private set; }

    [Header("Quest Tracking")]
    [Tooltip("A list of all currently active quests.")]
    private List<Quest> activeQuests = new List<Quest>();

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Make the QuestManager persist across scenes
        }
    }

    /// <summary>
    /// Starts a new quest.
    /// </summary>
    /// <param name="quest">The quest to begin.</param>
    public void StartQuest(Quest quest)
    {
        if (quest != null && !activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            quest.status = QuestStatus.InProgress;
            Debug.Log($"Quest Started: {quest.questName}");
            // TODO: Add quest to a UI panel
        }
    }

    /// <summary>
    /// Completes a quest and gives the player rewards.
    /// </summary>
    /// <param name="quest">The quest to complete.</param>
    public void CompleteQuest(Quest quest)
    {
        if (quest != null && activeQuests.Contains(quest))
        {
            quest.status = QuestStatus.Completed;
            activeQuests.Remove(quest);
            Debug.Log($"Quest Completed: {quest.questName}");
            // TODO: Grant rewards (XP, items, etc.)
        }
    }

    // Example of a method that might be called when an enemy is defeated
    public void OnEnemyDefeated(string enemyID)
    {
        foreach (var quest in activeQuests)
        {
            quest.UpdateProgress(enemyID);
        }
    }
}