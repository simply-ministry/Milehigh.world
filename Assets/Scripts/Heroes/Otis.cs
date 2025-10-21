// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Represents Otis the Skywanderer, the tenth and final Ɲōvəmîŋāđ.
/// As the protégé of the Dragon King Cirrus, Otis is a versatile combatant
/// who learns to master the skies and command the wind. His journey is one of
/// discovery, mentorship, and fulfilling a latent, unknown power.
/// </summary>
public class Otis : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Otis the Skywanderer";
        maxHealth = 120; // A balance of durability and agility
        maxEnergy = 120;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        Debug.Log($"{heroName} has been initialized and is ready for adventure!");
    }

    /// <summary>
    /// A swift gust of wind that pushes enemies back and deals minor damage.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} summons a 'Gusting Strike'!");
        // Simulate dealing 10 damage and applying a knockback effect.
        // In a real implementation, this would target an enemy and apply forces.
        Debug.Log("Deals 10 damage and pushes the target back.");
    }

    /// <summary>
    /// Calls upon favorable winds to increase his and his allies' speed.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        Debug.Log($"{heroName} summons 'Tailwind'!");
        // Simulate applying a 20% speed buff for 10 seconds.
        // In a real implementation, this would affect the hero's movement and attack speed attributes.
        Debug.Log("Applies a 20% speed buff to self and nearby allies for 10 seconds.");
    }

    /// <summary>
    * Unleashes the 'Tempest's Eye', a swirling vortex that pulls in and damages enemies.
    * This is the awakening of the power inherited from his mentor, Cirrus.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        Debug.Log($"{heroName} unleashes the 'Tempest's Eye'!");
        // Simulate creating a vortex that pulls in enemies and deals 15 damage per second for 5 seconds.
        // In a real implementation, this would spawn a physics-based vortex object.
        Debug.Log("Creates a vortex that pulls in enemies and deals damage over time.");
    }
}