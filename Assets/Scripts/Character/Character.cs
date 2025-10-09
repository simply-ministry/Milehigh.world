using UnityEngine;

/// <summary>
/// The base class for all characters in Milehigh.World.
/// Contains shared attributes for stats, progression, inventory, and abilities.
/// This component should be attached to any character prefab.
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Core Attributes")]
    public string characterName = "Character";
    public float health = 100f;
    public float maxHealth = 100f;
    public float mana = 100f;
    public float maxMana = 100f;
    public bool isAlive = true;

    [Header("Progression")]
    public int level = 1;
    public int skillPoints = 0;
    public float xp = 0f;
    public float xpToNextLevel = 100f;
    public float xpValueOnDefeat = 50f;

    [Header("Combat Stats")]
    public float baseAttackDamage = 5f;
    public float baseDefense = 0f;

    // We will create these custom classes in the next steps
    // [Header("Systems")]
    // public Inventory inventory;
    // public Equipment equipment;
    // public SkillTree skillTree;
    // public QuestJournal questJournal;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Initialize systems here
        // inventory = new Inventory(this);
        // equipment = new Equipment(this);
    }

    /// <summary>
    /// Applies damage to the character, factoring in defense.
    /// </summary>
    /// <param name="amount">The incoming damage amount.</param>
    public virtual void TakeDamage(float amount)
    {
        float damageTaken = Mathf.Max(1f, amount - baseDefense); // Always take at least 1 damage
        health -= damageTaken;

        Debug.Log($"{characterName} takes {damageTaken} damage.");

        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            Die();
        }
    }

    /// <summary>
    /// Heals the character for a given amount.
    /// </summary>
    /// <param name="amount">The amount to heal.</param>
    public void Heal(float amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        Debug.Log($"{characterName} heals for {amount}.");
    }

    /// <summary>
    /// Reduces character's mana and returns true if successful.
    /// </summary>
    /// <param name="amount">The amount of mana to use.</param>
    /// <returns>True if there was enough mana, false otherwise.</returns>
    public bool UseMana(float amount)
    {
        if (mana >= amount)
        {
            mana -= amount;
            Debug.Log($"{characterName} uses {amount} mana.");
            return true;
        }
        else
        {
            Debug.Log($"{characterName} does not have enough mana.");
            return false;
        }
    }

    /// <summary>
    /// Restores the character's mana.
    /// </summary>
    /// <param name="amount">The amount of mana to restore.</param>
    public void RestoreMana(float amount)
    {
        mana = Mathf.Min(maxMana, mana + amount);
        Debug.Log($"{characterName} restores {amount} mana.");
    }

    /// <summary>
    /// Handles the character's death logic.
    /// </summary>
    protected virtual void Die()
    {
        Debug.Log($"{characterName} has been defeated.");
        // In a real game, you might disable the GameObject or play a death animation.
        gameObject.SetActive(false);
    }
}