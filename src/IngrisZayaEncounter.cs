using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngrisZayaEncounter : MonoBehaviour
{
    [Header("Object References")]
    public GameObject ingris;
    public GameObject zaya;
    public Transform ingrisSwordTip;
    public Transform zayaBow;
    public Transform zayaArrowSpawn;
    public GameObject arrowPrefab;
    public GameObject fireTrailPrefab;
    public float fightDuration = 20f;
    public float dialoguePause = 2f;
    public float attackInterval = 2f;
    public float arrowSpeed = 20f;
    public float ingrisSwordAttackRange = 3f;
    public float ingrisFireTrailDuration = 1f;
    public float zayaMaxAttackDistance = 15f;
    public int ingrisDamage = 20;
    public int zayaArrowDamage = 15;

    private float timer;
    private bool fightActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();
    private bool ingrisCanAttack = true;
    private bool zayaCanAttack = true;
    private float ingrisAttackCooldown = 1f;
    private float zayaAttackCooldown = 0.5f;
    private float timeSinceIngrisAttack = 0f;
    private float timeSinceZayaAttack = 0f;


    [Header("Prefabs")]
    public GameObject arrowPrefab;
    public GameObject fireTrailPrefab;
    public GameObject symbolPrefab;
    public Transform symbolSpawnPoint;

    [Header("Encounter Settings")]
    public float fightDuration = 20f;
    public float dialoguePause = 2f;
    public float attackInterval = 2f;
    public float symbolDisplayTime = 5f;

    [Header("Ingris Settings")]
    public float ingrisSwordAttackRange = 3f;
    public float ingrisFireTrailDuration = 1f;
    public int ingrisDamage = 20;
    public float ingrisAttackCooldown = 1f;

    [Header("Zaya Settings")]
    public float arrowSpeed = 20f;
    public float zayaMaxAttackDistance = 15f;
    public int zayaArrowDamage = 15;
    public float zayaAttackCooldown = 0.5f;

    // --- Private State ---
    private float timer;
    private bool fightActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();
    private float timeSinceIngrisAttack;
    private float timeSinceZayaAttack;

    // --- Component References ---
    public IngrisZayaDialogue dialogueManager;
    private Animator ingrisAnimator;
    private Animator zayaAnimator;

    void Start()
    {
        ingrisAnimator = ingris.GetComponent<Animator>();
        zayaAnimator = zaya.GetComponent<Animator>();
        StartCoroutine(Encounter());
    }

    IEnumerator Encounter()
    {
        // Dialogue
        Debug.Log("Zaya: That symbol... I've seen it before. In my visions.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Ingris: Visions? Of fire and rebirth?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: And of a great darkness that threatens to consume all.");
        yield return new WaitForSeconds(dialoguePause);

        // Initiate Fight
        fightActive = true;
        timer = fightDuration;
        StartCoroutine(FightSequence());
        yield return new WaitForSeconds(fightDuration);
        fightActive = false;

        // Realization
        Debug.Log("Ingris: You're one of us.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: We fight for the same cause. Against the same enemy.");
        yield return new WaitForSeconds(dialoguePause);

        // Newfound understanding
        Debug.Log("Ingris: Then perhaps, archer, we should fight together.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: I'd like that, Phoenix Warrior.");

        // Clean up arrows
        foreach (var arrow in activeArrows)
        {
            Destroy(arrow);
        }
        activeArrows.Clear();

        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue Manager has not been assigned in the Inspector!");
            return;
        }

        // Subscribe to the dialogue completion event
        IngrisZayaDialogue.OnDialogueEnd += HandleDialogueEnd;

        // Start the initial dialogue
        StartCoroutine(dialogueManager.StartDialogue());
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        IngrisZayaDialogue.OnDialogueEnd -= HandleDialogueEnd;
    }

    private void HandleDialogueEnd()
    {
        // This method is called by the event from IngrisZayaDialogue
        StartCoroutine(FightAndConclusion());
    }

    private IEnumerator FightAndConclusion()
    {
        // 1. Initiate Fight
        Debug.Log("Dialogue over. The fight begins!");
        fightActive = true;
        timer = fightDuration;
        timeSinceIngrisAttack = ingrisAttackCooldown; // Allow immediate attack
        timeSinceZayaAttack = zayaAttackCooldown;   // Allow immediate attack
        StartCoroutine(FightSequence());

        yield return new WaitForSeconds(fightDuration);

        fightActive = false;
        Debug.Log("The fight has ended.");

        // 2. Post-Fight Scene
        yield return StartCoroutine(NoveminadDiscoverySequence());
    }

    IEnumerator FightSequence()
    {
        while (fightActive && timer > 0)
        {
            // Check which characters are ready to attack
            bool ingrisReady = timeSinceIngrisAttack >= ingrisAttackCooldown;
            bool zayaReady = timeSinceZayaAttack >= zayaAttackCooldown;

            // Attack logic: If both are ready, one is chosen at random.
            // If only one is ready, they will attack.
            if (ingrisReady && zayaReady)
            float rand = Random.value;

            if (rand < 0.5f && ingrisCanAttack)
            {
                IngrisAttack();
            }
            else if (zayaCanAttack)
            {
                ZayaAttack();
            // Randomly decide who attacks
            if (Random.value < 0.5f)
            {
                if (Random.value < 0.5f)
                {
                    IngrisAttack();
                    timeSinceIngrisAttack = 0f;
                }
                else
                {
                    ZayaAttack();
                    timeSinceZayaAttack = 0f;
                }
            }
            else if (ingrisReady)
            {
                IngrisAttack();
                timeSinceIngrisAttack = 0f;
            }
            else if (zayaReady)
            {
                ZayaAttack();
                timeSinceZayaAttack = 0f;
            }
            // If neither is ready, the system waits for the next attack interval.

            yield return new WaitForSeconds(attackInterval);
            timer -= attackInterval;
        }
    }

    void Update()
    {
        if (fightActive)
        {
            if (!ingrisCanAttack)
            {
                timeSinceIngrisAttack += Time.deltaTime;
                if (timeSinceIngrisAttack >= ingrisAttackCooldown)
                {
                    ingrisCanAttack = true;
                    timeSinceIngrisAttack = 0f;
                }
            }

            if (!zayaCanAttack)
            {
                timeSinceZayaAttack += Time.deltaTime;
                if (timeSinceZayaAttack >= zayaAttackCooldown)
                {
                    zayaCanAttack = true;
                    timeSinceZayaAttack = 0f;
                }
            }
            timeSinceIngrisAttack += Time.deltaTime;
            timeSinceZayaAttack += Time.deltaTime;
            UpdateArrows();
        }
    }

    IEnumerator NoveminadDiscoverySequence()
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

        // Clean up any remaining arrows
        foreach (var arrow in activeArrows)
        {
            if (arrow != null) Destroy(arrow);
        }
        activeArrows.Clear();
    }

    void IngrisAttack()
    {
        Debug.Log("Ingris attacks with a fiery sword strike!");
        ingrisAnimator.SetTrigger("Attack");
        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);

        ingrisCanAttack = false;


        GameObject fireTrail = Instantiate(fireTrailPrefab, ingrisSwordTip.position, ingrisSwordTip.rotation);
        Destroy(fireTrail, ingrisFireTrailDuration);

        Collider[] hitColliders = Physics.OverlapSphere(ingrisSwordTip.position, ingrisSwordAttackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == zaya)
            {
                Debug.Log("Ingris hit Zaya!");
                // zaya.GetComponent<Health>().TakeDamage(ingrisDamage);
            }
        }
    }

    void ZayaAttack()
    {
        Debug.Log("Zaya fires an arrow!");
        zayaAnimator.SetTrigger("Shoot");
        zayaCanAttack = false;
        FireArrow();
    }

    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, zayaArrowSpawn.position, zayaArrowSpawn.rotation);
        activeArrows.Add(arrow);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        if (arrowRb != null)
        {
            arrowRb.velocity = zayaArrowSpawn.forward * arrowSpeed;
        }
        activeArrows.Add(arrow);
        Destroy(arrow, 5f);
        Destroy(arrow, 5f); // Failsafe destruction
    }

    void UpdateArrows()
    {
        for (int i = activeArrows.Count - 1; i >= 0; i--)
        {
            if (activeArrows[i] == null)
            {
                activeArrows.RemoveAt(i);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(activeArrows[i].transform.position, activeArrows[i].transform.forward, out hit, Time.deltaTime * arrowSpeed))
                {
                    if (hit.collider.gameObject == ingris)
                    {
                        Debug.Log("Zaya's arrow hit Ingris!");
                        // ingris.GetComponent<Health>().TakeDamage(zayaArrowDamage);
                        Destroy(activeArrows[i]);
                        activeArrows.RemoveAt(i);
                    }
                    else
                    {
                        Destroy(activeArrows[i]);
                        activeArrows.RemoveAt(i);
                    }
                }
                else if (Vector3.Distance(activeArrows[i].transform.position, zaya.transform.position) > zayaMaxAttackDistance)
                {
                    Destroy(activeArrows[i]);
                    activeArrows.RemoveAt(i);
                }
            GameObject arrow = activeArrows[i];
            if (arrow == null)
            {
                activeArrows.RemoveAt(i);
                continue;
            }

            // Simple collision check with a raycast
            RaycastHit hit;
            if (Physics.Raycast(arrow.transform.position, arrow.transform.forward, out hit, Time.deltaTime * arrowSpeed))
            {
                if (hit.collider.gameObject == ingris)
                {
                    Debug.Log("Zaya's arrow hit Ingris!");
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
}