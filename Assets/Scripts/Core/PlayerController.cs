using UnityEngine;

/// <summary>
/// Handles player input and controls the player character's movement and actions.
/// This component should be attached to the main player GameObject.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the character moves.")]
    public float movementSpeed = 5.0f;

    [Header("References")]
    [Tooltip("The main camera used for calculating movement direction.")]
    public Transform cameraTransform;

    // Component references
    private CharacterController characterController;
    private Character character;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the required components attached to this GameObject.
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();

        // Find the main camera if not assigned.
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (characterController == null)
        {
            Debug.LogError("PlayerController requires a CharacterController component.");
        }
        if (character == null)
        {
            Debug.LogError("PlayerController requires a Character component.");
        }
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        // Don't process input if the game is paused or in a cutscene
        // if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
        // {
        //     return;
        // }

        HandleMovement();
        HandleActionInput();
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
    /// Handles input for actions like attacking.
    /// Interaction is handled by the separate Interactor.cs script.
    /// </summary>
    private void HandleActionInput()
    {
        // Check for the primary mouse button (left-click) to attack.
        if (Input.GetMouseButtonDown(0))
        {
            // In the future, this will trigger an attack animation and call the CombatManager.
            // For now, we just log a message.
            Debug.Log($"{character.characterName} performs a basic attack!");

            // Example of how it might eventually work:
            // Find a target in front of the player
            // Character target = FindTarget();
            // if (target != null)
            // {
            //     // Get a default or equipped ability
            //     Ability basicAttack = GetBasicAttackAbility();
            //     CombatManager.Instance.PlayerAction(character, target, basicAttack);
            // }
        }
    }
}