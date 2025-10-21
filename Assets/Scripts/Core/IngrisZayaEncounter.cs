// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages a complex story encounter between Ingris and Zaya, involving dialogue, a timed fight, and a final discovery sequence.
/// This script coordinates animations, attacks, and narrative progression.
/// </summary>
public class IngrisZayaEncounter : MonoBehaviour
{
    [Header("Object References")]
    /// <summary>
    /// The GameObject representing the Ingris character.
    /// </summary>
    public GameObject ingris;
    /// <summary>
    /// The GameObject representing the Zaya character.
    /// </summary>
    public GameObject zaya;
    /// <summary>
    /// The transform representing the tip of Ingris's sword, used as an origin for attacks.
    /// </summary>
    public Transform ingrisSwordTip;
    /// <summary>
    /// The point from which Zaya's arrows are spawned.
    /// </summary>
    public Transform zayaArrowSpawn;
    /// <summary>
    /// A transform indicating where the Noveminaad symbol should appear.
    /// </summary>
    public Transform symbolSpawnPoint;
    /// <summary>
    /// Reference to the dialogue manager for this scene.
    /// </summary>
    public IngrisZayaDialogue dialogueManager;

    [Header("Prefabs")]
    /// <summary>
    /// The prefab for the arrow Zaya fires.
    /// </summary>
    public GameObject arrowPrefab;
    /// <summary>
    /// The prefab for the fire trail effect on Ingris's sword.
    /// </summary>
    public GameObject fireTrailPrefab;
    /// <summary>
    /// The prefab for the Noveminaad symbol that appears during the discovery sequence.
    /// </summary>
    public GameObject symbolPrefab;

    [Header("Encounter Settings")]
    /// <summary>
    /// The total duration of the scripted fight in seconds.
    /// </summary>
    public float fightDuration = 20f;
    /// <summary>
    /// The duration to pause between lines of dialogue.
    /// </summary>
    public float dialoguePause = 2f;
    /// <summary>
    /// The time to display the Noveminaad symbol.
    /// </summary>
    public float symbolDisplayTime = 5f;

    [Header("Ingris Settings")]
    /// <summary>
    /// The range of Ingris's melee sword attack.
    /// </summary>
    public float ingrisSwordAttackRange = 3f;
    /// <summary>
    /// How long the fire trail effect lasts in seconds.
    /// </summary>
    public float ingrisFireTrailDuration = 1f;
    /// <summary>
    /// The damage dealt by Ingris's attack.
    /// </summary>
    public int ingrisDamage = 20;
    /// <summary>
    /// The cooldown period for Ingris's attack in seconds.
    /// </summary>
    public float ingrisAttackCooldown = 1f;

    [Header("Zaya Settings")]
    /// <summary>
    /// The speed of Zaya's arrows.
    /// </summary>
    public float arrowSpeed = 20f;
    /// <summary>
    /// The maximum distance an arrow can travel before being destroyed.
    /// </summary>
    public float zayaMaxAttackDistance = 15f;
    /// <summary>
    /// The damage dealt by Zaya's arrow.
    /// </summary>
    public int zayaArrowDamage = 15;
    /// <summary>
    /// The cooldown period for Zaya's attack in seconds.
    /// </summary>
    public float zayaAttackCooldown = 0.5f;

    // --- Private State ---
    private bool fightActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();
    private float ingrisAttackTimer;
    private float zayaAttackTimer;

    // --- Component References ---
    private Animator ingrisAnimator;
    private Animator zayaAnimator;

    /// <summary>
    /// Initializes the component, gets animator references, and subscribes to the dialogue end event.
    /// </summary>
    void Start()
    {
        ingrisAnimator = ingris.GetComponent<Animator>();
        zayaAnimator = zaya.GetComponent<Animator>();

        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue Manager has not been assigned in the Inspector!");
            return;
        }

        // Subscribe to the dialogue completion event to start the fight
        IngrisZayaDialogue.OnDialogueEnd += HandleDialogueEnd;

