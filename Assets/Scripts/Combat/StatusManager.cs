using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public void ApplyEffect(StatusEffectType effectType, float duration)
    {
        // Placeholder for applying a status effect
        Debug.Log($"Applying {effectType} for {duration} seconds.");
    }

    public void ApplyEffect(StatusEffectType effectType, float duration, float value)
    {
        // Placeholder for applying a status effect
        Debug.Log($"Applying {effectType} for {duration} seconds with value {value}.");
    }

    public void ApplyEffect(StatusEffectType effectType, int stacks)
    {
        // Placeholder for applying a status effect
        Debug.Log($"Applying {effectType} with {stacks} stacks.");
    }

    public void ApplyRandomDebuff()
    {
        // Placeholder for applying a random debuff
        Debug.Log("Applying a random debuff.");
    }
}

public enum StatusEffectType
{
    Burn,
    Stun,
    GuaranteedCrits,
    DragonForm,
    Invisibility,
    Untargetable,
    Confuse,
    Empower,
    Sleep,
    Lifesteal,
    Vulnerability,
    ChainLightning,
    DamageUp,
    Judgement,
    Invulnerable,
    ArmorBreak,
    WeaknessExposed,
    Execute,
    Evasion
}
