using UnityEngine;

public class AllianceTowerManager : MonoBehaviour
{
    void Start()
    {
        // Programmatically create the Mascot GameObject to ensure it exists in the scene.
        CreateMascot();
    }

    // Creates the Mascot character in the game world.
    private void CreateMascot()
    {
        // 1. Create a new GameObject named "Mascot"
        GameObject mascotGO = new GameObject("Mascot");

        // 2. Add the Mascot script to it. This makes it interactable.
        mascotGO.AddComponent<Mascot>();

        // 3. Add a collider so the player's raycast can hit it.
        mascotGO.AddComponent<BoxCollider>();

        // 4. Set its position in the world (e.g., in front of the tower)
        mascotGO.transform.position = new Vector3(0, 1, 5);

        Debug.Log("Mascot character has been spawned in the world.");
    }

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