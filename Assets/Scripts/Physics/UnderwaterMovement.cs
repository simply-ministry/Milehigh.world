using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnderwaterMovement : MonoBehaviour
{
[Header("Physics Properties")]
public float waterDensity = 1000f;
public float objectVolume = 1.0f;
public float dragCoefficient = 0.47f; // For a sphere
public float frontalArea = 1.0f;

[Header("Movement")]
public float verticalThrust = 10f;

private Rigidbody rb;
private float gravity;

void Start()
{
rb = GetComponent<Rigidbody>();
gravity = Physics.gravity.y;
}

void FixedUpdate()
{
// Assuming this script is only active when underwater.
// A real system would use triggers to enable/disable it.

// 1. Apply Buoyancy
float buoyantForce = waterDensity * objectVolume * -gravity;
rb.AddForce(Vector3.up * buoyantForce);

// 2. Apply Drag
Vector3 velocity = rb.velocity;
float speedSqr = velocity.sqrMagnitude;
if (speedSqr > 0)
{
float dragMagnitude = 0.5f * waterDensity * speedSqr * dragCoefficient * frontalArea;
Vector3 dragForce = -velocity.normalized * dragMagnitude;
rb.AddForce(dragForce);
}

// 3. Apply Player-controlled Thrust
float verticalInput = Input.GetAxis("Vertical"); // Using Unity's Input Manager
Vector3 thrust = transform.up * verticalInput * verticalThrust;
rb.AddForce(thrust);
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.