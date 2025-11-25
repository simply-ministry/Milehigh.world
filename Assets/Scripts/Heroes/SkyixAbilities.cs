using UnityEngine;

// This component manages the unique abilities for Sky.ix.
public class SkyixAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Becomes completely invisible and untargetable for a short duration, guaranteeing critical hits on her next three attacks.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Sky.ix unleashes their Rage Burst: VOID WALK!");
 // VFX: Sky.ix flickers and fades into a shimmering outline.
// Apply Invisibility, Untargetable, and a stack of Guaranteed Crits to self.
if(TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.Invisibility, 8f);
status.ApplyEffect(StatusEffectType.Untargetable, 8f);
status.ApplyEffect(StatusEffectType.GuaranteedCrits, 3); // 3 stacks
}
ResetRage();
}

/// <summary>
/// Opens a localized Void rift that pulls in nearby enemies, dealing continuous damage and slowing them.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Sky.ix channels their Spirit Break: REALITY TEAR!");
 // VFX: A swirling vortex of glitchy, purple energy appears at the target location.
// Instantiate a Reality Tear prefab that handles the pull, damage, and slow effects.
if (realityTearPrefab != null)
{
Instantiate(realityTearPrefab, target.transform.position, Quaternion.identity);
}
ResetMana();
}

/// <summary>
/// Teleports between all enemies, delivering a single, devastating blow to each before returning.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Sky.ix's Finisher: QUANTUM COLLAPSE!");
 // Grand cinematic: Sky.ix vanishes, and damage numbers appear on all enemies in rapid succession.
// Find all enemies and deal a large amount of damage to each.
Debug.Log("Sky.ix performs Quantum Collapse, striking all foes at once!");
var allEnemies = FindAllEnemies();
foreach (var enemy in allEnemies)
{
if(enemy.TryGetComponent<Health>(out var health))
{
health.TakeDamage(500, DamageType.Void);
}
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
