using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Milehigh.World.Core;

public class IngrisZayaFight : MonoBehaviour
{
    // These would be linked to the characters in the scene
    public Animator ingrisAnimator;
    public Animator zayaAnimator;
    public bool ingrisCanAttack = true;
    public bool zayaCanAttack = true;
    public bool fightActive = false;

    [Header("Ingris Abilities")]
    public float ingrisChargeAttackRange = 5f;
    public float ingrisChargeAttackDamageMultiplier = 1.5f;
    private bool ingrisCharging = false;

    [Header("Zaya Abilities")]
    public float zayaFocusModeDuration = 3f;
    public float zayaFocusModeAccuracyBonus = 0.2f; // Example accuracy bonus
    private float zayaFocusModeTimer = 0f;
    private bool zayaInFocusMode = false;

    void Update()
    {
        if (fightActive)
        {
            HandleIngrisCharge();
            HandleZayaFocus();
        }
    }

    void HandleIngrisCharge()
    {
        // Example: Shift key for charge
        if (ingrisCanAttack && Input.GetKeyDown(KeyCode.LeftShift))
        {
            ingrisCharging = true;
            ingrisAnimator.SetTrigger("Charge"); // Charge animation
            Debug.Log("Ingris is charging!");
            // In a full implementation, you would start charge movement and visual effects here.
        }

        if (ingrisCharging)
        {
            // In a real game, this would be a coroutine that moves the character
            // over a short duration. For this script, we'll simulate the end of the charge.
            Debug.Log("Ingris completes her charge!");
            // Assume the charge hits if the target is within range.
            // A real implementation would use Physics.OverlapSphere or similar.
            // For now, we'll just call the attack method directly.
            // The IngrisAttack method already contains the logic for increased damage.
            // We would need a reference to Zaya's GameObject to attack.
            // This logic is better handled in the main Encounter script.
            Debug.Log("Charge finished. Attack logic should be triggered in the encounter controller.");
            ingrisCharging = false; // Reset after the charge is complete
        }
    }

    void HandleZayaFocus()
    {
        // Example: Space key for focus
        if (zayaCanAttack && Input.GetKeyDown(KeyCode.Space))
        {
            zayaInFocusMode = true;
            zayaFocusModeTimer = zayaFocusModeDuration;
            zayaAnimator.SetTrigger("Focus"); // Focus animation
            Debug.Log("Zaya enters focus mode! Accuracy increased.");
            // In a full implementation, you would apply the accuracy bonus to her attacks.
        }

        if (zayaInFocusMode)
        {
            zayaFocusModeTimer -= Time.deltaTime;
            if (zayaFocusModeTimer <= 0f)
            {
                zayaInFocusMode = false;
                Debug.Log("Zaya's focus mode has ended.");
                // Remove accuracy bonus
            }
        }
    }

    // These methods would be called by the main encounter script to deal damage.
    // They are expanded to consider the special ability states.
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

    public void ZayaAttack(GameObject ingris, int zayaArrowDamage)
    {
        // The actual arrow firing logic would be handled by the Encounter script's
        // FireArrow method. This script only manages the state.
        if (zayaInFocusMode)
        {
            Debug.Log("Zaya fires a focused, high-precision arrow! (Accuracy bonus applied)");
            // In a real implementation, this might guarantee a hit or increase damage.
            var health = ingris.GetComponent<IHealth>();
            if (health != null) health.TakeDamage((float)zayaArrowDamage);
        }
        else
        {
            Debug.Log("Zaya fires a standard arrow.");
            var health = ingris.GetComponent<IHealth>();
            if (health != null) health.TakeDamage((float)zayaArrowDamage);
        }
    }
}