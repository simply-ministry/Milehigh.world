using UnityEngine;

/// <summary>
/// Represents Aeron The Brave, a classic Rogue/Scout who embodies
/// personal courage and freedom. He is an early-game companion who teaches
/// the player about stealth and individual rebellion.
/// </summary>
public class Aeron : NoveminaadHero
{
    protected override void Awake()
    {
        base.Awake();
        heroName = "Aeron The Brave";
        maxHealth = 150;
        maxEnergy = 100;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// A quick, precise strike that exploits an enemy's weakness.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} darts in, striking a vital point!");
        // TODO: Implement a high-critical-chance, low-energy attack.
    }

    /// <summary>
    /// Fades into the shadows, becoming difficult to target.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        Debug.Log($"{heroName} vanishes into the shadows!");
        // TODO: Apply a temporary stealth or high-evasion buff.
    }

    /// <summary>
    /// Unleashes a flurry of strikes, a testament to his rebellious spirit.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        Debug.Log($"{heroName} unleashes a 'Rebel's Flurry', a blur of motion and steel!");
        // TODO: Implement a rapid, multi-hit attack sequence on a single target.
    }
}