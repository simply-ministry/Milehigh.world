using UnityEngine;

public class AllianceTowerManager : MonoBehaviour
{
    public void UseTeleporter()
    {
        Debug.Log("Teleporter activated!");
        // Future teleporter logic will go here.
    }

    public void TriggerNPCDialogue(string npcName, string dialogue)
    {
        Debug.Log($"Started dialogue with {npcName}: {dialogue}");
        // Future dialogue system logic will go here.
        // Placeholder for teleporter logic
        Debug.Log("Teleporter activated!");
    }

    public void TriggerNPCDialogue(string name, string dialogue)
    {
        // Placeholder for NPC dialogue logic
        Debug.Log($"Started dialogue with {name}: {dialogue}");
    }

    public void UseLaunchpad()
    {
        Debug.Log("Launchpad activated!");
        // Future launchpad logic will go here.
        // Placeholder for launchpad logic
        Debug.Log("Launchpad activated!");
    }
}