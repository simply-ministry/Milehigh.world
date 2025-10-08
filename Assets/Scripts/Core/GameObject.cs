using System; // Required for Console.WriteLine

/// <summary>
/// Represents a basic object in the game world with encapsulated data.
/// </summary>
public class GameObject
{
    // Private fields to store the object's state.
    // They are not directly accessible from outside this class.
    private string _name;
    private float _x;
    private float _y;
    private int _health;

    // Public properties provide controlled access to the private fields.
    // The 'get' accessor allows reading the value, but there is no 'set' accessor,
    // making the properties read-only from outside the class.
    public string Name => _name;
    public float X => _x;
    public float Y => _y;
    public int Health => _health;

    // Constructor
    public GameObject(string name, float x, float y, int health)
    {
        _name = name;
        _x = x;
        _y = y;
        _health = health;
        Console.WriteLine($"{_name} created at ({_x}, {_y}) with {_health} health.");
    }

    // Public method to modify the object's state.
    // This is the only way to change the health from outside the class.
    public void TakeDamage(int damage)
    {
        // Ensure health doesn't go below zero
        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
        }
        Console.WriteLine($"{_name} took {damage} damage. Current Health: {_health}");

        if (_health <= 0)
        {
            Console.WriteLine($"{_name} has been defeated!");
        }
    }

    /// <summary>
    /// Provides a string representation of the GameObject.
    /// </summary>
    /// <returns>A string summarizing the object's state.</returns>
    public override string ToString()
    {
        return $"{Name} [Position: ({X}, {Y}), Health: {Health}]";
    }
}

/// <summary>
/// Contains the main entry point for the example application.
/// Demonstrates how to create and interact with GameObject instances.
/// </summary>
public class Example
{
    /// <summary>
    /// The main method where the program execution begins.
    /// </summary>
    /// <param name="args">Command-line arguments (not used here).</param>
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting GameObject Example...");

        // Create a new GameObject instance representing a player character.
        GameObject player = new GameObject("Hero", 0, 0, 100);
        Console.WriteLine($"Initial state: {player}"); // Use ToString()

        Console.WriteLine("\nApplying 20 damage...");
        // Call the public TakeDamage method on the player instance.
        player.TakeDamage(20);

        Console.WriteLine("\nAccessing public properties:");
        // Access the public X property of the player instance.
        Console.WriteLine($"Player's X position: {player.X}");
        // Access the public Health property.
        Console.WriteLine($"Player's current health: {player.Health}");

        Console.WriteLine("\nApplying 90 damage...");
        // Apply enough damage to defeat the player.
        player.TakeDamage(90);

        Console.WriteLine($"\nFinal state: {player}"); // Check final state

        Console.WriteLine("\nGameObject Example Finished.");
    }
}