        // Start the initial dialogue
        StartCoroutine(dialogueManager.StartDialogue());
    }

    /// <summary>
    /// Unsubscribes from the dialogue event when the object is destroyed to prevent memory leaks.
    /// </summary>
    void OnDestroy()
    {
        IngrisZayaDialogue.OnDialogueEnd -= HandleDialogueEnd;
    }

    /// <summary>
    /// Called every frame. Updates cooldown timers and arrow positions during the fight.
    /// </summary>
    void Update()
    {
        if (fightActive)
        {
            ingrisAttackTimer += Time.deltaTime;
            zayaAttackTimer += Time.deltaTime;
            UpdateArrows();
        }
    }

    /// <summary>
    /// Event handler that is called when the initial dialogue ends. It starts the main encounter sequence.
    /// </summary>
    private void HandleDialogueEnd()
    {
        StartCoroutine(FightAndConclusion());
    }

    /// <summary>
    /// Main coroutine that controls the flow of the encounter after the initial dialogue.
    /// It manages the fight sequence and the subsequent discovery scene.
    /// </summary>
    private IEnumerator FightAndConclusion()
    {
        Debug.Log("Dialogue over. The fight begins!");
        fightActive = true;
        ingrisAttackTimer = ingrisAttackCooldown; // Allow immediate attack
        zayaAttackTimer = zayaAttackCooldown;   // Allow immediate attack

        // Run the fight for the specified duration
        yield return StartCoroutine(FightSequence());

        fightActive = false;
        Debug.Log("The fight has ended.");

        // Clean up any remaining arrows after the fight
        CleanUpArrows();

        // Start the final part of the cutscene
        yield return StartCoroutine(NoveminadDiscoverySequence());
    }

    /// <summary>
    /// Coroutine that manages the combat loop, triggering attacks based on cooldowns.
    /// </summary>
    private IEnumerator FightSequence()
    {
        float encounterTimer = fightDuration;
        while (encounterTimer > 0)
        {
            // Ingris attacks if her cooldown is ready
            if (ingrisAttackTimer >= ingrisAttackCooldown)
            {
                IngrisAttack();
                ingrisAttackTimer = 0f;
            }

            // Zaya attacks if her cooldown is ready
            if (zayaAttackTimer >= zayaAttackCooldown)
            {
                ZayaAttack();
                zayaAttackTimer = 0f;
            }

            encounterTimer -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    /// <summary>
    /// Coroutine for the final part of the encounter, where Ingris and Zaya realize they are allies.
    /// </summary>
    private IEnumerator NoveminadDiscoverySequence()
    {
        Debug.Log("Ingris: (Panting) Enough! What is your purpose here?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: (Panting) The same as yours, I suspect.");
        yield return new WaitForSeconds(dialoguePause / 2);
        Debug.Log("Zaya: That symbol...");
        yield return new WaitForSeconds(dialoguePause);

        GameObject symbolInstance = Instantiate(symbolPrefab, symbolSpawnPoint.position, symbolSpawnPoint.rotation);
        yield return new WaitForSeconds(symbolDisplayTime);
        Destroy(symbolInstance);

        Debug.Log("Zaya: I've seen it before. In my visions.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Ingris: Visions? Of fire and rebirth?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: And of a great darkness that threatens to consume all.");
        yield return new WaitForSeconds(dialoguePause / 2);
        Debug.Log("Ingris: You're one of us.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: We fight for the same cause. Against the same enemy.");
        yield return new WaitForSeconds(dialoguePause / 2);
        Debug.Log("Ingris: Then perhaps, archer, we should fight together.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: I'd like that, Phoenix Warrior.");
    }

    /// <summary>
    /// Executes Ingris's attack, playing an animation and creating a fire trail effect.
    /// It also checks for a hit on Zaya.
    /// </summary>
    private void IngrisAttack()
    {
        Debug.Log("Ingris attacks with a fiery sword strike!");
        ingrisAnimator.SetTrigger("Attack");
        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);

        // Check for collision with Zaya
        Collider[] hitColliders = Physics.OverlapSphere(ingrisSwordTip.position, ingrisSwordAttackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == zaya)
            {
                Debug.Log("Ingris hit Zaya!");
                // In a real game, you would get a health component and call:
                // zaya.GetComponent<Health>().TakeDamage(ingrisDamage);
            }
        }
    }

    /// <summary>
    /// Executes Zaya's attack by playing an animation and firing an arrow.
    /// </summary>
    private void ZayaAttack()
    {
        Debug.Log("Zaya fires an arrow!");
        zayaAnimator.SetTrigger("Shoot");
        FireArrow();
    }

    /// <summary>
    /// Instantiates an arrow prefab and gives it velocity.
    /// </summary>
    private void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, zayaArrowSpawn.position, zayaArrowSpawn.rotation);
        activeArrows.Add(arrow);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        if (arrowRb != null)
        {
            arrowRb.velocity = zayaArrowSpawn.forward * arrowSpeed;
        }
        Destroy(arrow, 5f); // Failsafe destruction after 5 seconds
    }

    /// <summary>
    /// Updates the position of all active arrows and checks for collisions.
    /// </summary>
    private void UpdateArrows()
    {
        // Iterate backwards to safely remove items from the list while iterating
        for (int i = activeArrows.Count - 1; i >= 0; i--)
        {
            GameObject arrow = activeArrows[i];
            if (arrow == null)
            {
                activeArrows.RemoveAt(i);
                continue;
            }

            // Simple collision check with a raycast
            RaycastHit hit;
            float distanceThisFrame = Time.deltaTime * arrowSpeed;
            if (Physics.Raycast(arrow.transform.position, arrow.transform.forward, out hit, distanceThisFrame))
            {
                if (hit.collider.gameObject == ingris)
                {
                    Debug.Log("Zaya's arrow hit Ingris!");
                    // In a real game, you would get a health component and call:
                    // ingris.GetComponent<Health>().TakeDamage(zayaArrowDamage);
                }
                Destroy(arrow);
                activeArrows.RemoveAt(i);
            }
            else if (Vector3.Distance(arrow.transform.position, zaya.transform.position) > zayaMaxAttackDistance)
            {
                Destroy(arrow);
                activeArrows.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Destroys all active arrow GameObjects and clears the list.
    /// </summary>
    private void CleanUpArrows()
    {
        foreach (var arrow in activeArrows)
        {
            if (arrow != null)
            {
                Destroy(arrow);
            }
        }
        activeArrows.Clear();
    }
}