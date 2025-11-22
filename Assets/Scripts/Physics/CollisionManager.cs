using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionManager : MonoBehaviour
{
[Range(0, 1f)]
public float restitution = 0.8f; // Bounciness

private Rigidbody rb;

void Start()
{
rb = GetComponent<Rigidbody>();
}

void OnCollisionEnter(Collision collision)
{
// Ensure this logic only runs once per collision pair.
if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID())
{
return;
}

Rigidbody otherRb = collision.rigidbody;
if (otherRb == null) return;

// Calculate relative velocity along the collision normal.
Vector3 normal = collision.contacts[0].normal;
Vector3 relativeVelocity = otherRb.velocity - rb.velocity;
float velocityAlongNormal = Vector3.Dot(relativeVelocity, normal);

// Do nothing if objects are already moving apart.
if (velocityAlongNormal > 0) return;

// Get the restitution from the other object if it has this component.
float otherRestitution = 0.8f;
if(collision.gameObject.TryGetComponent<CollisionManager>(out var otherManager))
{
otherRestitution = otherManager.restitution;
}

// Use the average restitution.
float e = (restitution + otherRestitution) / 2f;

// Calculate impulse magnitude.
float impulseMagnitude = -(1 + e) * velocityAlongNormal;
impulseMagnitude /= (1 / rb.mass) + (1 / otherRb.mass);

// Apply impulse.
Vector3 impulse = impulseMagnitude * normal;
rb.AddForce(-impulse, ForceMode.Impulse);
otherRb.AddForce(impulse, ForceMode.Impulse);
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.