// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using Milehigh.World.Core;

/// <summary>
/// Manages the special abilities and states for Ingris and Zaya during their fight.
/// This script acts as a state machine for their unique combat mechanics.
/// </summary>
public class IngrisZayaFight : MonoBehaviour
{
    [Header("Character References")]
    [Tooltip("The Animator component for the Ingris character.")]
    public Animator ingrisAnimator;
    [Tooltip("The Animator component for the Zaya character.")]
    public Animator zayaAnimator;

    [Header("State Flags")]
    [Tooltip("Can Ingris currently perform an attack?")]
    public bool ingrisCanAttack = true;
    [Tooltip("Can Zaya currently perform an attack?")]
    public bool zayaCanAttack = true;
    [Tooltip("Is the fight sequence currently active?")]
    public bool fightActive = false;

    [Header("Ingris Abilities")]
    [Tooltip("The damage multiplier for Ingris's charge attack.")]
    public float ingrisChargeAttackDamageMultiplier = 1.5f;
    private bool ingrisCharging = false;

    [Header("Zaya Abilities")]
    [Tooltip("The duration of Zaya's Focus Mode in seconds.")]
    public float zayaFocusModeDuration = 3f;
    private float zayaFocusModeTimer = 0f;
    private bool zayaInFocusMode = false;

    /// <summary>
    /// Called every frame. Manages the state of Ingris's charge and Zaya's focus abilities.
    /// </summary>
    void Update()
    {
        if (fightActive)
        {
            HandleZayaFocus();
        }
    }

    /// <summary>
    /// Initiates Ingris's charge attack.
    /// </summary>
    public void StartIngrisCharge()
    {
        if (ingrisCanAttack)
        {
            ingrisCharging = true;
            ingrisAnimator.SetTrigger("Charge");
            Debug.Log("Ingris is charging!");
        }
    }

    /// <summary>
    /// Handles the logic for Zaya's focus mode.
    /// </summary>
    void HandleZayaFocus()
    {
        if (zayaInFocusMode)
        {
            zayaFocusModeTimer -= Time.deltaTime;
            if (zayaFocusModeTimer <= 0f)
            {
                zayaInFocusMode = false;
                Debug.Log("Zaya's focus mode has ended.");
            }
        }
    }

    /// <summary>
    /// Activates Zaya's focus mode.
    /// </summary>
    public void ActivateZayaFocus()
    {
        if (zayaCanAttack)
        {
            zayaInFocusMode = true;
            zayaFocusModeTimer = zayaFocusModeDuration;
            zayaAnimator.SetTrigger("Focus");
            Debug.Log("Zaya enters focus mode! Accuracy increased.");
        }
    }

    /// <summary>
    /// Executes Ingris's attack, applying bonus damage if charging.
    /// </summary>
    /// <param name="zaya">The Zaya character's GameObject.</param>
    /// <param name="ingrisDamage">The base damage of the attack.</param>
    public void IngrisAttack(GameObject zaya, int ingrisDamage)
    {
        var health = zaya.GetComponent<IHealth>();
        if (health == null) return;

        if (ingrisCharging)
        {
            Debug.Log("Ingris lands a powerful charge attack!");
            health.TakeDamage(ingrisDamage * ingrisChargeAttackDamageMultiplier);
            ingrisCharging = false;
        }
        else
        {
            Debug.Log("Ingris performs a standard attack.");
            health.TakeDamage((float)ingrisDamage);
        }
    }

    /// <summary>
    /// Executes Zaya's attack, with a conceptual bonus for being in focus mode.
    /// </summary>
    /// <param name="ingris">The Ingris character's GameObject.</param>
    /// <param name="zayaArrowDamage">The base damage of the arrow.</param>
    public void ZayaAttack(GameObject ingris, int zayaArrowDamage)
    {
        var health = ingris.GetComponent<IHealth>();
        if (health == null) return;

        if (zayaInFocusMode)
        {
            Debug.Log("Zaya fires a focused, high-precision arrow!");
            health.TakeDamage((float)zayaArrowDamage);
        }
        else
        {
            Debug.Log("Zaya fires a standard arrow.");
            health.TakeDamage((float)zayaArrowDamage);
        }
    }
}