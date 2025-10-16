// Example interface for any entity that can take damage.
// Place this in its own file, e.g., "IHealth.cs".
public interface IHealth
{
    void TakeDamage(float amount);
}