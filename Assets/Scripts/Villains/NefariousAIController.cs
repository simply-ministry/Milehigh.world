using UnityEngine;

// Manages the AI and abilities for the antagonist Nefarious.
public class NefariousAIController : MonoBehaviour
{
[Header("AI Properties")]
public float detectionRadius = 30f;
public float attackRange = 10f;
private GameObject playerTarget;

void Start() { playerTarget = GameObject.FindGameObjectWithTag("Player"); }

void Update()
{
if (playerTarget == null) return;
float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
if (distanceToPlayer <= detectionRadius)
{
transform.LookAt(playerTarget.transform);
if (distanceToPlayer <= attackRange) { /* TODO: Implement attack logic */ }
else { /* TODO: Implement movement logic */ }
}
}

// --- ABILITIES ---
// Placeholder for signature abilities based on lore.
public void PerformSignatureAttack(GameObject target)
{
Debug.Log($"Nefarious uses a signature attack on {target.name}!");
}

}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
