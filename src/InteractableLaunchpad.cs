using UnityEngine;

public class InteractableLaunchpad : Interactable
{
    private AllianceTowerManager towerManager;

    void Start()
    {
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = "[E] Use Launchpad";
    }

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