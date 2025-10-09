using UnityEngine;

/// <summary>
/// An enum to track Otis's narrative progression from antagonist to hero.
/// </summary>
public enum RedemptionStatus
{
    Vengeful,
    Conflicted,
    Redeemed
}

/// <summary>
/// Represents Otis the Skywanderer, also known as X. A former astronaut turned vengeful ranger.
/// His gameplay is defined by high mobility and increased damage output as he takes risks.
/// </summary>
public class Otis : Novamina
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// The alias he uses during his quest for revenge.
    /// </summary>
    public string Alias { get; private set; } = "X the Skywanderer";

    /// <summary>
    /// Tracks his character arc and redemption. This could unlock different
    /// dialogue or abilities at different stages.
    /// </summary>
    public RedemptionStatus ArcStatus { get; private set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// Otis doesn't use a traditional resource like mana. Instead, his power is
    /// tied to his "Grit". This property calculates a damage multiplier based on his missing health.
    /// This directly implements the "Wanderer's Grit" ability.
    /// </summary>
    public float WanderersGritMultiplier
    {
        get
        {
            // The lower his health, the higher the multiplier.
            // Example: At 50% health, multiplier is 1.5. At 10% health, it's 1.9.
            if (maxHealth == 0) return 1.0f; // Avoid division by zero
            return 1.0f + (1.0f - ((float)currentHealth / maxHealth));
        }
    }


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Otis";
        Archetype = "Agile DPS / Scout / Manipulator";
        maxHealth = 120; // Moderately durable for a ranged character.
        currentHealth = maxHealth;
        ArcStatus = RedemptionStatus.Vengeful; // Starts his story driven by revenge.
    }


    // --- Narrative Methods ---

    /// <summary>
    /// A narrative method to advance Otis's character arc. This would be
    /// called after completing a key story mission or a significant character interaction.
    /// </summary>
    public void AdvanceRedemptionArc()
    {
        if (ArcStatus == RedemptionStatus.Vengeful)
        {
            ArcStatus = RedemptionStatus.Conflicted;
            Debug.Log($"[NARRATIVE] {name} begins to question his path of vengeance...");
        }
        else if (ArcStatus == RedemptionStatus.Conflicted)
        {
            ArcStatus = RedemptionStatus.Redeemed;
            Debug.Log($"[NARRATIVE] {name} has let go of his hatred and now fights for a new cause!");
            // This could potentially modify his abilities, e.g., giving them a supportive element.
        }
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// Placeholder for the "Skyfire" ability. A ranged attack whose damage is
    /// amplified by his Wanderer's Grit.
    /// </summary>
    /// <param name="target">The enemy to attack.</param>
    public void UseSkyfire(Character target)
    {
        // The core of his gameplay: base damage is multiplied by his current Grit.
        float baseDamage = 20f;
        float finalDamage = baseDamage * WanderersGritMultiplier;

        Debug.Log($"{name} unleashes Skyfire at {target.name} for {finalDamage:F1} damage! (Grit Multiplier: {WanderersGritMultiplier:F2}x)");
        // In-game logic to apply the finalDamage to the target would go here.
        // For example: target.TakeDamage(this, finalDamage);
    }

    /// <summary>
    /// Placeholder for the "Gravity Manipulation" ability. A utility skill
    /// used for mobility or enemy control.
    /// </summary>
    /// <param name="target">The character to be affected.</param>
    /// <param name="mode">"Propel" for self-movement, "Crush" for enemy debuff.</param>
    public void ManipulateGravity(Character target, string mode)
    {
        switch (mode.ToLower())
        {
            case "propel":
                if (target == this)
                {
                    // In-game logic would move Otis quickly to a new location (dash/teleport).
                    Debug.Log($"{name} reverses gravity on himself, propelling to a new position!");
                }
                break;
            case "crush":
                // In-game logic would apply a "slow" or "rooted" status effect to the enemy.
                Debug.Log($"{name} intensifies gravity around {target.name}, slowing them down!");
                break;
        }
    }
}