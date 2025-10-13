using UnityEngine;

/// <summary>
/// The base class for all Shadow Syndicate villains.
/// Provides core attributes and AI state management.
/// </summary>
public abstract class ShadowSyndicateVillain : MonoBehaviour
{
 [Header("Villain Attributes")]
 public string villainName;
 public int maxHealth = 200;
 public int currentHealth;

 public enum VillainAIState { Idle, Patrolling, Chasing, Attacking, Casting, Dead }
 public VillainAIState currentState;

 protected virtual void Awake()
 {
 currentHealth = maxHealth;
 currentState = VillainAIState.Idle;
 }

 /// <summary>
 /// Reduces the villain's health by a specified amount.
 /// </summary>
 /// <param name="damageAmount">The amount of damage to take.</param>
 public virtual void TakeDamage(int damageAmount)
 {
 currentHealth -= damageAmount;
 Debug.Log($"{villainName} takes {damageAmount} damage. Health is now {currentHealth}/{maxHealth}.");

 if (currentHealth <= 0)
 {
 currentHealth = 0;
 Die();
 }
 }

 /// <summary>
 /// Handles the villain's death.
 /// </summary>
 protected virtual void Die()
 {
 currentState = VillainAIState.Dead;
 Debug.Log($"{villainName} has been vanquished.");
 // TODO: Play death animation, trigger loot drop, etc.
 }

 // --- ABILITIES (to be overridden by child classes) ---

 public virtual void UsePrimaryAbility(GameObject target)
 {
 Debug.Log($"{villainName} uses their Primary Ability on {target.name}.");
 currentState = VillainAIState.Attacking;
 }

 public virtual void UseUltimateAbility()
 {
 Debug.Log($"{villainName} unleashes their Ultimate Ability!");
 currentState = VillainAIState.Casting;
 }
}