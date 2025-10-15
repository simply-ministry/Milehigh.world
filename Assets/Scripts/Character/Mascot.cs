using UnityEngine;

/// <summary>
/// Defines the behavior of the Mascot character when interacted with.
/// This class provides a specific implementation for an interactable object.
/// </summary>
public class Mascot : Interactable
{
    /// <summary>
    /// Overrides the base Interact method to define the Mascot's specific interaction.
    /// When called, it logs a message to the console indicating the interaction occurred.
    /// </summary>
    protected override void Interact()
    {
        Debug.Log("Interacted with the Mascot! It purrs and stares with its three eyes.");
    }
}