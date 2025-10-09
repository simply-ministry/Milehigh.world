using UnityEngine;

/// <summary>
/// Represents a non-player character (NPC) that the player can interact with to trigger dialogue.
/// This class holds the NPC's name and dialogue content.
/// </summary>
public class InteractableNPC : Interactable
{
    [Header("NPC Dialogue")]
    /// <summary>
    /// The name of the NPC, which can be displayed in the UI.
    /// </summary>
    public string npcName = "Mysterious Stranger";
    /// <summary>
    /// The line of dialogue the NPC will say when interacted with.
    /// </summary>
    [TextArea(3, 10)] // Makes the string field bigger in the inspector
    public string dialogue = "Hello, traveler. The Void calls, doesn't it?";

    /// <summary>
    /// A reference to the central manager for the tower scene.
    /// </summary>
    private AllianceTowerManager towerManager;

    /// <summary>
    /// Initializes the component by finding the AllianceTowerManager and setting a custom prompt message.
    /// </summary>
    void Start()
    {
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = $"[E] Talk to {npcName}";
    }

    /// <summary>
    /// Overrides the base Interact method to define the NPC's specific action.
    /// It calls the TriggerNPCDialogue method on the AllianceTowerManager.
    /// </summary>
    protected override void Interact()
    {
        if (towerManager != null)
        {
            Debug.Log($"Interacting with {npcName}.");
            towerManager.TriggerNPCDialogue(npcName, dialogue);
        }
        else
        {
            Debug.LogError("AllianceTowerManager not found in the scene!");
        }
    }
}