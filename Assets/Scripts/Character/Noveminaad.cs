using UnityEngine;

/// <summary>
/// An abstract base class for all "Noveminaad" characters.
/// It defines the fundamental attributes and behaviors that all characters of this type share,
/// such as health, mana, movement, and combat actions.
/// </summary>
public abstract class Noveminaad : MonoBehaviour
{
    /// <summary>
    /// The name of the character.
    /// </summary>
    public string CharacterName;
    /// <summary>
    /// The character's current level.
    /// </summary>
    public int Level = 1;
    /// <summary>
    /// The character's current health points.
    /// </summary>
    public float Health = 100f;
    /// <summary>
    /// The character's current mana or energy points.
    /// </summary>
    public float Mana = 50f;
    /// <summary>
    /// The speed at which the character moves.
    /// </summary>
    public float MoveSpeed = 5f;
    /// <summary>
    /// The character's base attack power.
    /// </summary>
    public float AttackPower = 20f;
    /// <summary>
    /// The character's base defense value, used to mitigate incoming damage.
    /// </summary>
    public float Defense = 10f;

    /// <summary>
    /// A placeholder for a character's unique special ability.
    /// This method must be implemented by all inheriting classes.
    /// </summary>
    public abstract void UseSpecialAbility();

    /// <summary>
    /// Moves the character in a specified direction.
    /// </summary>
    /// <param name="direction">The direction vector in which to move the character.</param>
    public virtual void Move(Vector3 direction)
    {
        transform.position += direction * MoveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Performs a basic attack, logging the action to the console.
    /// </summary>
    public virtual void Attack()
    {
        Debug.Log(CharacterName + " attacks for " + AttackPower + " damage!");
    }

    /// <summary>
    /// Reduces the character's health by a specified amount, factoring in defense.
    /// </summary>
    /// <param name="amount">The incoming raw damage amount.</param>
    public virtual void TakeDamage(float amount)
    {
        Health -= Mathf.Max(0, amount - Defense);
        Debug.Log(CharacterName + " takes damage. Health now: " + Health);
        if (Health <= 0) Die();
    }

    /// <summary>
    /// Handles the character's death when health reaches zero.
    /// </summary>
    protected virtual void Die()
    {
        Debug.Log(CharacterName + " has fallen.");
    }
}