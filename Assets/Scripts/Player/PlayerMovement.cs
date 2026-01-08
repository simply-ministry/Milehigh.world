using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // The CharacterController component is used for player movement and collision.
    // It's a common way to handle player movement in Unity without directly manipulating Rigidbody physics.
    private CharacterController controller;

    // --- Movement Variables ---
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;

    // --- Gravity and Jumping ---
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    private Vector3 velocity;

    // --- Ground Check ---
    public Transform groundCheck;
    public float groundDistance = 0.4f; // Radius of the sphere for ground checking
    public LayerMask groundMask; // Layer to detect as ground
    private bool isGrounded;

    // --- Stamina System ---
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    private float currentStamina;

    void Start()
    {
        // Get the CharacterController component attached to this GameObject
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
    }

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