using UnityEngine;

public class TheOmen : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "The Omen";
        maxHealth = 1300;
        currentHealth = maxHealth;
        attack = 160;
        defense = 50;
        speed = 80f;
    }
}