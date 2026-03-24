using UnityEngine;

// This component manages the unique abilities for Micah.
public class MicahAbilities : CharacterAbilitiesBase
{
// --- SIGNATURE ABILITIES ---

/// <summary>
/// Taunts all enemies on the field and becomes invulnerable for a short period of time.
/// </summary>
public override void ExecuteRageBurst(GameObject target)
{
if (!IsRageBurstReady) return;
Debug.Log("Micah unleashes their Rage Burst: LAST STAND!");
 // VFX: Micah slams his shield, drawing red lines of aggro from all enemies to himself.
// Taunt all enemies and become invulnerable.
var allEnemies = FindAllEnemies();
foreach(var enemy in allEnemies) { if (enemy.TryGetComponent<AIController>(out var ai)) { ai.SetTarget(this.gameObject); } }
if(TryGetComponent<StatusManager>(out var status)) { status.ApplyEffect(StatusEffectType.Invulnerable, 8f); }
ResetRage();
}

/// <summary>
/// Projects a massive energy shield in front of the party, blocking all incoming projectiles for its duration.
/// </summary>
public override void ExecuteSpiritBreak(GameObject target)
{
if (!IsSpiritBreakReady) return;
Debug.Log("Micah channels their Spirit Break: AEGIS OF HOPE!");
 // VFX: A massive, translucent blue shield appears before Micah, blocking incoming fire.
// Instantiate the shield prefab.
if (aegisShieldPrefab != null)
{
Instantiate(aegisShieldPrefab, transform.position + transform.forward * 2, transform.rotation);
}
ResetMana();
}

/// <summary>
/// Grants the entire party a shield equal to 50% of their maximum health.
/// </summary>
public override void PerformNovaminaadFinisher(GameObject target)
{
Debug.Log("Micah's Finisher: UNBREAKABLE WALL!");
 // Grand cinematic: Shields of light envelop the entire party.
// Grant all allies a temporary shield based on their max health.
Debug.Log("Micah forms an Unbreakable Wall around the party!");
var allies = FindAllAllies();
foreach (var ally in allies) {
if (ally.TryGetComponent<Health>(out var health) && ally.TryGetComponent<ShieldManager>(out var shield))
{
shield.ApplyShield(health.maxHealth * 0.5f, 20f); // 50% max health shield for 20s
}
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.
