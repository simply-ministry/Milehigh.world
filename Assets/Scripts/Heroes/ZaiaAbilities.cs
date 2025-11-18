using UnityEngine;

// This component manages the unique abilities for Zaia.
public class ZaiaAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Marks an enemy. If the marked enemy's health falls below a certain threshold (e.g., 25%), they are instantly executed.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Zaia unleashes their Rage Burst: FINAL JUDGMENT!");
 // VFX: A glowing, spectral sigil appears on the target.
// Mark the target for execution. A separate system would check this status.
if (target.TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.Judgement, 30f, 0.25f); // Duration 30s, execute threshold 25%
}
ResetRage();
}

/// <summary>
/// Creates a holy area that damages enemies and heals allies within it over time.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Zaia channels their Spirit Break: CONSECRATED GROUND!");
 // VFX: A circle of golden light appears on the ground around Zaia.
// Instantiate a Consecrated Ground prefab that handles the AoE heal/damage.
if (consecratedGroundPrefab != null)
{
Instantiate(consecratedGroundPrefab, transform.position, Quaternion.identity);
}
ResetMana();
}

/// <summary>
/// All party members gain a buff that executes any enemy they damage if that damage would drop the enemy below 15% health.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Zaia's Finisher: UNWAVERING DECREE!");
 // Grand cinematic: All allies' weapons are blessed with a golden light.
// Apply a buff to all allies that allows them to execute low-health enemies.
Debug.Log("Zaia issues an Unwavering Decree!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Execute, 15f, 0.15f); } // Threshold 15%
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
