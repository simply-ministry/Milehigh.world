using UnityEngine;

public class DelilahTheDesolate : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Delilah the Desolate";
        maxHealth = 1500;
        currentHealth = maxHealth;
        attack = 180;
        defense = 70;
    }
}