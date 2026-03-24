using UnityEngine;

// This component manages the unique abilities for Aeron.
public class AeronAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Unleashes a deafening roar that stuns all nearby enemies and provides a temporary attack buff to Aeron.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Aeron unleashes their Rage Burst: PRIMAL ROAR!");
 // VFX: A powerful shockwave emanates from Aeron's roar.
// Stun nearby enemies and apply an attack buff to self.
var enemies = FindEnemiesInArea(transform.position, 15f);
foreach(var enemy in enemies) { if(enemy.TryGetComponent<StatusManager>(out var s)) { s.ApplyEffect(StatusEffectType.Stun, 4f); } }
if(TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.DamageUp, 10f); }
ResetRage();
}

/// <summary>
/// Soars into the air and crashes down, dealing massive AoE damage based on his current altitude.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Aeron channels their Spirit Break: GIGA IMPACT!");
 // VFX: Aeron leaps high into the air, then crashes down, creating a crater.
// This would likely be implemented as a coroutine to handle the animation.
// For this script, we simulate the end result: damage in an area.
Debug.Log("Aeron crashes down with Giga Impact!");
var enemiesInImpact = FindEnemiesInArea(target.transform.position, 8f);
float damage = 350; // Base damage, could be increased by altitude in a full implementation.
foreach(var enemy in enemiesInImpact)
{
if(enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(damage, DamageType.Physical); }
}
ResetMana();
}

/// <summary>
/// Leads the team in a coordinated charge, breaking enemy lines and making all enemies Vulnerable.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Aeron's Finisher: LIONHEART CHARGE!");
 // Grand cinematic: The party charges forward as one, led by Aeron.
// Damage and apply Vulnerable to all enemies.
Debug.Log("Aeron leads the Lionheart Charge!");
var allEnemies = FindAllEnemies();
foreach (var enemy in allEnemies) {
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(300, DamageType.Physical); }
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Vulnerability, 15f); }
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
