using UnityEngine;

// This component manages the unique abilities for Reverie.
public class ReverieAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Consumes all Enigma to unleash a devastatingly powerful, but random, effect on the battlefield.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Reverie unleashes their Rage Burst: CHAOS UNLEASHED!");
 // VFX: A chaotic explosion of random elemental energy.
// Unleash one of several powerful random effects.
int effect = Random.Range(0, 4);
switch(effect) {
case 0:
Debug.Log("Chaos Unleashed: Massive Damage!");
if(target.TryGetComponent<Health>(out var h)) { h.TakeDamage(500, DamageType.Elemental); }
break;
case 1:
Debug.Log("Chaos Unleashed: Full Restoration!");
currentHealth = maxHealth;
currentMana = maxMana;
break;
case 2:
Debug.Log("Chaos Unleashed: Vulnerability!");
if(target.TryGetComponent<StatusManager>(out var s)) { s.ApplyEffect(StatusEffectType.Vulnerability, 10f); }
break;
case 3:
Debug.Log("Chaos Unleashed: Mass Stun!");
var enemies = FindEnemiesInArea(transform.position, 15f);
foreach(var e in enemies) { if(e.TryGetComponent<StatusManager>(out var st)) { st.ApplyEffect(StatusEffectType.Stun, 5f); } }
break;
}
ResetRage();
}

/// <summary>
/// Instantly fills the Enigma gauge to maximum, allowing for immediate use of Chaos Unleashed.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Reverie channels their Spirit Break: ENIGMA OVERLOAD!");
 // VFX: Reverie glows with pure potential, her Enigma gauge instantly filling.
// A unique property 'enigma' on her component is set to its max value.
this.enigma = this.maxEnigma; // Assuming these properties exist on ReverieAbilities.cs
Debug.Log("Reverie overloads her power, maxing out her Enigma!");
ResetMana();
}

/// <summary>
/// For a short time, all damage dealt by the party has a chance to trigger a random magical explosion.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Reverie's Finisher: WOVEN FATES!");
 // Grand cinematic: All allies' weapons glow with chaotic energy.
// Apply a party-wide buff that gives attacks a chance to cause a magical explosion.
Debug.Log("Reverie weaves the party's fates into a chaotic tapestry!");
var allies = FindAllAllies();
foreach(var ally in allies)
{
if(ally.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.ChainLightning, 15f); }
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
