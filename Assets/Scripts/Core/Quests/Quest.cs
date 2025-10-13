using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Defines the possible states of a quest.
/// </summary>
public enum QuestState
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

/// <summary>
/// A ScriptableObject that defines the properties and objectives of a quest.
/// Create instances of this in the Unity Editor to define all the quests in the game.
/// </summary>
[CreateAssetMenu(fileName = "New Quest", menuName = "Milehigh.World/Quests/Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Information")]
    [Tooltip("A unique identifier for this quest (e.g., 'MAIN_01', 'SIDE_URBAN_CORE_02').")]
    public string questID;

    [Tooltip("The name of the quest as it appears in the UI.")]
    public string questName = "New Quest";

    [TextArea(3, 10)]
    [Tooltip("A detailed description of the quest's story and objectives.")]
    public string description = "Quest Description";

    [Header("Quest Rewards")]
    [Tooltip("The amount of experience points awarded upon completion.")]
    public int experienceReward;

    [Tooltip("A list of items given to the player when the quest is completed.")]
    public List<Item> itemRewards;

    // --- Internal State ---
    // This state would be managed by the QuestManager for each player.
    // This is just the definition of the quest, not its live instance.
}