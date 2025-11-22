using UnityEngine;

// This component manages the unique abilities for Anastasia.
public class AnastasiaAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Pulls the entire battlefield into the Dreamscape, confusing all enemies and empowering all allies for a short time.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Anastasia unleashes their Rage Burst: ONEIRIC COLLAPSE!");
 // VFX: The battlefield environment distorts and shifts into a dreamlike state.
// Confuse all enemies and apply an empowerment buff to all allies.
var enemies = FindEnemiesInArea(transform.position, 30f);
foreach(var enemy in enemies) { if(enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Confuse, 10f); } }
var allies = FindAllAllies();
foreach(var ally in allies) { if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Empower, 10f); } }
ResetRage();
}

/// <summary>
/// Puts all enemies on the battlefield to sleep for a moderate duration. The effect breaks if they take damage.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Anastasia channels their Spirit Break: MASS SLUMBER!");
 // VFX: A wave of tranquil, sleepy energy washes over the enemies.
// Puts all enemies on the battlefield to sleep.
var allEnemies = FindAllEnemies();
foreach(var enemy in allEnemies)
{
if(enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Sleep, 15f); }
}
ResetMana();
}

/// <summary>
/// Creates a psychic link between all allies, causing their attacks to also heal the party for a percentage of the damage dealt.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Anastasia's Finisher: SHARED DREAM!");
 // Grand cinematic: A psychic link of light connects all allies.
// Apply a lifesteal effect to the entire party.
Debug.Log("Anastasia weaves a Shared Dream, linking the party's fates!");
var allies = FindAllAllies();
foreach(var ally in allies)
{
if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Lifesteal, 15f); }
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
