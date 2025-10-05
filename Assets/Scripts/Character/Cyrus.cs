using UnityEngine;

public class Cyrus : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Cyrus";
        maxHealth = 1800;
        currentHealth = maxHealth;
        attack = 200;
        defense = 80;
    }
}