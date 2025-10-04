using UnityEngine;

/// <summary>
/// Provides a more realistic collision response for Rigidbodies.
/// Attach this component to a GameObject with a Rigidbody to override the default
/// physics engine's collision response.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CollisionManager : MonoBehaviour
{
    /// <summary>
    /// The coefficient of restitution (bounciness). A value of 1 means a perfectly
    /// elastic collision (no energy loss), while a value of 0 means a perfectly
    /// inelastic collision (objects stick together).
    /// </summary>
    [Tooltip("The bounciness of the object. 0 = no bounce, 1 = perfect bounce.")]
    [Range(0f, 1f)]
    public float restitution = 0.8f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // We are handling collision response manually, so we can disable Unity's default response.
        // Note: This approach has limitations and might not be suitable for all scenarios.
        // For this example, we will calculate and apply the impulse manually.
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ensure the other object also has a Rigidbody to participate in the physics calculation.
        Rigidbody otherRb = collision.collider.attachedRigidbody;
        if (otherRb == null)
        {
            return;
        }

        // We only want to process the collision from one of the two objects to avoid double calculations.
        // We'll use the instance ID to decide which object handles the collision.
        // The object with the lower instance ID will be responsible for the calculation.
        if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            HandleCollision(collision, otherRb);
        }
    }

    /// <summary>
    /// Calculates and applies the collision response based on conservation of momentum.
    /// </summary>
    /// <param name="collision">The collision data from Unity.</param>
    /// <param name="otherRb">The Rigidbody of the other object in the collision.</param>
    private void HandleCollision(Collision collision, Rigidbody otherRb)
    {
        // Get the point of contact and the collision normal.
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;

        // Get the velocities of the two objects at the point of contact.
        Vector3 v1 = rb.GetPointVelocity(contact.point);
        Vector3 v2 = otherRb.GetPointVelocity(contact.point);

        // Calculate the relative velocity along the normal.
        float relativeVelocityNormal = Vector3.Dot(v2 - v1, normal);

        // If the objects are already moving apart, do nothing.
        if (relativeVelocityNormal > 0)
        {
            return;
        }

        // Use the smaller coefficient of restitution between the two objects.
        CollisionManager otherManager = otherRb.GetComponent<CollisionManager>();
        float combinedRestitution = (otherManager != null) ? Mathf.Min(restitution, otherManager.restitution) : restitution;

        // Calculate the impulse magnitude using the formula for 1D elastic collisions.
        float impulseMagnitude = -(1 + combinedRestitution) * relativeVelocityNormal;
        impulseMagnitude /= (1 / rb.mass) + (1 / otherRb.mass);

        // Apply the impulse to both objects along the collision normal.
        Vector3 impulse = impulseMagnitude * normal;
        rb.AddForce(-impulse, ForceMode.Impulse);
        otherRb.AddForce(impulse, ForceMode.Impulse);
    }
}