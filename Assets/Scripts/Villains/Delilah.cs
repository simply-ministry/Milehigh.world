using UnityEngine;

/// <summary>
/// Represents Delilah the Desolate, the tragic transformation of Ingris
/// after succumbing to the Void's emptiness. She is a powerful and
/// emotionally charged boss encounter.
/// </summary>
public class Delilah : ShadowSyndicateVillain
{
    [Header("Desolate Soul")]
    [TextArea(2, 4)]
    /// <summary>A line of dialogue Delilah speaks.</summary>
    public string dialogue = "The Phoenix has burned out... only the ashes remain.";

    /// <summary>
    /// Initializes Delilah's specific attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        villainName = "Delilah the Desolate";
        maxHealth = 4000; // High health for a major boss
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Strikes with a Void-Corrupted Blade, draining the life from her target.
    /// A twisted version of Ingris's Sun-Forged Blade.
    /// </summary>
    /// <param name="target">The hero to be targeted.</param>
    public override void UsePrimaryAbility(GameObject target)
    {
        base.UsePrimaryAbility(target);
        Debug.Log($"{villainName} strikes with her Void-Corrupted Blade, whispering, 'Feel the emptiness!'");
        // TODO: Implement a melee attack that leeches health from the target.
    }

    /// <summary>
    /// Unleashes a wave of pure despair, a corrupted version of Ingris's inspiring presence.
    /// This ability instills hopelessness in her enemies.
    /// </summary>
    public void CastDespair()
    {
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName}: '{dialogue}'");
        Debug.Log($"{villainName} casts a wave of despair!");
        // TODO: Apply a potent debuff (e.g., attack power and defense down) to all heroes.
    }

    /// <summary>
    /// Shatters her own essence in a final, desperate act, dealing massive damage
    /// to all nearby. A dark reflection of the Phoenix Protocol's rebirth.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        currentState = VillainAIState.Casting;
        Debug.Log($"{villainName} screams, 'If I must be nothing, I will take you with me!'");
        // TODO: Implement a massive, close-range AoE explosion upon her own death or at low health.
    }
}
