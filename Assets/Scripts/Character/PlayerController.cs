using UnityEngine;

/// <summary>
/// Handles player input and controls character movement.
/// This script requires a Rigidbody and a Character component on the same GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Character character;
    private Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();

        // Ensure the Rigidbody doesn't have gravity if we are controlling it manually in this way,
        // and freeze rotation to prevent the character from tipping over.
        rb.useGravity = true; // Let's keep gravity for now to work with the physics system
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Read input from the horizontal and vertical axes (WASD or arrow keys).
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a movement vector based on the input.
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    void FixedUpdate()
    {
        // Apply the movement force in the FixedUpdate loop to ensure smooth physics interactions.
        // We'll use the speed from the attached Character script.
        if (moveDirection.magnitude > 0.1f)
        {
            rb.AddForce(moveDirection * character.speed, ForceMode.Acceleration);
        }
    }
}