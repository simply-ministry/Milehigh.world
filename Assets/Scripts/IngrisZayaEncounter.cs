// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the client-side logic for the Ingris vs. Zaya narrative encounter.
/// This script handles dialogue, triggers animations, and sends attack requests to a server-side manager.
/// The actual combat logic (damage, cooldowns) is handled by the ServerCombatManager.
/// </summary>
public class IngrisZayaEncounter : MonoBehaviour
{
    [Header("Network References")]
    [Tooltip("Reference to the server-side combat logic manager.")]
    public ServerCombatManager serverCombatManager;

    [Header("Character References")]
    [Tooltip("The GameObject for the Ingris character.")]
    public GameObject ingris;
    [Tooltip("The GameObject for the Zaya character.")]
    public GameObject zaya;

    [Header("Visual Effect References")]
    [Tooltip("The transform representing the tip of Ingris's sword for VFX.")]
    public Transform ingrisSwordTip;
    [Tooltip("The transform from which Zaya's visual-only arrows are spawned.")]
    public Transform zayaArrowSpawn;
    [Tooltip("The prefab for the visual-only arrow fired by Zaya.")]
    public GameObject arrowPrefab;
    [Tooltip("The prefab for the fire trail visual effect on Ingris's sword.")]
    public GameObject fireTrailPrefab;

    [Header("Encounter Settings")]
    [Tooltip("The total duration of the scripted fight sequence in seconds.")]
    public float fightDuration = 20f;
    [Tooltip("The pause duration in seconds between lines of dialogue.")]
    public float dialoguePause = 2f;
    [Tooltip("The interval at which attack requests are sent during the fight.")]
    public float attackInterval = 2f;
    [Tooltip("The speed of the visual-only arrows.")]
    public float arrowSpeed = 20f;
    [Tooltip("The duration of the fire trail visual effect.")]
    public float ingrisFireTrailDuration = 1f;

    // --- Private State ---
    private Animator ingrisAnimator;
    private Animator zayaAnimator;

    /// <summary>
    /// Initializes the encounter, gets component references, and starts the main sequence.
    /// </summary>
    void Start()
    {
        ingrisAnimator = ingris.GetComponent<Animator>();
        zayaAnimator = zaya.GetComponent<Animator>();

        if (serverCombatManager == null)
        {
            Debug.LogError("ServerCombatManager not assigned!");
            return;
        }

        StartCoroutine(Encounter());
    }

    /// <summary>
    /// The main coroutine that controls the flow of the entire encounter.
    /// </summary>
    private IEnumerator Encounter()
    {
        // --- Dialogue Sequence ---
        Debug.Log("[CLIENT] Zaya: That symbol... I've seen it before. In my visions.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Ingris: Visions? Of fire and rebirth?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: And of a great darkness that threatens to consume all.");
        yield return new WaitForSeconds(dialoguePause);

        // --- Initiate Fight ---
        StartCoroutine(ClientFightSequence());
        yield return new WaitForSeconds(fightDuration);

        // --- Realization Sequence ---
        Debug.Log("[CLIENT] Ingris: You're one of us.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: We fight for the same cause. Against the same enemy.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Ingris: Then perhaps, archer, we should fight together.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: I'd like that, Phoenix Warrior.");
    }

    /// <summary>
    /// Coroutine that sends attack requests to the server at regular intervals.
    /// </summary>
    private IEnumerator ClientFightSequence()
    {
        float fightEndTime = Time.time + fightDuration;
        float nextAttackTime = 0f;

        while (Time.time < fightEndTime)
        {
            if (Time.time > nextAttackTime)
            {
                if (Random.value < 0.5f)
                {
                    IngrisAttackRequest();
                }
                else
                {
                    ZayaAttackRequest();
                }
                nextAttackTime = Time.time + attackInterval;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Triggers local visual effects for Ingris's attack and sends a request to the server.
    /// </summary>
    private void IngrisAttackRequest()
    {
        Debug.Log("[CLIENT] Ingris requests to attack.");
        ingrisAnimator.SetTrigger("Attack");
        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);
        serverCombatManager.HandleIngrisAttack(ingris);
    }

    /// <summary>
    /// Triggers local visual effects for Zaya's attack and sends a request to the server.
    /// </summary>
    private void ZayaAttackRequest()
    {
        Debug.Log("[CLIENT] Zaya requests to attack.");
        zayaAnimator.SetTrigger("Shoot");
        FireVisualArrow();
        serverCombatManager.HandleZayaAttack(zaya, zayaArrowSpawn.forward);
    }

    /// <summary>
    /// Instantiates a visual-only arrow prefab that does not have collision.
    /// </summary>
    private void FireVisualArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, zayaArrowSpawn.position, zayaArrowSpawn.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        if (arrowRb != null)
        {
            arrowRb.velocity = zayaArrowSpawn.forward * arrowSpeed;
            arrow.GetComponent<Collider>().enabled = false;
        }
        Destroy(arrow, 5f);
    }
}