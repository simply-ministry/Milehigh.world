using UnityEngine;

/// <summary>
/// Represents Kai the Prophet, one of the Ɲōvəmîŋāđ.
/// Kai is a tactical seer whose abilities focus on gathering information,
/// revealing enemy weaknesses, and providing support to his allies.
/// </summary>
public class Kai : Novamina
{
    // Placeholder for Kai's unique abilities.
    // In a full implementation, these methods would contain logic
    // to reveal information to the player or apply buffs.

    public void PropheticGlimpse(Character enemy)
    {
        Debug.Log($"{characterName} uses Prophetic Glimpse on {enemy.characterName}, revealing a weakness!");
    }

    public void TruthOfLinq()
    {
        Debug.Log($"{characterName} reveals the Truth of Lîŋq, illuminating hidden paths.");
    }
}