using UnityEngine;

// This script defines the behavior of the Mascot character when interacted with.
// It inherits from the abstract Interactable class and overrides the Interact method.
public class Mascot : Interactable
{
    // This method is called when the player interacts with the Mascot.
    // It logs a message to the console to indicate that the interaction was successful.
    protected override void Interact()
    {
        Debug.Log("Interacted with the Mascot! It purrs and stares with its three eyes.");
    }
}