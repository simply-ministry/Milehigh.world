using UnityEngine;

/// <summary>
/// Represents a teleporter pad that the player can interact with.
/// This class triggers a teleport event via the AllianceTowerManager when interacted with.
/// </summary>
public class InteractableTeleporter : Interactable
{
    /// <summary>
    /// A reference to the central manager for the tower scene.
    /// </summary>
    private AllianceTowerManager towerManager;

    /// <summary>
    /// Initializes the component by finding the AllianceTowerManager in the scene
    /// and setting a custom prompt message for the interaction.
    /// </summary>
    void Start()
    {
        // Find the scene's manager script.
        // NOTE: FindObjectOfType can be slow. If you have one manager, making it a singleton is more efficient.
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = "[E] Use Teleporter";
    }

    /// <summary>
    /// Overrides the base Interact method to define the teleporter's specific action.
    /// It calls the UseTeleporter method on the AllianceTowerManager.
    /// </summary>
    protected override void Interact()
    {
        if (towerManager != null)
        {
            Debug.Log("InteractableTeleporter is calling AllianceTowerManager.UseTeleporter()");
            towerManager.UseTeleporter();
        }
        else
        {
            Debug.LogError("AllianceTowerManager not found in the scene!");
        }
    }
}