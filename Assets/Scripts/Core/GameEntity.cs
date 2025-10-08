using UnityEngine;

/// <summary>
/// Represents a basic entity in the game world, designed to be a Unity Component.
/// </summary>
public class GameEntity : MonoBehaviour
{
    // Public fields will be visible in the Unity Editor, allowing for design-time configuration.
    [Header("Entity Properties")]
    [SerializeField] private string entityName = "Default Entity";
    [SerializeField] private int health = 100;

    // Public properties provide controlled, read-only access to the entity's state from other scripts.
    public string Name => entityName;
    public int Health => health;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// It's used for initialization and setting up references.
    /// </summary>
    private void Awake()
    {
        // The entity's name and health are typically set in the Unity Editor.
        // We log a message to confirm the entity has been initialized.
        Debug.Log($"{entityName} has been initialized with {health} health.");
    }

    /// <summary>
    /// Public method to apply damage to the entity.
    /// This is the primary way other components should interact with this entity's health.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        // Ensure health doesn't go below zero.
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        Debug.Log($"{entityName} took {damage} damage. Current Health: {health}");

        if (health <= 0)
        {
            Debug.Log($"{entityName} has been defeated!");
            // In a real game, you would add logic here to handle the entity's death,
            // such as playing an animation, dropping loot, or removing the object from the scene.
        }
    }

    /// <summary>
    /// Provides a string representation of the GameEntity's current state.
    /// </summary>
    /// <returns>A string summarizing the entity's state.</returns>
    public override string ToString()
    {
        // Note: The concept of a single X, Y position is less relevant for a MonoBehaviour,
        // as its position is determined by the Transform component of the GameObject it's attached to.
        // We now reference the Transform for position information.
        return $"{entityName} [Position: {transform.position}, Health: {Health}]";
    }
}