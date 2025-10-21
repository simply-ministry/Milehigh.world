// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

namespace Milehigh.World.Core
{
    /// <summary>
    /// This script demonstrates how to programmatically manipulate a GameObject's
    /// position, rotation, and scale, which can be used for sprites.
    /// </summary>
    public class SpriteManipulator : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the sprite moves when using the MoveSprite method.
        /// </summary>
        public float moveSpeed = 5f;
        /// <summary>
        /// The speed at which the sprite rotates in the Update loop demonstration.
        /// </summary>
        public float rotationSpeed = 100f;
        /// <summary>
        /// The speed at which the sprite scales up and down in the Update loop demonstration.
        /// </summary>
        public float scaleSpeed = 1f;

        /// <summary>
        /// The initial position of the GameObject, stored at the start.
        /// </summary>
        private Vector3 initialPosition;
        /// <summary>
        /// The initial rotation of the GameObject, stored at the start.
        /// </summary>
        private Quaternion initialRotation;
        /// <summary>
        /// The initial scale of the GameObject, stored at the start.
        /// </summary>
        private Vector3 initialScale;

        /// <summary>
        /// Stores the initial transform values to use as a reference for the Update loop's visual demonstration.
        /// </summary>
        void Start()
        {
            // Store the initial transform values to use as a reference
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialScale = transform.localScale;
        }

        /// <summary>
        /// Provides a visual demonstration of the manipulation methods each frame.
        /// This method continuously rotates, scales, and moves the object in a predictable pattern.
        /// </summary>
        void Update()
        {
            // This Update loop provides a visual demonstration of the manipulation methods.

            // 1. Rotation: Continuously rotate the object around the Z-axis.
            RotateSprite(Vector3.forward, rotationSpeed * Time.deltaTime);

            // 2. Scaling: Scale the object up and down using a sine wave for a smooth pulse effect.
            float scaleFactor = 1 + Mathf.Sin(Time.time * scaleSpeed) * 0.5f;
            SetScale(initialScale * scaleFactor);

            // 3. Movement: Move the object in a circular path.
            float moveX = Mathf.Cos(Time.time) * 2f;
            float moveY = Mathf.Sin(Time.time) * 2f;
            SetPosition(initialPosition + new Vector3(moveX, moveY, 0));
        }

        /// <summary>
        /// Sets the absolute world position of the GameObject.
        /// </summary>
        /// <param name="newPosition">The new world position.</param>
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        /// <summary>
        /// Moves the GameObject by a given vector, scaled by moveSpeed.
        /// </summary>
        /// <param name="movement">The direction and magnitude of movement.</param>
        public void MoveSprite(Vector3 movement)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Sets the absolute world rotation of the GameObject.
        /// </summary>
        /// <param name="newRotation">The new rotation as a Quaternion.</param>
        public void SetRotation(Quaternion newRotation)
        {
            transform.rotation = newRotation;
        }

        /// <summary>
        /// Rotates the GameObject around a given axis by a specific angle.
        /// </summary>
        /// <param name="axis">The axis of rotation (e.g., Vector3.forward for 2D Z-axis).</param>
        /// <param name="angle">The angle to rotate by in degrees.</param>
        public void RotateSprite(Vector3 axis, float angle)
        {
            transform.Rotate(axis, angle);
        }

        /// <summary>
        /// Sets the absolute local scale of the GameObject.
        /// </summary>
        /// <param name="newScale">The new local scale.</param>
        public void SetScale(Vector3 newScale)
        {
            transform.localScale = newScale;
        }
    }
}