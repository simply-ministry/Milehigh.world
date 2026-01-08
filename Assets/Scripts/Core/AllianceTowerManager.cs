using UnityEngine;

/// <summary>
/// Manages the state and events for the Alliance Tower hub scene.
/// This script orchestrates interactions with key objects like teleporters,
/// NPCs, and launchpads. Implemented as a singleton for easy access.
/// </summary>
public class AllianceTowerManager : MonoBehaviour
{
    // Singleton instance
    public static AllianceTowerManager Instance { get; private set; }

    [Header("Manager State")]
    [Tooltip("A description of the current state of the tower's events.")]
    public string managerState = "Idle";

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Handles the player using the main teleporter.
    /// In a full game, this would likely trigger a scene change or open a world map.
    /// </summary>
    public void UseTeleporter()
    {
        managerState = "Teleporter Activated";
        Debug.Log("AllianceTowerManager: Teleporter has been used. Initiating warp sequence...");
        // Example: UnityEngine.SceneManagement.SceneManager.LoadScene("WorldMap");
    }

    /// <summary>
    /// Triggers a dialogue sequence with an NPC.
    /// </summary>
    /// <param name="npcName">The name of the NPC.</param>
    /// <param name="dialogue">The line of dialogue to display.</param>
    public void TriggerNPCDialogue(string npcName, string dialogue)
    {
        managerState = $"In Dialogue with {npcName}";
        Debug.Log($"AllianceTowerManager: [{npcName}] says: '{dialogue}'");
        // Example: UIManager.Instance.ShowDialogueBox(npcName, dialogue);
    }

    /// <summary>
    /// Handles the player using the launchpad.
    /// This could initiate a minigame or a special travel sequence.
    /// </summary>
    public void UseLaunchpad()
    {
        managerState = "Launchpad Activated";
        Debug.Log("AllianceTowerManager: Launchpad activated. Preparing for takeoff!");
        // Example: StartCoroutine(LaunchpadCutscene());
    }
}