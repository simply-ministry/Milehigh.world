using UnityEngine;

public class Nafaerius : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Nafaerius";
        maxHealth = 2000;
        currentHealth = maxHealth;
        attack = 150;
        defense = 100;
    }
}