using UnityEngine;

// This component manages the unique abilities for Kai.
public class KaiAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Reveals the weak points of all enemies on the field, guaranteeing critical hits for the entire party for a short duration.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Kai unleashes their Rage Burst: MOMENT OF CLARITY!");
 // VFX: A wave of temporal energy reveals weak points on all enemies.
// Grant guaranteed critical hits to the entire party.
var allies = FindAllAllies();
foreach (var ally in allies)
{
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.GuaranteedCrits, 10f); }
}
ResetRage();
}

/// <summary>
/// Creates a field of distorted time, freezing all enemies within it for several seconds.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Kai channels their Spirit Break: TEMPORAL STASIS!");
 // VFX: A dome of shimmering, slow-moving energy envelops the battlefield.
// Freeze all enemies in a large area.
var enemies = FindEnemiesInArea(transform.position, 25f);
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Stun, 5f); }
}
ResetMana();
}

/// <summary>
/// Resets all ally cooldowns and fully restores their Mana.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Kai's Finisher: PROPHECY UNWRITTEN!");
 // Grand cinematic: A pulse of prophetic energy resets the flow of battle.
Debug.Log("The prophecy is unwritten! Cooldowns and mana are restored!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<AbilityManager>(out var manager)) { manager.ResetCooldowns(); }
// Assuming the base class handles mana restoration.
ally.GetComponent<CharacterAbilitiesBase>().RestoreManaToFull();
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
