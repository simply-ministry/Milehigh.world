// Placed in Assets/Scripts/Core/Interfaces/IHealth.cs

/// <summary>
/// Defines a contract for any game entity that has health and can take damage.
/// This allows for a unified way to handle damage across different types of objects (players, enemies, etc.).
/// </summary>
public interface IHealth
{
    /// <summary>
    /// The current health points of the entity.
    /// </summary>
    float CurrentHealth { get; }

    /// <summary>
    /// The maximum possible health points of the entity.
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// Reduces the entity's health by a specified amount.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict.</param>
    void TakeDamage(float amount);
}