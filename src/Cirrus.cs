using UnityEngine;

public class Cirrus : Character
{
    public bool dragonForm = false;
    public int rage = 0;
    private float dragonFormCooldown = 5f;
    private float lastDragonFormToggleTime;
    private float elementalBreathCooldown = 3f;
    private float lastElementalBreathTime;
    private float draconicFuryCooldown = 7f;
    private float lastDraconicFuryTime;
    private float rageGainOnDamage = 10f;
    private float rageDecayRate = 2f; // Rage lost per second
    private float rageAccumulator = 0f;

    protected override void Awake()
    {
        base.Awake();
        characterName = "Cirrus";
        maxHealth = 120;
        currentHealth = maxHealth;
        attack = 80;
        defense = 70;
        speed = 60f;

        lastDragonFormToggleTime = -dragonFormCooldown;
        lastElementalBreathTime = -elementalBreathCooldown;
        lastDraconicFuryTime = -draconicFuryCooldown;
    }

    private void Update()
    {
        DecayRage();
    }

    private void DecayRage()
    {
        if (rage > 0)
        {
            rageAccumulator += rageDecayRate * Time.deltaTime;
            if (rageAccumulator >= 1f)
            {
                int decayAmount = Mathf.FloorToInt(rageAccumulator);
                rage = Mathf.Max(0, rage - decayAmount);
                rageAccumulator -= decayAmount;
            }
        }
    }

    public void DragonFormToggle()
    {
        if (Time.time >= lastDragonFormToggleTime + dragonFormCooldown)
        {
            dragonForm = !dragonForm;
            if (dragonForm)
            {
                attack += 20;
                defense += 20;
                Debug.Log($"{characterName} transforms into Dragon Form!");
            }
            else
            {
                attack -= 20;
                defense -= 20;
                Debug.Log($"{characterName} returns to human form!");
            }
            lastDragonFormToggleTime = Time.time;
        }
    }

    public void ElementalBreath(Character target, string element)
    {
        if (Time.time >= lastElementalBreathTime + elementalBreathCooldown)
        {
            if (dragonForm)
            {
                int damage = attack + 40; // Dragon form buff
                target.TakeDamage(damage);
                Debug.Log($"{characterName} uses {element} Breath on {target.characterName} for {damage} damage!");
                rage = Mathf.Min(rage + 10, 100);
                Debug.Log($"{characterName}'s Rage increases!");
                lastElementalBreathTime = Time.time;
            }
            else
            {
                Debug.Log($"{characterName} needs to be in Dragon Form to use this!");
            }
        }
    }

    public void DraconicFury(Character target)
    {
        if (Time.time >= lastDraconicFuryTime + draconicFuryCooldown)
        {
            int damage = attack + rage; // Rage buff
            target.TakeDamage(damage);
            Debug.Log($"{characterName} unleashes Draconic Fury on {target.characterName} for {damage} damage!");
            rage = 0; // Consume rage
            lastDraconicFuryTime = Time.time;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (damage > 0)
        {
            int rageGain = Mathf.RoundToInt(rageGainOnDamage * damage / maxHealth);
            rage = Mathf.Min(rage + rageGain, 100); // Scale rage gain by damage taken
            Debug.Log($"{characterName}'s Rage increases!");
        }
    }
}