using UnityEngine;

/// <summary>
/// An interactable component for launchpads. When the player interacts with this object,
/// it calls the UseLaunchpad method on the AllianceTowerManager singleton.
/// </summary>
public class InteractableLaunchpad : Interactable
{
    /// <summary>
    /// Called when the script instance is being loaded. Sets the specific prompt message for the launchpad.
    /// </summary>
    private void Start()
    {
        promptMessage = "[E] Use Launchpad";
    }

    /// <summary>
    /// Defines the interaction logic for the launchpad.
    /// </summary>
    protected override void Interact()
    {
        if (AllianceTowerManager.Instance != null)
        {
            AllianceTowerManager.Instance.UseLaunchpad();
        }
        else
        {
            Debug.LogError("AllianceTowerManager singleton instance not found in the scene!");
        }
    }
}
