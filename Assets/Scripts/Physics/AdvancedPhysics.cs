using UnityEngine;

/// <summary>
/// Adds advanced physics properties like friction and air resistance to a Rigidbody.
/// Attach this component to any GameObject with a Rigidbody to apply these effects.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AdvancedPhysics : MonoBehaviour
{
    /// <summary>
    /// The coefficient of kinetic friction. Affects how quickly the object slows down
    /// when sliding on a surface. Only applied when the object is grounded.
    /// </summary>
    [Tooltip("Coefficient of kinetic friction. Affects sliding speed. Only applied when grounded.")]
    [Range(0f, 2f)]
    public float friction = 0.2f;

    /// <summary>
    /// The air resistance factor (drag). Affects how the object moves through the air.
    /// Higher values mean more resistance.
    /// </summary>
    [Tooltip("Air resistance factor. Affects movement through the air.")]
    public float airResistance = 0.1f;

    /// <summary>
    /// A reference to the Rigidbody component attached to this GameObject.
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// A flag indicating whether the object is currently touching the ground.
    /// </summary>
    private bool isGrounded;

    /// <summary>
    /// Initializes the component by getting the required Rigidbody reference.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called every fixed-framerate frame. Applies physics calculations.
    /// </summary>
    void FixedUpdate()
    {
        // Air resistance is always applied.
        ApplyAirResistance();

        // Friction is only applied when the object is considered grounded.
        if (isGrounded)
        {
            ApplyFriction();
        }
    }

    /// <summary>
    /// Applies a force that opposes the object's velocity, simulating air drag.
    /// The force is proportional to the square of the velocity, which is a common
    /// model for turbulent drag.
    /// </summary>
    private void ApplyAirResistance()
    {
        if (rb.velocity.magnitude < 0.01f) return;

        Vector3 dragForce = -airResistance * rb.velocity.sqrMagnitude * rb.velocity.normalized;
        rb.AddForce(dragForce);
    }

    /// <summary>
    /// Applies a kinetic friction force that opposes the object's horizontal motion.
    /// This simplified model calculates friction based on the normal force (mass * gravity).
    /// </summary>
    private void ApplyFriction()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude < 0.01f) return;

        // Calculate the magnitude of the friction force.
        float normalForce = rb.mass * Physics.gravity.magnitude;
        float frictionMagnitude = friction * normalForce;

        // Apply the force in the direction opposite to the horizontal velocity.
        Vector3 frictionForce = -frictionMagnitude * horizontalVelocity.normalized;

        // Ensure that friction doesn't cause the object to reverse direction in one frame.
        // This prevents jittering at low velocities.
        Vector3 velocityChange = frictionForce / rb.mass * Time.fixedDeltaTime;
        if (velocityChange.magnitude >= horizontalVelocity.magnitude)
        {
            // If the friction force is strong enough to stop the object, just set horizontal velocity to zero.
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            rb.AddForce(frictionForce, ForceMode.Force);
        }
    }

    /// <summary>
    /// Checks if the object is currently on the ground by analyzing collision contacts.
    /// This method is called once per frame for every collider/rigidbody that is touching another rigidbody/collider.
    /// </summary>
    /// <param name="collision">The collision data associated with this event.</param>
    void OnCollisionStay(Collision collision)
    {
        // Check the contact points to see if we are colliding with a surface below us.
        foreach (ContactPoint contact in collision.contacts)
        {
            // A normal pointing mostly upwards indicates a ground surface.
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    /// <summary>
    /// When the object stops colliding with something, assume it's no longer grounded.
    /// This is a simplification and might not be accurate in all scenarios (e.g., sliding off a ledge).
    /// </summary>
    /// <param name="collision">The collision data associated with this event.</param>
    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}