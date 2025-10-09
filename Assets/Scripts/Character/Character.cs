using UnityEngine;

/// <summary>
/// A base class for all characters in the game, both playable and non-playable.
/// Contains shared attributes like health, stats, and inventory.
/// </summary>
public class Character : MonoBehaviour
{
    public string characterName;
    public float health;
    public float maxHealth;
}