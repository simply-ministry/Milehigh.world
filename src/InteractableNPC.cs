// ~~~~~~~~~~~~~ NEW SCRIPT 4: InteractableNPC.cs ~~~~~~~~~~~~~
// Attach this to your NPC models. You can set their name and dialogue in the Inspector.
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using UnityEngine;

public class InteractableNPC : Interactable
{
    [Header("NPC Dialogue")]
    public string npcName = "Mysterious Stranger";
    [TextArea(3, 10)] // Makes the string field bigger in the inspector
    public string dialogue = "Hello, traveler. The Void calls, doesn't it?";

    private AllianceTowerManager towerManager;

    void Start()
    {
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = $"[E] Talk to {npcName}";
    }

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