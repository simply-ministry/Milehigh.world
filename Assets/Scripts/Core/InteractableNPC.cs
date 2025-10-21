using UnityEngine;

/// <summary>
/// An interactable component for NPCs. Allows the player to talk to an NPC,
/// triggering a dialogue event in the AllianceTowerManager.
/// </summary>
public class InteractableNPC : Interactable
{
    [Header("NPC Dialogue")]
    [Tooltip("The name of the NPC, displayed in the interaction prompt.")]
    public string npcName = "Mysterious Stranger";

    [Tooltip("The dialogue the NPC will say when interacted with.")]
    [TextArea(3, 10)]
    public string dialogue = "Hello, traveler. The Void calls, doesn't it?";

    /// <summary>
    /// Called when the script instance is being loaded. Sets a custom prompt message using the NPC's name.
    /// </summary>
    private void Start()
    {
        promptMessage = $"[E] Talk to {npcName}";
    }

    /// <summary>
    /// Defines the interaction logic for the NPC.
    /// </summary>
    protected override void Interact()
    {
        if (AllianceTowerManager.Instance != null)
        {
            AllianceTowerManager.Instance.TriggerNPCDialogue(npcName, dialogue);
        }
        else
        {
            Debug.LogError("AllianceTowerManager singleton instance not found in the scene!");
        }
    }
}
