using UnityEngine;

/// <summary>
/// A MonoBehaviour to demonstrate the usage of the GameEntity class within Unity.
/// To use this, attach it to a GameObject in a Unity scene.
/// </summary>
public class GameEntityExample : MonoBehaviour
{
    private GameEntity player;
    private GameEntity monster;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        Debug.Log("--- Starting GameEntity Example ---");

        // Create a new GameEntity instance representing a player character.
        player = new GameEntity("Hero", 0, 0, 100);
        Debug.Log($"Initial state: {player}"); // Uses the overridden ToString() method

        // Create another instance for a monster.
        monster = new GameEntity("Goblin", 10, 5, 30);
        Debug.Log($"A wild {monster.Name} appears!");

        Debug.Log("\n--- Simulating Combat ---");

        // Player attacks the Goblin
        Debug.Log($"{player.Name} attacks {monster.Name}.");
        monster.TakeDamage(25);

        // Goblin retaliates
        Debug.Log($"{monster.Name} attacks {player.Name}.");
        player.TakeDamage(10);

        Debug.Log("\n--- Accessing Public Properties ---");
        // Access the public properties of the player instance.
        Debug.Log($"Player's X position: {player.X}");
        Debug.Log($"Player's current health: {player.Health}");
        Debug.Log($"Monster's current health: {monster.Health}");

        Debug.Log("\n--- Finishing Blow ---");
        // Apply enough damage to defeat the monster.
        monster.TakeDamage(15);

        Debug.Log($"\n--- Final States ---");
        Debug.Log($"Player's final state: {player}");
        Debug.Log($"Monster's final state: {monster}");

        Debug.Log("\n--- GameEntity Example Finished ---");
    }
}