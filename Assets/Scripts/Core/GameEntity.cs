using UnityEngine;

/// <summary>
/// Represents a basic entity in the game world with encapsulated data.
/// This is a plain C# class, not a MonoBehaviour, making it a lightweight data container.
/// The [System.Serializable] attribute allows instances of this class to be serialized
/// and viewed in the Unity Inspector.
/// </summary>
[System.Serializable]
public class GameEntity
{
    // Private fields, exposed to the Unity Inspector via [SerializeField]
    // but not directly accessible from other scripts.
    [SerializeField] private string _name;
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private int _health;

    // Public properties provide controlled, read-only access to the private fields.
    // This is the core of encapsulation.

    /// <summary>
    /// The name of the game entity.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// The X-coordinate of the entity's position.
    /// </summary>
    public float X => _x;

    /// <summary>
    /// The Y-coordinate of the entity's position.
    /// </summary>
    public float Y => _y;

    /// <summary>
    /// The current health points of the entity.
    /// </summary>
    public int Health => _health;

    // Constructor

    /// <summary>
    /// Initializes a new instance of the GameEntity class.
    /// </summary>
    /// <param name="name">The name for the game entity.</param>
    /// <param name="x">The initial X-coordinate.</param>
    /// <param name="y">The initial Y-coordinate.</param>
    /// <param name="health">The initial health points.</param>
    public GameEntity(string name, float x, float y, int health)
    {
        _name = name;
        _x = x;
        _y = y;
        _health = health;
        Debug.Log($"[GameEntity] A new entity named '{_name}' was created at ({_x}, {_y}) with {_health} HP.");
    }

    // Public methods define the behaviors and operations for the entity.

    /// <summary>
    /// Reduces the game entity's health by a specified amount.
    /// This is the only public method to modify health, ensuring all damage logic
    /// (like bounds checking and logging) is consistently applied.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict. Must be non-negative.</param>
    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning($"[GameEntity] Damage amount cannot be negative. Ignoring damage to {_name}.");
            return;
        }

        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
        }
        Debug.Log($"[GameEntity] {_name} took {damage} damage. Current Health: {_health}");

        if (_health <= 0)
        {
            Debug.Log($"[GameEntity] {_name} has been defeated!");
            // In a real game, you might fire off an event here, e.g., OnDefeated?.Invoke(this);
        }
    }

    /// <summary>
    /// Provides a string representation of the GameEntity for easy debugging.
    /// </summary>
    /// <returns>A string summarizing the entity's current state.</returns>
    public override string ToString()
    {
        return $"{_name} [Position: ({_x}, {_y}), Health: {_health}]";
    }
}