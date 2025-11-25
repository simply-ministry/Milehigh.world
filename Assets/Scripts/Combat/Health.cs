using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, DamageType damageType)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
    }

    public void Revive(float healthPercentage)
    {
        currentHealth = maxHealth * healthPercentage;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
    }
}

public enum DamageType
{
    Physical,
    Elemental,
    Void
}
