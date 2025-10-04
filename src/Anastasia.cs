using UnityEngine;
using System.Collections.Generic;

public class Anastasia : Character
{
    public int mana = 100;
    public int maxMana = 100;
    private float dreamWeavingCooldown = 2f;
    private float lastDreamWeavingTime;
    private float astralProjectionCooldown = 3f;
    private float lastAstralProjectionTime;
    private float manaRegenRate = 5f; // Mana per second
    private float manaAccumulator = 0f;

    protected override void Awake()
    {
        base.Awake();
        characterName = "Anastasia";
        maxHealth = 80;
        currentHealth = maxHealth;
        attack = 60;
        defense = 40;
        speed = 70f;

        lastDreamWeavingTime = -dreamWeavingCooldown;
        lastAstralProjectionTime = -astralProjectionCooldown;
    }

    private void Update()
    {
        RegenerateMana();
    }

    private void RegenerateMana()
    {
        manaAccumulator += manaRegenRate * Time.deltaTime;
        if (manaAccumulator >= 1f)
        {
            int regenAmount = Mathf.FloorToInt(manaAccumulator);
            mana = Mathf.Min(mana + regenAmount, maxMana);
            manaAccumulator -= regenAmount;
        }
    }

    public void DreamWeaving(Character target, string effect)
    {
        if (Time.time >= lastDreamWeavingTime + dreamWeavingCooldown)
        {
            if (mana >= 20)
            {
                mana -= 20;
                if (effect == "heal")
                {
                    target.Heal(50);
                    Debug.Log($"{characterName} uses Dream Weaving to heal {target.characterName}!");
                }
                else if (effect == "sleep")
                {
                    target.speed -= 20f; // This is a simplistic approach. A real implementation would use a status effect system.
                    Debug.Log($"{characterName} uses Dream Weaving to slow down {target.characterName}!");
                }
                lastDreamWeavingTime = Time.time;
            }
            else
            {
                Debug.Log($"{characterName} doesn't have enough mana!");
            }
        }
    }

    public void AstralProjection(Character target)
    {
        if (Time.time >= lastAstralProjectionTime + astralProjectionCooldown)
        {
            if (mana >= 30)
            {
                mana -= 30;
                int damage = attack + 30; // Magic attack
                target.TakeDamage(damage);
                Debug.Log($"{characterName} uses Astral Projection on {target.characterName} for {damage} damage!");
                lastAstralProjectionTime = Time.time;
            }
            else
            {
                Debug.Log($"{characterName} doesn't have enough mana!");
            }
        }
    }
}