using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages a story encounter between Ingris and Zaya using a single monolithic coroutine.
/// NOTE: A more robust, event-driven version of this encounter exists in 'Assets/Scripts/Core/IngrisZayaEncounter.cs'.
/// This version may be deprecated.
/// </summary>
public class IngrisZayaEncounter : MonoBehaviour
{
    [Header("Character References")]
    /// <summary>
    /// The GameObject representing the Ingris character.
    /// </summary>
    public GameObject ingris;
    /// <summary>
    /// The GameObject representing the Zaya character.
    /// </summary>
    public GameObject zaya;

    [Header("Attack Origins")]
    /// <summary>
    /// The transform representing the tip of Ingris's sword.
    /// </summary>
    public Transform ingrisSwordTip;
    /// <summary>
    /// The transform for Zaya's bow.
    /// </summary>
    public Transform zayaBow;
    /// <summary>
    /// The point from which Zaya's arrows are spawned.
    /// </summary>
    public Transform zayaArrowSpawn;

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
    /// The prefab for the Noveminaad symbol effect.
    /// </summary>
    public GameObject symbolPrefab;
    /// <summary>
    /// The point where the Noveminaad symbol appears.
    /// </summary>
    public Transform symbolSpawnPoint;

    [Header("Encounter Timing")]
    /// <summary>
    /// The total duration of the scripted fight in seconds.
    /// </summary>
    public float fightDuration = 20f;
    /// <summary>
    /// The duration to pause between lines of dialogue.
    /// </summary>
    public float dialoguePause = 2f;
    /// <summary>
    /// The interval between attack checks during the fight sequence.
    /// </summary>
    public float attackInterval = 2f;
    /// <summary>
    /// How long the Noveminaad symbol is displayed.
    /// </summary>
    public float symbolDisplayTime = 5f;

    [Header("Combat Parameters")]
    /// <summary>
    /// The speed of Zaya's arrows.
    /// </summary>
    public float arrowSpeed = 20f;
    /// <summary>
    /// The range of Ingris's melee sword attack.
    /// </summary>
    public float ingrisSwordAttackRange = 3f;
    /// <summary>
    /// How long the fire trail effect lasts in seconds.
    /// </summary>
    public float ingrisFireTrailDuration = 1f;
    /// <summary>
    /// The maximum distance an arrow can travel before being destroyed.
    /// </summary>
    public float zayaMaxAttackDistance = 15f;
    /// <summary>
    /// The damage dealt by Ingris's attack.
    /// </summary>
    public int ingrisDamage = 20;
    /// <summary>
    /// The damage dealt by Zaya's arrow.
    /// </summary>
    public int zayaArrowDamage = 15;

    // --- Private State ---
    private float timer;
    private bool fightActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();
    private bool ingrisCanAttack = true;
    private bool zayaCanAttack = true;
    private float ingrisAttackCooldown = 1f;
    private float zayaAttackCooldown = 0.5f;
    private float timeSinceIngrisAttack = 0f;
    private float timeSinceZayaAttack = 0f;

    // --- Component References ---
    private Animator ingrisAnimator;
    private Animator zayaAnimator;

    /// <summary>
    /// Initializes components and starts the encounter sequence.
    /// </summary>
    void Start()
    {
        ingrisAnimator = ingris.GetComponent<Animator>();
        zayaAnimator = zaya.GetComponent<Animator>();
        StartCoroutine(FullEncounter());
    }

    /// <summary>
    /// Called every frame. Updates arrow positions and attack cooldowns during the fight.
    /// </summary>
    void Update()
    {
        if (fightActive)
        {
            UpdateArrows();
            if (timeSinceIngrisAttack >= ingrisAttackCooldown) ingrisCanAttack = true;
            if (timeSinceZayaAttack >= zayaAttackCooldown) zayaCanAttack = true;
        }
    }

