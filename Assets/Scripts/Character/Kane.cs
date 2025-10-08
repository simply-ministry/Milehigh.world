using UnityEngine;

public class Kane : Novamina
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Kane";
        Archetype = "Fallen Champion";
        maxHealth = 1700;
        currentHealth = maxHealth;
        attack = 190;
        defense = 90;
    }
}