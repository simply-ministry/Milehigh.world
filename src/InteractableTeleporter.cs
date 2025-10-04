using UnityEngine;

public class InteractableTeleporter : Interactable
{
    private AllianceTowerManager towerManager;

    void Start()
    {
        // Find the scene's manager script.
        // NOTE: FindObjectOfType can be slow. If you have one manager, making it a singleton is more efficient.
        towerManager = FindObjectOfType<AllianceTowerManager>();
        promptMessage = "[E] Use Teleporter";
    }

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