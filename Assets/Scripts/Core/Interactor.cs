using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows the player to interact with objects in the world that have an Interactable component.
/// This script should be attached to the player GameObject.
/// </summary>
public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")]
    /// <summary>
    /// The distance from the player within which interactions are possible.
    /// </summary>
    [Tooltip("The distance from the player within which interactions are possible.")]
    public float interactionDistance = 3f;

    /// <summary>
    /// The key used to trigger an interaction.
    /// </summary>
    [Tooltip("The key used to trigger an interaction.")]
    public KeyCode interactionKey = KeyCode.E;

    /// <summary>
    /// The UI Text element that displays the interaction prompt.
    /// </summary>
    [Tooltip("The UI Text element that displays the interaction prompt.")]
    public Text interactionPromptText;

    private Camera playerCamera;
    private Interactable currentInteractable;

    /// <summary>
    /// Initializes the Interactor by finding the main camera and hiding the interaction prompt.
    /// </summary>
    void Start()
    {
        // Find the main camera in the scene.
        playerCamera = Camera.main;

        // Ensure the interaction prompt is hidden at the start.
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
    /// Handles the interaction logic each frame.
    /// </summary>
    void Update()
    {
        // Continuously check for interactable objects in front of the player.
        HandleInteractionCheck();

        // Check if the player presses the interaction key while an interactable object is in range.
        HandleInteractionInput();
    }

    /// <summary>
    /// Casts a ray from the camera to detect interactable objects.
    /// </summary>
    private void HandleInteractionCheck()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not set.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo;

        // Perform the raycast.
        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            // Check if the object hit has an Interactable component.
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // An interactable object is in range.
                SetInteractable(interactable);
            }
            else
            {
                // The object in range is not interactable.
                ClearInteractable();
            }
        }
        else
        {
            // Nothing is in range.
            ClearInteractable();
        }
    }

    /// <summary>
    /// Handles the player's input for interaction.
    /// </summary>
    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            // Trigger the interaction.
            currentInteractable.BaseInteract();
        }
    }

    /// <summary>
    /// Sets the current interactable object and displays the interaction prompt.
    /// </summary>
    /// <param name="newInteractable">The new interactable object to focus on.</param>
    private void SetInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        if (interactionPromptText != null)
        {
            interactionPromptText.text = newInteractable.promptMessage;
            interactionPromptText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Clears the current interactable object and hides the interaction prompt.
    /// </summary>
    private void ClearInteractable()
    {
        currentInteractable = null;
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }
}