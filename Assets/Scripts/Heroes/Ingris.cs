using UnityEngine;

/// <summary>
/// Represents Ingris the Phoenix Warrior, the paragon of the Techno-Mystic hybrid.
/// Her existence proves that technology and Void energy can coexist, and she guides
/// the player toward the "Balance Ending."
/// </summary>
public class Ingris : NoveminaadHero
{
    [Header("Phoenix Warrior Traits")]
    /// <summary>Indicates whether the Phoenix Protocol is active.</summary>
    public bool isPhoenixProtocolActive = false;

    /// <summary>
    /// Initializes Ingris's specific attributes.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        heroName = "Ingris the Phoenix Warrior";
        maxHealth = 180;
        maxEnergy = 150; // High energy for her dual abilities
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    /// <summary>
    /// Strikes with a Sun-Forged Blade, dealing both physical and energy damage.
    /// This ability represents the fusion of her martial prowess and mystical power.
    /// </summary>
    public override void UsePrimaryAbility()
    {
        base.UsePrimaryAbility();
        Debug.Log($"{heroName} strikes with her Sun-Forged Blade, searing her foe!");
        // TODO: Implement a melee attack that deals hybrid damage.
    }

    /// <summary>
    /// Erects a kinetic barrier, absorbing incoming attacks.
    /// This represents her technological, Juggernaut-like heritage.
    /// </summary>
    public override void UseSecondaryAbility()
    {
        base.UseSecondaryAbility();
        currentState = HeroState.Defending;
        Debug.Log($"{heroName} erects a shimmering Kinetic Barrier!");
        // TODO: Implement a temporary shield or damage absorption buff.
    }

    /// <summary>
    /// Activates the Phoenix Protocol. If she falls in battle while this is active,
    /// she will be reborn from the ashes, restored to a portion of her health.
    /// </summary>
    public override void UseUltimateAbility()
    {
        base.UseUltimateAbility();
        isPhoenixProtocolActive = true;
        Debug.Log($"{heroName} activates the Phoenix Protocol. Her heart burns with cosmic fire!");
        // TODO: Apply a status effect that triggers a self-revive on death.
    }

    /// <summary>
    /// Overridden death logic to account for the Phoenix Protocol.
    /// </summary>
    protected override void Die()
    {
        if (isPhoenixProtocolActive)
        {
            isPhoenixProtocolActive = false;
            currentHealth = maxHealth / 2; // Revive with 50% health
            currentState = HeroState.Idle;
            Debug.Log($"{heroName} is consumed by fire and reborn from the ashes!");
            // TODO: Play rebirth VFX/SFX.
        }
        else
        {
            base.Die();
        }
    }
}
