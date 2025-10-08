using UnityEngine;

public class Lucent : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Lucent";
        maxHealth = 1200;
        currentHealth = maxHealth;
        attack = 80;
        defense = 60;
    }
}