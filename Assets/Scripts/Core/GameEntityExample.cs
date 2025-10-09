using UnityEngine;

/// <summary>
/// An example script demonstrating how to use the GameEntity class.
/// To use this example:
/// 1. Create two GameObjects in your scene (e.g., "Player" and "Monster").
/// 2. Attach the GameEntity script to both GameObjects.
/// 3. In the Inspector for each GameObject, set their Entity Name and Health.
/// 4. Create another GameObject (e.g., "ExampleRunner").
/// 5. Attach this GameEntityExample script to the "ExampleRunner" GameObject.
/// 6. Drag the "Player" and "Monster" GameObjects from the Hierarchy into the
///    corresponding "Player" and "Monster" fields in the Inspector for this script.
/// 7. Run the scene.
/// </summary>
public class GameEntityExample : MonoBehaviour
{
    /// <summary>
    /// A reference to the GameEntity that represents the player.
    /// Assign this in the Unity Inspector.
    /// </summary>
    public GameEntity player;

    /// <summary>
    /// A reference to the GameEntity that represents the monster.
    /// Assign this in the Unity Inspector.
    /// </summary>
    public GameEntity monster;

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// It runs the combat simulation if both entities have been assigned.
    /// </summary>
    void Start()
    {
        if (player == null || monster == null)
        {
            Debug.LogError("Player and Monster entities must be assigned in the Inspector for the example to run.");
            return;
        }

        Debug.Log("--- Starting GameEntity Example ---");

        Debug.Log($"Initial state: {player}"); // Uses the overridden ToString() method
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