namespace Milehigh.World.Core
{
    /// <summary>
    /// Provides a common contract for any game entity that can take damage.
    /// This allows for a unified way to handle damage across different types of
    /// characters and objects.
    /// </summary>
    public interface IHealth
    {
        /// <summary>
        /// Reduces the health of the entity by a specified amount.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        void TakeDamage(float amount);
    }
}