using System;
using UnityEngine;

/// <summary>
/// Represents Omega.one, a powerful entity created by Sky.ix.
/// Its allegiance is ambiguous, and its actions are guided by a cold, inscrutable logic.
/// </summary>
public class OmegaOne : Character
{
    // --- Core Character & Narrative Properties ---

    /// <summary>
    /// A reference to the creator of Omega.one.
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// The current primary directive guiding Omega.one's actions.
    /// This can be changed in response to battlefield conditions.
    /// </summary>
    [field: SerializeField]
    public Directive CurrentDirective { get; private set; }


    // --- Resource and Combat Properties ---

    /// <summary>
    /// A resource representing the computational power Omega.one allocates to its tasks.
    /// It regenerates at a steady rate and is consumed by its abilities.
    /// </summary>
    [field: SerializeField]
    public float ProcessingPower { get; private set; }

    /// <summary>
    /// The maximum Processing Power Omega.one can utilize at any given moment.
    /// </summary>
    [field: SerializeField]
    public float MaxProcessingPower { get; private set; } = 100f;


    // --- Initialization ---

    protected override void Awake()
    {
        base.Awake();
        characterName = "Omega.one";
        maxHealth = 180; // Highly durable, constructed being.
        currentHealth = maxHealth;
        CurrentDirective = Directive.Guardian; // Default directive is to protect its creator.
        ProcessingPower = MaxProcessingPower;
    }


    // --- Core Mechanic (Directive Switching) ---

    /// <summary>
    /// Changes Omega.one's operational directive to adapt to new combat situations.
    /// </summary>
    /// <param name="newDirective">The new directive to adopt.</param>
    public void SwitchDirective(Directive newDirective)
    {
        CurrentDirective = newDirective;
        Debug.Log($"{characterName} switches operational parameters. New directive: {newDirective}.");
    }


    // --- Abilities (Methods) ---

    /// <summary>
    /// A precise energy beam attack. Its effect can change based on the current directive.
    /// </summary>
    /// <param name="target">The enemy to target.</param>
    public void EnergyLance(Character target)
    {
        if (ProcessingPower >= 25)
        {
            ProcessingPower -= 25;
            Debug.Log($"{characterName} fires a concentrated energy lance at {target.characterName}.");

            switch (CurrentDirective)
            {
                case Directive.Guardian:
                    // In Guardian mode, the lance might also grant a small shield to its creator.
                    Debug.Log("...Residual energy forms a protective barrier around its creator.");
                    break;
                case Directive.Analysis:
                     // In Analysis mode, the lance reveals a weakness.
                    Debug.Log($"...Target analysis complete. {target.characterName} is now vulnerable to follow-up attacks.");
                    break;
                case Directive.Annihilation:
                    // In Annihilation mode, the lance simply deals more damage.
                     Debug.Log("...The lance burns with extreme intensity, causing massive damage.");
                    break;
            }
        }
    }

    /// <summary>
    /// An ability to overload its own systems for a massive, self-damaging area attack.
    /// This is a high-risk, high-reward ultimate ability.
    /// </summary>
    /// <param name="enemiesInRange">All enemies caught in the blast radius.</param>
    public void SystemOverload(System.Collections.Generic.List<Character> enemiesInRange)
    {
        // This ability might not cost a resource, but rather health.
        int healthCost = (int)(maxHealth * 0.30f); // Costs 30% of its max health.
        currentHealth -= healthCost;

        Debug.Log($"{characterName} diverts all power, initiating a system overload! It sacrifices {healthCost} health.");
        foreach(var enemy in enemiesInRange)
        {
            // In a real implementation, this would apply damage to each enemy.
            Debug.Log($"...A massive energy pulse radiates outwards, striking {enemy.characterName}!");
        }
    }
}