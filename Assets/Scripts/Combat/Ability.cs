using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Milehigh.World/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    [TextArea(3, 5)]
    public string description = "Ability Description";
    public float resourceCost = 10f;
    public float cooldownDuration = 1.0f;

    [Header("Damage Properties")]
    public int power = 10;
    public float critChance = 0.05f;
    public float critMultiplier = 2.0f;

    public virtual void Use(Character caster, Character target)
    {
        if (target == null) return;

        float totalDamage = CombatManager.CalculateDamage(caster, target, this);
        target.TakeDamage(totalDamage);

        Debug.Log($"{caster.characterName} uses {abilityName} on {target.characterName}, dealing {totalDamage} damage.");
    }
}