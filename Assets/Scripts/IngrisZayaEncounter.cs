using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngrisZayaEncounter : MonoBehaviour
{
    // --- Public References ---
    public ServerCombatManager serverCombatManager; // Reference to the server-side logic
    public GameObject ingris;
    public GameObject zaya;
    public Transform ingrisSwordTip;
    public Transform zayaArrowSpawn;
    public GameObject arrowPrefab; // Visual-only arrow
    public GameObject fireTrailPrefab; // Visual-only effect

    // --- Control Parameters ---
    public float fightDuration = 20f;
    public float dialoguePause = 2f;
    public float attackInterval = 2f;
    public float arrowSpeed = 20f;
    public float ingrisFireTrailDuration = 1f;

    // --- Private State ---
    private float timer;
    private bool fightActive = false;
    private Animator ingrisAnimator;
    private Animator zayaAnimator;

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

    IEnumerator Encounter()
    {
        // --- Dialogue Sequence ---
        Debug.Log("[CLIENT] Zaya: That symbol... I've seen it before. In my visions.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Ingris: Visions? Of fire and rebirth?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: And of a great darkness that threatens to consume all.");
        yield return new WaitForSeconds(dialoguePause);

        // --- Initiate Fight ---
        fightActive = true;
        timer = fightDuration;
        StartCoroutine(ClientFightSequence());
        yield return new WaitForSeconds(fightDuration);
        fightActive = false;

        // --- Realization Sequence ---
        Debug.Log("[CLIENT] Ingris: You're one of us.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: We fight for the same cause. Against the same enemy.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Ingris: Then perhaps, archer, we should fight together.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("[CLIENT] Zaya: I'd like that, Phoenix Warrior.");
    }

    IEnumerator ClientFightSequence()
    {
        // This coroutine now only *requests* attacks from the server.
        // The server is responsible for cooldowns and validation.
        while (fightActive && timer > 0)
        {
            yield return new WaitForSeconds(attackInterval);

            if (Random.value < 0.5f)
            {
                // Request Ingris's attack
                IngrisAttackRequest();
            }
            else
            {
                // Request Zaya's attack
                ZayaAttackRequest();
            }

            timer -= attackInterval;
        }
    }

    // --- Client-Side Attack Requests & Visuals ---

    void IngrisAttackRequest()
    {
        Debug.Log("[CLIENT] Ingris requests to attack.");

        // Trigger local visual effects immediately for responsiveness.
        ingrisAnimator.SetTrigger("Attack");
        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);

        // Send the attack request to the server for processing.
        serverCombatManager.HandleIngrisAttack(ingris);
    }

    void ZayaAttackRequest()
    {
        Debug.Log("[CLIENT] Zaya requests to attack.");

        // Trigger local visual effects.
        zayaAnimator.SetTrigger("Shoot");
        FireVisualArrow();

        // Send the attack request to the server.
        serverCombatManager.HandleZayaAttack(zaya, zayaArrowSpawn.forward);
    }

    void FireVisualArrow()
    {
        // This arrow is for visual feedback only. It has no collision or damage logic.
        GameObject arrow = Instantiate(arrowPrefab, zayaArrowSpawn.position, zayaArrowSpawn.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        if (arrowRb != null)
        {
            arrowRb.velocity = zayaArrowSpawn.forward * arrowSpeed;
            // Disable collision on the client-side arrow
            arrow.GetComponent<Collider>().enabled = false;
        }
        // The arrow will be destroyed automatically after a few seconds.
        Destroy(arrow, 5f);
    }
}