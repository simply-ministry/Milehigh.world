using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 100;
    public int currentHealth;
    public int attack = 50;
    public int defense = 50;
    public float speed = 50f;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Max(damage - defense, 0);
        currentHealth = Mathf.Max(currentHealth - damageTaken, 0);
        Debug.Log($"{characterName} takes {damageTaken} damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"{characterName} heals for {amount} health!");
    }

    protected virtual void Die()
    {
        Debug.Log($"{characterName} has been defeated!");
        // In a real game, you might disable the GameObject, play a death animation, etc.
        gameObject.SetActive(false);
    }
}