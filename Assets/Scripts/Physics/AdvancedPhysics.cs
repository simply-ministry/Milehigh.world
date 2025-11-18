using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedPhysics : MonoBehaviour
{
[Range(0, 2f)]
public float friction = 0.2f;
[Range(0, 1f)]
public float airResistance = 0.1f;

private Rigidbody rb;
private bool isGrounded;

void Start()
{
rb = GetComponent<Rigidbody>();
}

void FixedUpdate()
{
CheckGrounded();
ApplyFriction();
ApplyAirResistance();
}

private void CheckGrounded()
{
// Simple ground check using a short raycast.
isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
}

private void ApplyFriction()
{
if (isGrounded)
{
Vector3 frictionForce = -rb.velocity * friction;
// Only apply horizontal friction.
frictionForce.y = 0;
rb.AddForce(frictionForce, ForceMode.VelocityChange);
}
}

private void ApplyAirResistance()
{
// Drag is proportional to the square of velocity.
float speed = rb.velocity.magnitude;
float dragMagnitude = airResistance * speed * speed;
Vector3 dragForce = -rb.velocity.normalized * dragMagnitude;
rb.AddForce(dragForce, ForceMode.Force);
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.