// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Simulates underwater movement physics for a GameObject with a Rigidbody.
/// This script applies buoyancy and allows for player-controlled swimming when active.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class UnderwaterMovement : MonoBehaviour
{
    /// <summary>
    /// The speed for horizontal swimming (forward, backward, left, right).
    /// </summary>
    [Tooltip("The speed for horizontal swimming.")]
    public float swimSpeed = 3.0f;
    /// <summary>
    /// The speed for vertical swimming (ascending and descending).
    /// </summary>
    [Tooltip("The speed for ascending and descending.")]
    public float verticalSwimSpeed = 2.0f;
    /// <summary>
    /// The strength of the upward buoyant force applied to the object when underwater.
    /// </summary>
    [Tooltip("The strength of the upward buoyant force.")]
    public float buoyancy = 1.5f;
    /// <summary>
    /// A flag indicating whether the object is currently considered to be underwater.
    /// This should be controlled by another script (e.g., one that uses triggers).
    /// </summary>
    [Tooltip("Is the object currently in water?")]
    public bool isUnderwater = false;

    /// <summary>
    /// A reference to the Rigidbody component on this GameObject.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Initializes the component by getting the required Rigidbody reference.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            // This should not happen due to the [RequireComponent] attribute, but it's good practice to check.
            Debug.LogError("UnderwaterMovement requires a Rigidbody component, but it was not found.");
        }
    }

    /// <summary>
    /// Called every fixed-framerate frame. Applies buoyancy and swimming forces if the object is underwater.
    /// </summary>
    void FixedUpdate()
    {
        if (isUnderwater)
        {
            // Apply a constant upward force to simulate buoyancy.
            rb.AddForce(Vector3.up * buoyancy, ForceMode.Acceleration);

            // Get player input for swimming.
            float horizontalInput = Input.GetAxis("Horizontal"); // Left/Right
            float verticalInput = Input.GetAxis("Vertical");   // Forward/Backward
            float ascendInput = 0f;

            if (Input.GetKey(KeyCode.Space)) ascendInput = 1f;        // Ascend
            if (Input.GetKey(KeyCode.LeftControl)) ascendInput = -1f; // Descend

            // Calculate movement vectors
            Vector3 horizontalDirection = new Vector3(horizontalInput, 0, verticalInput);
            Vector3 verticalDirection = new Vector3(0, ascendInput, 0);

            // Apply forces based on input, making horizontal movement relative to player's orientation
            rb.AddForce(transform.TransformDirection(horizontalDirection) * swimSpeed, ForceMode.Acceleration);
            rb.AddForce(verticalDirection * verticalSwimSpeed, ForceMode.Acceleration);
        }
    }
}