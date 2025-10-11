using UnityEngine;

/// <summary>
/// Handles player input and controls the player character's movement and actions.
/// This component should be attached to the main player GameObject.
/// It requires Character, CharacterController, AbilitySystem, and Interactor components.
/// </summary>
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystem))]
[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(TargetingSystem))]
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
    public Character CurrentTarget { get; private set; }

    // Public method to allow other systems (like TargetingSystem) to set the target.
    public void SetTarget(Character newTarget)
    {
        CurrentTarget = newTarget;
    }

    // Component references
    private CharacterController characterController;
    private Character character;
    private AbilitySystem abilitySystem;
    private Interactor interactor;
    private TargetingSystem targetingSystem;

    void Awake()
    {
        // Get the required components attached to this GameObject.
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
            Debug.LogError("PlayerController: Main camera not found. Please assign the cameraTransform reference.");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        // If combat is active, movement and interaction are typically disabled.
        if (CombatManager.Instance != null && CombatManager.Instance.IsCombatActive())
        {
            // In combat, we only handle combat-related inputs.
            HandleTargetSelection();
            HandleCombatInput();
        }
        else
        {
            // If not in combat, handle world exploration inputs.
            HandleMovement();
            interactor.CheckForInteractable(); // Let the interactor look for things.
            HandleInteractionInput();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movementDirection = (cameraForward * verticalInput + cameraTransform.right * horizontalInput).normalized;

        Vector3 moveVector = movementDirection * movementSpeed;

        if (!characterController.isGrounded)
        {
            moveVector.y += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Handles input for interacting with the environment.
    /// </summary>
    private void HandleInteractionInput()
    {
        // Default interaction key is 'E', handled by the Interactor component.
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactor.TryInteract();
        }
    }

    /// <summary>
    /// Handles cycling through targets using the Tab key.
    /// </summary>
    private void HandleTargetSelection()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            targetingSystem.CycleTarget();
        }
    }

    /// <summary>
    /// Handles all combat-related input, like basic attacks and abilities.
    /// </summary>
    private void HandleCombatInput()
    {
        // Basic attack with left-click
        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentTarget != null && abilitySystem.abilities.Count > 0)
            {
                // Use the first ability as the "basic attack"
                CombatManager.Instance.PlayerAction(character, CurrentTarget, abilitySystem.abilities[0]);
            }
            else
            {
                Debug.Log("Select a target first (right-click) before attacking.");
            }
        }

        // Trigger abilities with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryUseAbility(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryUseAbility(2);
        }
        // Add more keybindings for other abilities as needed.
    }

    /// <summary>
    /// Helper function to use a specific ability from the ability system.
    /// </summary>
    private void TryUseAbility(int abilityIndex)
    {
        if (CurrentTarget == null)
        {
            Debug.Log("No target selected to use ability on.");
            return;
        }
        if (abilityIndex < 0 || abilityIndex >= abilitySystem.abilities.Count)
        {
            Debug.Log($"No ability found at index {abilityIndex}.");
            return;
        }

        CombatManager.Instance.PlayerAction(character, CurrentTarget, abilitySystem.abilities[abilityIndex]);
    }
}