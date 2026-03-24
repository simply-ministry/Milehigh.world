using UnityEngine;

// This component manages the unique abilities for Otis/X.
public class OtisXAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Randomly unleashes a powerful ability from either his noble Sentinel past or his corrupted Void present.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Otis/X unleashes their Rage Burst: MEMORY FRAGMENT!");
 // VFX: Otis/X glitches between a noble and a corrupted form, then attacks.
// Randomly execute one of two powerful attacks.
if (Random.value > 0.5f) {
Debug.Log("Memory Fragment: Sentinel's Blade!");
// Simulate a multi-hit combo
if(target.TryGetComponent<Health>(out var health))
{
health.TakeDamage(100, DamageType.Physical);
health.TakeDamage(100, DamageType.Physical);
health.TakeDamage(150, DamageType.Physical);
}
} else {
Debug.Log("Memory Fragment: Void Lash!");
// Simulate an AoE attack
var enemies = FindEnemiesInArea(target.transform.position, 5f);
foreach(var enemy in enemies)
{
if(enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(200, DamageType.Void); }
}
}
ResetRage();
}

/// <summary>
/// Switches between "Sentinel" and "Corrupted" stances, altering his skills and stats for a limited time.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Otis/X channels their Spirit Break: PARTITION SHIFT!");
 // VFX: Otis/X is wreathed in either light or shadow.
// Switch between Sentinel (defensive) and Corrupted (offensive) stances.
// This would be managed by a property on the OtisXAbilities component.
if (currentStance == Stance.Sentinel) {
currentStance = Stance.Corrupted;
Debug.Log("Partition Shift: Switched to CORRUPTED stance!");
} else {
currentStance = Stance.Sentinel;
Debug.Log("Partition Shift: Switched to SENTINEL stance!");
}
ResetMana();
}

/// <summary>
/// Briefly regaining full clarity, Otis leads the team with a perfect tactical strike that inflicts a long-lasting defense debuff on the target.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Otis/X's Finisher: REDEEMED STRIKE!");
 // Grand cinematic: Otis/X strikes with the clarity of his former self.
// Inflict a long-lasting defense debuff on the primary target.
Debug.Log("OtIS lands a Redeemed Strike!");
if (target.TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.ArmorBreak, 20f);
status.ApplyEffect(StatusEffectType.WeaknessExposed, 20f);
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
