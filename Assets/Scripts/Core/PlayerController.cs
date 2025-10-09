using UnityEngine;

/// <summary>
/// Handles player input and controls the player character's movement and actions.
/// This component should be attached to the main player GameObject.
/// It requires Character, CharacterController, and AbilitySystem components.
/// </summary>
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystem))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the character moves.")]
    public float movementSpeed = 5.0f;

    [Header("References")]
    [Tooltip("The main camera used for calculating movement direction. If not set, it will be found automatically.")]
    public Transform cameraTransform;

    [Header("Targeting")]
    [Tooltip("The currently selected target for abilities.")]
    private Character currentTarget;

    // Component references
    private CharacterController characterController;
    private Character character;
    private AbilitySystem abilitySystem;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the required components attached to this GameObject.
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();
        abilitySystem = GetComponent<AbilitySystem>();

        // Find the main camera if not assigned.
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("PlayerController: Main camera not found. Please assign the cameraTransform reference.");
                this.enabled = false; // Disable the script if no camera is found.
                return;
            }
        }

        if (characterController == null) Debug.LogError("PlayerController requires a CharacterController component.");
        if (character == null) Debug.LogError("PlayerController requires a Character component.");
        if (abilitySystem == null) Debug.LogError("PlayerController requires an AbilitySystem component.");
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        // Don't process input if the game is not in the 'Playing' state.
        if (GameManager.Instance == null || GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing)
        {
            return;
        }

        HandleMovement();
        HandleTargeting();
        HandleAbilityInput();
    }

    /// <summary>
    /// Handles the character's movement based on player input.
    /// </summary>
    private void HandleMovement()
    {
        // Get input from the horizontal and vertical axes (e.g., WASD or joystick).
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the camera.
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movementDirection = (cameraForward * verticalInput + cameraTransform.right * horizontalInput).normalized;

        // Apply movement speed.
        Vector3 moveVector = movementDirection * movementSpeed;

        // Apply gravity. The CharacterController.isGrounded check is important.
        if (!characterController.isGrounded)
        {
            moveVector.y += Physics.gravity.y * Time.deltaTime;
        }

        // Move the character.
        characterController.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Handles selecting a target with the mouse.
    /// </summary>
    private void HandleTargeting()
    {
        // Use raycasting to select a target with the mouse
        if (Input.GetMouseButtonDown(0)) // Left-click to target
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is an enemy.
                if (hit.collider.CompareTag("Enemy"))
                {
                    Character targetCharacter = hit.collider.GetComponent<Character>();
                    if (targetCharacter != null)
                    {
                        currentTarget = targetCharacter;
                        Debug.Log($"Target set to: {currentTarget.characterName}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles input for using abilities.
    /// </summary>
    private void HandleAbilityInput()
    {
        // Trigger abilities with key presses (e.g., '1', '2', '3')
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentTarget != null)
            {
                abilitySystem.UseAbility(0, currentTarget); // Use the first ability in the list
            }
            else
            {
                Debug.Log("No target selected to use ability on.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentTarget != null)
            {
                abilitySystem.UseAbility(1, currentTarget); // Use the second ability
            }
             else
            {
                Debug.Log("No target selected to use ability on.");
            }
        }
        // Add more keybindings for other abilities as needed.
    }
}