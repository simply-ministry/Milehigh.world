using System;
using UnityEngine;

/// <summary>
/// An enum to represent the different states a character can be in.
/// </summary>
public enum CharacterState
{
    Idle,
    Walking,
    Attacking,
    VoidTransformed,
    UnifiedEntity,
    Dead
}

/// <summary>
/// The base class for all characters in Milehigh.World.
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Core Identification")]
    [Tooltip("A unique identifier for this character instance.")]
    public Guid CharacterId { get; private set; }

    [Header("Core Attributes")]
    public string characterName = "Character";
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxMana = 100f;
    public float currentMana;
    public bool isAlive = true;

    [Header("Combat Stats")]
    public int attack = 10;
    public int defense = 5;

    protected virtual void Awake()
    {
        CharacterId = Guid.NewGuid();
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    /// <summary>
    /// Applies damage to the character.
    /// </summary>
    public virtual void TakeDamage(float amount)
    {
        if (!isAlive) return;

        float damageTaken = Mathf.Max(1f, amount - defense);
        currentHealth -= damageTaken;

        Debug.Log($"{characterName} takes {damageTaken} damage.");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Heals the character for a given amount.
    /// </summary>
    public void Heal(float amount)
    {
        if (!isAlive) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"{characterName} heals for {amount}.");
    }

    /// <summary>
    /// Reduces character's mana and returns true if successful.
    /// </summary>
    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the character's death logic.
    /// </summary>
    protected virtual void Die()
    {
        isAlive = false;
        Debug.Log($"{characterName} has been defeated.");
        gameObject.SetActive(false);
    }
}