using UnityEngine;

/// <summary>
/// A simple AI that attacks a random player character.
/// </summary>
[RequireComponent(typeof(Character))]
public class AIController : MonoBehaviour
{
    private Character self;

    void Awake()
    {
        self = GetComponent<Character>();
    }

    public void TakeTurn()
    {
        // A real AI would have more complex logic. For now, we'll just attack.
        PlayerCharacter target = FindObjectOfType<PlayerCharacter>(); // Assuming a PlayerCharacter script/tag
        if (target != null && target.isAlive)
        {
            Debug.Log($"{self.characterName} decides to attack {target.characterName}!");
            // In a full implementation, the AI would use its own abilities.
            // For now, a basic attack will suffice.
            // self.Attack(target);
        }
    }
}