// ~~~~~~~~~~~~~ SCRIPT 2: Interactable.cs ~~~~~~~~~~~~~
// The abstract base class. No changes needed here.
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public string promptMessage = "[E] Interact";

    public void BaseInteract()
    {
        Interact();
    }

    protected abstract void Interact();
}