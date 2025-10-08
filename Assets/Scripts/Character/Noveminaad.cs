using UnityEngine;

public abstract class Noveminaad : MonoBehaviour
{
    public string CharacterName;
    public int Level = 1;
    public float Health = 100f;
    public float Mana = 50f;
    public float MoveSpeed = 5f;
    public float AttackPower = 20f;
    public float Defense = 10f;

    public abstract void UseSpecialAbility();

    public virtual void Move(Vector3 direction)
    {
        transform.position += direction * MoveSpeed * Time.deltaTime;
    }

    public virtual void Attack()
    {
        Debug.Log(CharacterName + " attacks for " + AttackPower + " damage!");
    }

    public virtual void TakeDamage(float amount)
    {
        Health -= Mathf.Max(0, amount - Defense);
        Debug.Log(CharacterName + " takes damage. Health now: " + Health);
        if (Health <= 0) Die();
    }

    protected virtual void Die()
    {
        Debug.Log(CharacterName + " has fallen.");
    }
}