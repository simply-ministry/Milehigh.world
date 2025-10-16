using System;
using UnityEngine;

/// <summary>
/// Represents Zaia the Just, one of the Ɲōvəmîŋāđ.
/// She operates as a Rogue/Assassin, specializing in stealth, mobility, and high-precision strikes.
/// </summary>
public class Zaia : Novamina
{
    // --- Narrative Properties ---

    /// <summary>
    /// A reference to Aeron, whom she awaits the gathering of the Ɲōvəmîŋāđ with.
    /// This suggests a close bond, perhaps as a loyal guardian or confidante.
    /// </summary>
    public Guid AeronCompanionId { get; set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource that builds as Zaia moves and successfully attacks.
    /// It is spent on her advanced evasive and offensive abilities.
    /// </summary>
    [field: SerializeField]
    public float Momentum { get; private set; }

    /// <summary>
    /// The maximum amount of Momentum Zaia can build.
    /// </summary>
    [field: SerializeField]
    public float MaxMomentum { get; private set; } = 100f;

    /// <summary>
    /// A flag indicating if Zaia is currently in a stealth state.
    /// </summary>
    public bool IsStealthed { get; private set; }


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Zaia";
        Archetype = "Rogue/Assassin";
        maxHealth = 130; // Agile but not overly fragile.
        currentHealth = maxHealth;
        Momentum = 0f;
        IsStealthed = false;
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// Placeholder for a basic, quick attack that generates Momentum.
    /// </summary>
    /// <param name="target">The enemy to strike.</param>
    public void SwiftStrike(Character target)
    {
        float momentumGained = 15f;
        Momentum = Mathf.Min(MaxMomentum, Momentum + momentumGained);
        Debug.Log($"{characterName} strikes {target.characterName} with blinding speed! (+{momentumGained} Momentum)");
        // ... damage logic ...
    }

    /// <summary>
    /// Enters a stealth mode, making Zaia invisible to enemies.
    /// The first attack from stealth deals bonus damage.
    /// This ability likely consumes Momentum over time or has a set duration.
    /// </summary>
    public void ShadowVanish()
    {
        if (Momentum >= 30)
        {
            Momentum -= 30;
            IsStealthed = true;
            Debug.Log($"{characterName} spends Momentum to vanish into the shadows.");
            // In-game logic would make her untargetable by enemies.
        }
    }

    /// <summary>
    /// A high-damage, precision attack that consumes a large amount of Momentum.
    /// Deals significantly more damage if used while stealthed.
    /// </summary>
    /// <param name="target">The enemy to assassinate.</param>
    public void ExploitWeakness(Character target)
    {
        if (Momentum >= 50)
        {
            Momentum -= 50;
            Debug.Log($"{characterName} analyzes {target.characterName} and strikes at a vital point!");

            if (IsStealthed)
            {
                IsStealthed = false; // Attacking breaks stealth.
                Debug.Log("...The attack from the shadows is a devastating critical hit!");
                // ... logic for very high bonus damage ...
            }
            else
            {
                // ... logic for standard high damage ...
            }
        }
    }

    /// <summary>
    /// An evasive maneuver that allows Zaia to quickly reposition and avoid damage.
    /// </summary>
    public void EvasiveDash()
    {
        // This might have a cooldown instead of a resource cost to ensure it's always available for defense.
        Debug.Log($"{characterName} performs a nimble dash, evading incoming attacks.");
        // In-game logic would move her character quickly and grant a brief moment of invulnerability.
    }
}