    /// <summary>
    /// The main coroutine that controls the entire encounter from start to finish.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator FullEncounter()
    {
        // Scene 1: Initial Encounter Dialogue
        Debug.Log("Zaya: I've heard tales of your fiery wrath, Phoenix Warrior. They say you leave nothing but ash in your wake.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Ingris: And what of it, archer? Are you here to test those tales?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: I'm here to ensure this land doesn't become another of your conquests.");
        yield return new WaitForSeconds(dialoguePause / 2);

        // Scene 2: The Fight
        fightActive = true;
        timer = fightDuration;
        StartCoroutine(FightSequence());
        yield return new WaitForSeconds(fightDuration);
        fightActive = false;

        // Scene 3: Noveminad Discovery
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

        // Clean up any remaining arrows
        foreach (var arrow in activeArrows)
        {
            Destroy(arrow);
        }
        activeArrows.Clear();
    }

    /// <summary>
    /// Coroutine that manages the combat loop, triggering attacks based on cooldowns.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator FightSequence()
    {
        while (fightActive && timer > 0)
        {
            timeSinceIngrisAttack += Time.deltaTime;
            timeSinceZayaAttack += Time.deltaTime;

            float rand = Random.value;
            if (rand < 0.5f && ingrisCanAttack && timeSinceIngrisAttack >= ingrisAttackCooldown)
            {
                IngrisAttack();
                ingrisCanAttack = false;
                timeSinceIngrisAttack = 0f;
            }
            else if (zayaCanAttack && timeSinceZayaAttack >= zayaAttackCooldown)
            {
                ZayaAttack();
                zayaCanAttack = false;
                timeSinceZayaAttack = 0f;
            }

            yield return new WaitForSeconds(attackInterval);
            timer -= attackInterval;
        }
    }

    /// <summary>
    /// Executes Ingris's attack, playing an animation and creating a fire trail effect.
    /// </summary>
    void IngrisAttack()
    {
        Debug.Log("Ingris attacks with a fiery sword strike!");
        ingrisAnimator.SetTrigger("Attack");

        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);

        Collider[] hitColliders = Physics.OverlapSphere(ingrisSwordTip.position, ingrisSwordAttackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == zaya)
            {
                Debug.Log("Ingris hit Zaya!");
                // In a real game, would call: zaya.GetComponent<Health>().TakeDamage(ingrisDamage);
            }
        }
    }

    /// <summary>
    /// Executes Zaya's attack by playing an animation and firing an arrow.
    /// </summary>
    void ZayaAttack()
    {
        Debug.Log("Zaya fires an arrow!");
        zayaAnimator.SetTrigger("Shoot");
        FireArrow();
    }

    /// <summary>
    /// Instantiates an arrow prefab and gives it velocity.
    /// </summary>
    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, zayaArrowSpawn.position, zayaArrowSpawn.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        if (arrowRb != null)
        {
            arrowRb.velocity = zayaArrowSpawn.forward * arrowSpeed;
        }
        activeArrows.Add(arrow);
        Destroy(arrow, 5f);
    }

    /// <summary>
    /// Updates the position of all active arrows and checks for collisions.
    /// </summary>
    void UpdateArrows()
    {
        for (int i = activeArrows.Count - 1; i >= 0; i--)
        {
            if (activeArrows[i] == null)
            {
                activeArrows.RemoveAt(i);
                continue;
            }

            RaycastHit hit;
            if (Physics.Raycast(activeArrows[i].transform.position, activeArrows[i].transform.forward, out hit, Time.deltaTime * arrowSpeed))
            {
                if (hit.collider.gameObject == ingris)
                {
                    Debug.Log("Zaya's arrow hit Ingris!");
                    // In a real game, would call: ingris.GetComponent<Health>().TakeDamage(zayaArrowDamage);
                }
                Destroy(activeArrows[i]);
                activeArrows.RemoveAt(i);
            }
            else if (Vector3.Distance(activeArrows[i].transform.position, zaya.transform.position) > zayaMaxAttackDistance)
            {
                Destroy(activeArrows[i]);
                activeArrows.RemoveAt(i);
            }
        }
    }
}