// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Applies a custom gravity force to any Rigidbody that enters its trigger volume.
/// This can be used to create localized gravity fields or anti-gravity zones.
/// </summary>
[RequireComponent(typeof(Collider))]
public class CustomGravity : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("The direction and magnitude of the gravity to be applied within this zone.")]
    public Vector3 gravityForce = new Vector3(0, -9.81f, 0);

    /// <summary>
    /// Ensures the attached collider is set as a trigger.
    /// </summary>
    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Called when another collider enters the trigger.
    /// If the other collider has a Rigidbody, this method disables its default gravity
    /// and begins applying the custom gravity force.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Disable standard gravity for this object while it's in the zone.
            rb.useGravity = false;
        }
    }

    /// <summary>
    /// Called once per frame for every collider that is touching the trigger.
    /// Applies the custom gravity force to any Rigidbody within the zone.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply the custom gravity force.
            rb.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Called when another collider has stopped touching the trigger.
    /// If the other collider has a Rigidbody, this method re-enables its default gravity.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Re-enable standard gravity once the object leaves the zone.
            rb.useGravity = true;
        }
    }
}