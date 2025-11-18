using UnityEngine;

// This component manages the unique abilities for Ingris.
public class IngrisAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Unleashes a torrent of fire, dealing massive damage to a single target and applying a burning effect over time.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Ingris unleashes their Rage Burst: SCORCHED EARTH!");
 // VFX: A massive wave of fire erupts from Ingris.
// Apply high damage and a Damage-Over-Time effect to the target.
if (target.TryGetComponent<Health>(out var health)) { health.TakeDamage(250, DamageType.Elemental); }
if (target.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Burn, 8f); }
ResetRage();
}

/// <summary>
/// Sacrifices 50% of her current health to revive a fallen ally with 50% health.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Ingris channels their Spirit Break: PHOENIX DOWN!");
 // VFX: Ingris is engulfed in light, which then transfers to a fallen ally.
// Sacrifice 50% current health to revive a party member.
currentHealth *= 0.5f;
var fallenAlly = FindFallenAlly(); // Assumes a helper function to find a defeated party member.
if (fallenAlly != null && fallenAlly.TryGetComponent<Health>(out var allyHealth))
{
allyHealth.Revive(0.5f); // Revive with 50% health.
Debug.Log($"{name} revives {fallenAlly.name}!");
}
ResetMana();
}

/// <summary>
/// Heals the entire party to full health and grants a temporary damage buff.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Ingris's Finisher: REBIRTH'S EMBRACE!");
 // Grand cinematic: A wave of gentle phoenix fire washes over the party.
Debug.Log("The party is embraced by the Phoenix's rebirth!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<Health>(out var health)) { health.HealToFull(); }
if (ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.DamageUp, 15f); }
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
