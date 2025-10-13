using UnityEngine;

/// <summary>
/// An enum to represent the different elements Reverie can control.
/// </summary>
public enum Element
{
    Fire,
    Water,
    Earth,
    Air
}

/// <summary>
/// An enum to track Reverie's skepticism towards Anastasia's leadership and the prophecy.
/// </summary>
public enum BeliefStatus
{
    Skeptic,
    Questioning,
    Believer
}

/// <summary>
/// Represents Reverie, a powerful and unpredictable Mage/Controller.
/// She challenges the status quo and shapes reality to her will.
/// </summary>
public class Reverie : Novamina
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// Tracks her evolving belief in the prophecy and the group's mission.
    /// This is part of her "Skeptic" archetype.
    /// </summary>
    public BeliefStatus CurrentBelief { get; private set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource that passively builds over time or with ability use,
    /// representing her growing influence over reality.
    /// </summary>
    public float Enigma { get; set; }

    /// <summary>
    /// The maximum amount of Enigma she can hold before it must be unleashed.
    /// </summary>
    public float MaxEnigma { get; private set; } = 100f;


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Reverie";
        Archetype = "Controller / Elemental Mage";
        maxHealth = 110; // Standard health for a mage.
        currentHealth = maxHealth;
        CurrentBelief = BeliefStatus.Skeptic; // Starts as a skeptic.
        Enigma = 0f; // Starts with no Enigma, must build it.
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// Placeholder for the "Reality Shift" ability. A versatile skill for
    /// mobility and environmental control.
    /// </summary>
    /// <param name="shiftType">"Teleport" for movement or "Manipulate" for environmental change.</param>
    public void UseRealityShift(string shiftType)
    {
        // This ability might not cost a resource, but have a cooldown instead.
        switch (shiftType.ToLower())
        {
            case "teleport":
                Debug.Log($"{characterName} bends reality, instantly teleporting to a new location.");
                // In-game logic for repositioning.
                break;
            case "manipulate":
                Debug.Log($"{characterName} warps the environment, creating a temporary barrier.");
                // In-game logic for environmental manipulation.
                break;
            default:
                Debug.LogWarning($"Unknown Reality Shift type: {shiftType}");
                break;
        }
    }
}