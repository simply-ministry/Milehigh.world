// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
/// <summary>
/// An interface for any game entity that can take damage.
/// This provides a common contract for health-related functionality,
/// promoting code reusability and organization.
/// </summary>
public interface IHealth
{
    /// <summary>
    /// Gets the current health of the entity.
    /// </summary>
    float CurrentHealth { get; }

    /// <summary>
    /// Gets the maximum health of the entity.
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// Reduces the entity's health by a specified amount.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    void TakeDamage(float amount);
}