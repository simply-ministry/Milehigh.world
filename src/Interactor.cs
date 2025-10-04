// ~~~~~~~~~~~~~ SCRIPT 1: Interactor.cs ~~~~~~~~~~~~~
// Attach this script to your Player GameObject. No changes needed here.
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI References (Optional)")]
    public Text interactionPromptText;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("Interactor Error: No camera found!");
            }
        }
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hitInfo;
        bool hitInteractable = false;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                hitInteractable = true;
                if (interactionPromptText != null)
                {
                    interactionPromptText.text = interactable.promptMessage;
                    interactionPromptText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(interactionKey))
                {
                    interactable.BaseInteract();
                }
            }
        }

        if (!hitInteractable && interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }
}