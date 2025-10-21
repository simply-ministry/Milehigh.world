// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// This script provides a technical demonstration of programmatic control over a GameObject's
/// position, rotation, and scale. It is intended for developer reference and is not tied
/// to a specific gameplay feature.
/// </summary>
public class SpriteManipulator : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the object moves in its circular path.")]
    public float moveSpeed = 2f;

    [Header("Rotation Settings")]
    [Tooltip("The speed at which the object rotates continuously.")]
    public float rotationSpeed = 50f;

    [Header("Scale Settings")]
    [Tooltip("The speed at which the object pulses in size.")]
    public float scaleSpeed = 1.5f;
    [Tooltip("The minimum scale for the pulse effect.")]
    public float minScale = 0.8f;
    [Tooltip("The maximum scale for the pulse effect.")]
    public float maxScale = 1.2f;

    private Vector3 initialPosition;
    private float angle;

    void Start()
    {
        // Store the initial position to use as the center of the circular path
        initialPosition = transform.position;
    }

    void Update()
    {
        // --- MOVEMENT ---
        // Move the object in a circular path around its initial position
        angle += moveSpeed * Time.deltaTime;
        float x = initialPosition.x + Mathf.Cos(angle) * 2f; // 2f is the radius
        float y = initialPosition.y + Mathf.Sin(angle) * 2f;
        transform.position = new Vector3(x, y, initialPosition.z);

        // --- ROTATION ---
        // Continuously rotate the object around its Z-axis
        RotateSprite(Vector3.forward * rotationSpeed * Time.deltaTime);

        // --- SCALING ---
        // Create a "pulse" effect by scaling the object up and down using a sine wave
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * scaleSpeed) + 1) / 2f);
        SetScale(new Vector3(scale, scale, scale));
    }

    /// <summary>
    /// Sets the absolute position of the sprite.
    /// </summary>
    /// <param name="newPosition">The new world-space position.</param>
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    /// <summary>
    /// Moves the sprite by a given vector, relative to its current position.
    /// </summary>
    /// <param name="movementVector">The vector to add to the current position.</param>
    public void MoveSprite(Vector3 movementVector)
    {
        transform.Translate(movementVector);
    }

    /// <summary>
    /// Sets the absolute rotation of the sprite as a Quaternion.
    /// </summary>
    /// <param name="newRotation">The new world-space rotation.</param>
    public void SetRotation(Quaternion newRotation)
    {
        transform.rotation = newRotation;
    }

    /// <summary>
    /// Applies a rotation to the sprite, relative to its current rotation.
    /// </summary>
    /// <param name="rotationVector">The Euler angles to add to the current rotation.</param>
    public void RotateSprite(Vector3 rotationVector)
    {
        transform.Rotate(rotationVector);
    }

    /// <summary>
    /// Sets the absolute local scale of the sprite.
    /// </summary>
    /// <param name="newScale">The new local scale vector.</param>
    public void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
}