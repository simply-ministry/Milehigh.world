using UnityEngine;

/// <summary>
/// An abstract base class for all objects in the game world that the player can interact with.
/// Provides the basic structure for interaction prompts and triggers.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    /// <summary>
    /// The message displayed to the player when they are in range to interact with this object.
    /// </summary>
    public string promptMessage = "[E] Interact";

    /// <summary>
    /// A public-facing method that can be called by other scripts (like an Interactor) to trigger the interaction.
    /// This serves as a wrapper for the protected Interact method.
    /// </summary>
    public void BaseInteract()
    {
        Interact();
    }

    /// <summary>
    /// The core interaction logic. This method must be implemented by any class that inherits from Interactable.
    /// It defines what happens when the player interacts with this object.
    /// </summary>
    protected abstract void Interact();
}