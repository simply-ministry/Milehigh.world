using UnityEngine;

/// <summary>
/// Manages player movement, including walking, sprinting, jumping, and gravity.
/// It also includes a basic stamina system for sprinting.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // The CharacterController component is used for player movement and collision.
    // It's a common way to handle player movement in Unity without directly manipulating Rigidbody physics.
    private CharacterController controller;

    // --- Movement Variables ---
    /// <summary>The speed at which the player walks.</summary>
    public float walkSpeed = 5f;
    /// <summary>The speed at which the player sprints.</summary>
    public float sprintSpeed = 8f;
    private float currentSpeed;

    // --- Gravity and Jumping ---
    /// <summary>The force of gravity applied to the player.</summary>
    public float gravity = -9.81f;
    /// <summary>The height the player can jump.</summary>
    public float jumpHeight = 2f;
    private Vector3 velocity;

    // --- Ground Check ---
    /// <summary>A transform representing the point from which to check for the ground.</summary>
    public Transform groundCheck;
    /// <summary>The radius of the sphere for ground checking.</summary>
    public float groundDistance = 0.4f;
    /// <summary>The layer mask to detect as ground.</summary>
    public LayerMask groundMask;
    private bool isGrounded;

    // --- Stamina System ---
    /// <summary>The maximum amount of stamina the player has.</summary>
    public float maxStamina = 100f;
    /// <summary>The rate at which stamina is drained while sprinting.</summary>
    public float staminaDrainRate = 20f;
    /// <summary>The rate at which stamina regenerates.</summary>
    public float staminaRegenRate = 15f;
    private float currentStamina;

    /// <summary>
    /// Initializes the component by getting the CharacterController and setting default values.
    /// </summary>
    void Start()
    {
        // Get the CharacterController component attached to this GameObject
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
    }

    /// <summary>
    /// Called every frame. Handles player input for movement, jumping, and sprinting.
    /// </summary>
    void Update()
    {
        // Check if the player is grounded using a sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset vertical velocity if grounded and falling
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep player on the ground
        }

        // Get input for horizontal and vertical movement
        float x = Input.GetAxis("Horizontal"); // A/D keys or Left/Right arrows
        float z = Input.GetAxis("Vertical");   // W/S keys or Up/Down arrows

        // Create a movement vector relative to the player's forward direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Handle Sprinting and Stamina
        HandleSprinting();

        // Apply movement to the CharacterController
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Handle Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles the logic for sprinting, including speed changes and stamina management.
    /// </summary>
    private void HandleSprinting()
    {
        // Check for sprint key press and if the player is moving
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).sqrMagnitude > 0)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            currentSpeed = walkSpeed;
            // Regenerate stamina if it's not full
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
        }
        // Clamp stamina to ensure it doesn't go below 0 or above maxStamina
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}
