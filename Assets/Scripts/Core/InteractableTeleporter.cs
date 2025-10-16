using UnityEngine;

/// <summary>
/// An interactable component for teleporters. When the player interacts with this object,
/// it calls the UseTeleporter method on the AllianceTowerManager singleton.
/// </summary>
public class InteractableTeleporter : Interactable
{
    private void Start()
    {
        promptMessage = "[E] Use Teleporter";
    }

    protected override void Interact()
    {
        if (AllianceTowerManager.Instance != null)
        {
            AllianceTowerManager.Instance.UseTeleporter();
        }
        else
        {
            Debug.LogError("AllianceTowerManager singleton instance not found in the scene!");
        }
    }
}