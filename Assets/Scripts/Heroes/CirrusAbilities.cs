using UnityEngine;

// This component manages the unique abilities for Cirrus.
public class CirrusAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Transforms into his dragon form for a short time, gaining new abilities and massively increased stats.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Cirrus unleashes their Rage Burst: DRAGON'S FURY!");
 // VFX: Cirrus is wreathed in lightning and fire, transforming into a draconic form.
// Enter a powered-up state.
if (TryGetComponent<StatusManager>(out var status))
{
status.ApplyEffect(StatusEffectType.DragonForm, 20f); // Buffs stats and abilities for 20s.
}
ResetRage();
}

/// <summary>
/// Unleashes a massive explosion of fire, heavily damaging all enemies on the battlefield.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Cirrus channels their Spirit Break: SUPERNOVA!");
 // VFX: A massive explosion of fire and plasma radiates from Cirrus.
// Deal heavy damage to all enemies on the screen.
var enemies = FindAllEnemies();
foreach (var enemy in enemies)
{
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(400, DamageType.Elemental); }
}
ResetMana();
}

/// <summary>
/// Summons a storm of meteors to strike all enemies, dealing massive elemental damage and stunning them.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Cirrus's Finisher: WRATH OF THE ANCIENTS!");
 // Grand cinematic: The sky darkens and a meteor shower rains down on the battlefield.
Debug.Log("The Wrath of the Ancients scours the battlefield!");
var enemies = FindAllEnemies();
foreach (var enemy in enemies) {
if (enemy.TryGetComponent<Health>(out var health)) { health.TakeDamage(600, DamageType.Elemental); }
if (enemy.TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Stun, 6f); }
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
