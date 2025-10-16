using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A singleton manager for handling all quest-related logic.
/// It tracks the state of all quests, and provides methods to start,
/// advance, and complete them.
/// </summary>
public class QuestManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static QuestManager Instance { get; private set; }

    // --- Quest Tracking ---
    // A dictionary to store the current state of every quest instance.
    private Dictionary<Quest, QuestState> questStates = new Dictionary<Quest, QuestState>();

    // Public properties for other systems to query quest states.
    public IReadOnlyDictionary<Quest, QuestState> QuestStates => questStates;
    public List<Quest> InProgressQuests => questStates.Where(q => q.Value == QuestState.InProgress).Select(q => q.Key).ToList();
    public List<Quest> CompletedQuests => questStates.Where(q => q.Value == QuestState.Completed).Select(q => q.Key).ToList();

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
            DontDestroyOnLoad(gameObject); // Make the QuestManager persistent across scenes
        }
    }

    /// <summary>
    /// Starts a new quest if it has not been started before.
    /// </summary>
    /// <param name="quest">The quest to start.</param>
    public void StartQuest(Quest quest)
    {
        if (quest == null) return;

        if (!questStates.ContainsKey(quest))
        {
            questStates.Add(quest, QuestState.InProgress);
            Debug.Log($"[QuestManager] Quest Started: '{quest.questName}'");
            // Here you would typically trigger a UI update to show the new quest.
        }
        else
        {
            Debug.LogWarning($"[QuestManager] Tried to start quest '{quest.questName}' which already has a state ({questStates[quest]}).");
        }
    }

    /// <summary>
    /// Completes a quest, granting the player rewards.
    /// </summary>
    /// <param name="quest">The quest to complete.</param>
    /// <param name="playerCharacter">The character (player) to receive the rewards.</param>
    public void CompleteQuest(Quest quest, Character playerCharacter)
    {
        if (quest == null) return;

        if (questStates.ContainsKey(quest) && questStates[quest] == QuestState.InProgress)
        {
            questStates[quest] = QuestState.Completed;
            Debug.Log($"[QuestManager] Quest Completed: '{quest.questName}'");

            // Grant rewards
            Debug.Log($"[QuestManager] Rewarding {playerCharacter.characterName}: {quest.experienceReward} XP.");
            playerCharacter.GetComponent<ExperienceHandler>()?.AddXP(quest.experienceReward);

            InventorySystem playerInventory = playerCharacter.GetComponent<InventorySystem>();
            if (playerInventory != null && quest.itemRewards != null)
            {
                foreach (var item in quest.itemRewards)
                {
                    playerInventory.AddItem(item);
                    Debug.Log($"[QuestManager] Rewarded item: {item.itemName}.");
                }
            }

            // Trigger UI update for quest completion.
        }
        else
        {
            Debug.LogWarning($"[QuestManager] Tried to complete quest '{quest.questName}' that was not in progress.");
        }
    }

    /// <summary>
    /// Gets the current state of a specific quest.
    /// </summary>
    /// <param name="quest">The quest to check.</param>
    /// <returns>The current state of the quest, or NotStarted if it's not being tracked.</returns>
    public QuestState GetQuestState(Quest quest)
    {
        return questStates.TryGetValue(quest, out QuestState state) ? state : QuestState.NotStarted;
    }
}