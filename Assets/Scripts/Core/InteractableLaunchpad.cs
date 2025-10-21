// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// An interactable component for launchpads. When the player interacts with this object,
/// it calls the UseLaunchpad method on the AllianceTowerManager singleton.
/// </summary>
public class InteractableLaunchpad : Interactable
{
    private void Start()
    {
        promptMessage = "[E] Use Launchpad";
    }

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