using UnityEngine;

/// <summary>
/// Manages key interactions and programmatic setup for the Alliance Tower scene.
/// This class is responsible for spawning characters and handling events like teleporting and dialogue.
/// </summary>
public class AllianceTowerManager : MonoBehaviour
{
    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures that essential characters, like the Mascot, are created in the scene.
    /// </summary>
    void Start()
    {
        // Programmatically create the Mascot GameObject to ensure it exists in the scene.
        CreateMascot();
    }

    /// <summary>
    /// Creates and configures the Mascot character in the game world.
    /// This method sets up the Mascot's components and initial position.
    /// </summary>
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

    /// <summary>
    /// Placeholder method for activating a teleporter.
    /// </summary>
    public void UseTeleporter()
    {
        Debug.Log("Teleporter activated!");
        // Future teleporter logic will go here.
    }

    /// <summary>
    /// Placeholder method for triggering dialogue with an NPC.
    /// </summary>
    /// <param name="npcName">The name of the NPC to start a conversation with.</param>
    /// <param name="dialogue">The line of dialogue to be displayed.</param>
    public void TriggerNPCDialogue(string npcName, string dialogue)
    {
        Debug.Log($"Started dialogue with {npcName}: {dialogue}");
        // Future dialogue system logic will go here.
    }

    /// <summary>
    /// Placeholder method for activating a launchpad.
    /// </summary>
    public void UseLaunchpad()
    {
        Debug.Log("Launchpad activated!");
        // Future launchpad logic will go here.
    }
}