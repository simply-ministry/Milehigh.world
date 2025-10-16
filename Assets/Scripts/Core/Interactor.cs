using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows the player to interact with objects in the world that have an Interactable component.
/// This script should be attached to the player GameObject. It is controlled by the PlayerController.
/// </summary>
public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("The distance from the player within which interactions are possible.")]
    public float interactionDistance = 3f;

    [Tooltip("The UI Text element that displays the interaction prompt.")]
    public Text interactionPromptText;

    private Camera playerCamera;
    private Interactable currentInteractable;

    void Awake()
    {
        playerCamera = Camera.main;
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Interaction Prompt Text is not assigned in the Inspector.");
        }
    }

    /// <summary>
    /// This method is called by an external controller (like PlayerController) every frame
    /// to check for interactable objects in view.
    /// </summary>
    public void CheckForInteractable()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            var interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetInteractable(interactable);
            }
            else
            {
                ClearInteractable();
            }
        }
        else
        {
            ClearInteractable();
        }
    }

    /// <summary>
    /// This method is called by an external controller to attempt an interaction.
    /// </summary>
    public void TryInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.BaseInteract();
        }
    }

    private void SetInteractable(Interactable newInteractable)
    {
        if (currentInteractable == newInteractable) return;

        currentInteractable = newInteractable;
        if (interactionPromptText != null)
        {
            interactionPromptText.text = currentInteractable.promptMessage;
            interactionPromptText.gameObject.SetActive(true);
        }
    }

    private void ClearInteractable()
    {
        if (currentInteractable == null) return;

        currentInteractable = null;
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }
}