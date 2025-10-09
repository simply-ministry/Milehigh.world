using UnityEngine;

/// <summary>
/// Represents a launchpad that the player can interact with.
/// This class triggers a launch event via the AllianceTowerManager when interacted with.
/// </summary>
public class InteractableLaunchpad : Interactable
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
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = "[E] Use Launchpad";
    }

    /// <summary>
    /// Overrides the base Interact method to define the launchpad's specific action.
    /// It calls the UseLaunchpad method on the AllianceTowerManager.
    /// </summary>
    protected override void Interact()
    {
        if (towerManager != null)
        {
            towerManager.UseLaunchpad();
        }
        else
        {
            Debug.LogError("AllianceTowerManager not found in the scene!");
        }
    }
}