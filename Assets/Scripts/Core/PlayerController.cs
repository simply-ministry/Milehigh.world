using UnityEngine;

/// <summary>
/// Handles player input for character movement, interaction, and combat actions.
/// This component serves as the central hub for player control, delegating tasks
/// to other components like the <see cref="CharacterController"/> for movement,
/// <see cref="AbilitySystem"/> for combat skills, and <see cref="Interactor"/> for world interactions.
/// It requires several other components to be attached to the same GameObject to function correctly.
/// </summary>
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystem))]
[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(TargetingSystem))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the character moves in meters per second.")]
    public float movementSpeed = 5.0f;

    [Header("References")]
    [Tooltip("The main camera used for calculating camera-relative movement. If not set, it will be found automatically.")]
    public Transform cameraTransform;

    [Header("Targeting")]
    [Tooltip("The currently selected target for abilities.")]
    public Character CurrentTarget { get; private set; }

    // --- Component References ---
    private CharacterController characterController;
    private Character character;
    private AbilitySystem abilitySystem;
    private Interactor interactor;
    private TargetingSystem targetingSystem;

    /// <summary>
    /// Caches required component references and finds the main camera.
    /// </summary>
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();
        abilitySystem = GetComponent<AbilitySystem>();
        interactor = GetComponent<Interactor>();
        targetingSystem = GetComponent<TargetingSystem>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("PlayerController: Main camera not found. Please ensure your main camera is tagged 'MainCamera'.");
            this.enabled = false; // Disable the script if no camera is found.
        }
    }

    /// <summary>
    // Called every frame. Determines whether to handle combat or exploration input
    // based on the current combat state from the CombatManager.
    /// </summary>
    void Update()
    {
        if (CombatManager.Instance != null && CombatManager.Instance.IsCombatActive())
        {
            HandleCombatInput();
        }
        else
        {
            HandleExplorationInput();
        }
    }

    /// <summary>
    /// Groups together all input handlers for non-combat (exploration) states.
    /// </summary>
    private void HandleExplorationInput()
    {
        HandleMovement();
        interactor.CheckForInteractable(); // Let the interactor look for things.
        HandleInteractionInput();
    }

    /// <summary>
    /// Handles keyboard input for character movement based on the camera's orientation.
    /// </summary>
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera, ignoring vertical tilt.
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movementDirection = (cameraForward * verticalInput + cameraTransform.right * horizontalInput).normalized;

        Vector3 moveVector = movementDirection * movementSpeed;

        // Apply gravity if the character is not grounded.
        if (!characterController.isGrounded)
        {
            moveVector.y += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Handles input for interacting with objects in the environment.
    /// </summary>
    private void HandleInteractionInput()
    {
        // The 'E' key is a common standard for interaction.
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactor.TryInteract();
        }
    }

    /// <summary>
    /// Groups together all input handlers for combat states.
    /// </summary>
    private void HandleCombatInput()
    {
        HandleTargetSelection();
        HandleAbilityInput();
    }

    /// <summary>
    /// Handles cycling through available targets using the Tab key.
    /// </summary>
    private void HandleTargetSelection()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            targetingSystem.CycleTarget();
        }
    }

    /// <summary>
    /// Handles input for using abilities and basic attacks during combat.
    /// </summary>
    private void HandleAbilityInput()
    {
        // Basic attack with left mouse button (assumes it's the first ability).
        if (Input.GetMouseButtonDown(0))
        {
            TryUseAbility(0); // Use the ability at index 0 as the basic attack.
        }

        // Use abilities with number keys 1 and 2.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryUseAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryUseAbility(2);
        }
        // Additional abilities can be mapped to other keys here.
    }

    /// <summary>
    /// Attempts to use an ability from the AbilitySystem by its index.
    /// Checks for a valid target and ability before requesting the action from the CombatManager.
    /// </summary>
    /// <param name="abilityIndex">The index of the ability in the AbilitySystem's list.</param>
    private void TryUseAbility(int abilityIndex)
    {
        CurrentTarget = targetingSystem.GetCurrentTarget();
        if (CurrentTarget == null)
        {
            Debug.Log("No target selected to use ability on.");
            return;
        }
        if (abilitySystem.abilities == null || abilityIndex < 0 || abilityIndex >= abilitySystem.abilities.Count)
        {
            Debug.Log($"No ability found at index {abilityIndex}.");
            return;
        }

        // Request the CombatManager to execute the action.
        CombatManager.Instance.PlayerAction(character, CurrentTarget, abilitySystem.abilities[abilityIndex]);
    }

    /// <summary>
    /// Public method to allow other systems (like the <see cref="TargetingSystem"/>) to set the player's target.
    /// </summary>
    /// <param name="newTarget">The new character to set as the target.</param>
    public void SetTarget(Character newTarget)
    {
        CurrentTarget = newTarget;
    }
}