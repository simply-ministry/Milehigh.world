using UnityEngine;

public abstract class CharacterAbilitiesBase : MonoBehaviour
{
[Header("Core Resources")]
[SerializeField] protected float maxHealth = 100f;
[SerializeField] protected float currentHealth;
[SerializeField] protected float maxMana = 100f;
[SerializeField] protected float currentMana;
[SerializeField] protected float maxRage = 100f;
[SerializeField] protected float currentRage;

public bool IsRageBurstReady => currentRage >= maxRage;
public bool IsSpiritBreakReady => currentMana >= maxMana;

protected virtual void Start()
{
currentHealth = maxHealth;
currentMana = 20f;
currentRage = 0f;
}

public void TakeDamage(float amount)
{
currentHealth -= amount;
currentRage = Mathf.Min(maxRage, currentRage + amount); // Gain rage on taking damage
if (currentHealth <= 0) Die();
}

protected void ResetRage() => currentRage = 0f;
protected void ResetMana() => currentMana = 0f;

protected virtual void Die()
{
Debug.Log($"{gameObject.name} has been defeated.");
}

// --- Abstract methods for unique character abilities ---
public abstract void ExecuteRageBurst(GameObject target);
public abstract void ExecuteSpiritBreak(GameObject target);
public abstract void PerformNovaminaadFinisher(GameObject target);
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.