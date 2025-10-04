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
    }

    public void UseLaunchpad()
    {
        Debug.Log("Launchpad activated!");
        // Future launchpad logic will go here.
    }
}