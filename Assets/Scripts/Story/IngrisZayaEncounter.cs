using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngrisZayaEncounter : MonoBehaviour
{
    public GameObject ingris;
    public GameObject zaya;
    public Transform ingrisSwordTip;
    public Transform zayaBow;
    public Transform zayaArrowSpawn;
    public GameObject arrowPrefab;
    public GameObject fireTrailPrefab;
    public GameObject symbolPrefab; // Prefab for the Noveminad symbol effect
    public Transform symbolSpawnPoint; // Where the symbol appears

    public float fightDuration = 20f;
    public float dialoguePause = 2f;
    public float attackInterval = 2f;
    public float arrowSpeed = 20f;
    public float ingrisSwordAttackRange = 3f;
    public float ingrisFireTrailDuration = 1f;
    public float zayaMaxAttackDistance = 15f;
    public int ingrisDamage = 20;
    public int zayaArrowDamage = 15;
    public float symbolDisplayTime = 5f; // How long the symbol stays

    private float timer;
    private bool fightActive = false;
    private bool symbolActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();
    private bool ingrisCanAttack = true;
    private bool zayaCanAttack = true;
    private float ingrisAttackCooldown = 1f;
    private float zayaAttackCooldown = 0.5f;
    private float timeSinceIngrisAttack = 0f;
    private float timeSinceZayaAttack = 0f;

    private Animator ingrisAnimator;
    private Animator zayaAnimator;

    void Start()
    {
        ingrisAnimator = ingris.GetComponent<Animator>();
        zayaAnimator = zaya.GetComponent<Animator>();
        StartCoroutine(FullEncounter());
    }

    IEnumerator FullEncounter()
    {
        // Scene 1: Initial Encounter

        // Dialogue
        Debug.Log("Zaya: I've heard tales of your fiery wrath, Phoenix Warrior. They say you leave nothing but ash in your wake.");
        yield return new WaitForSeconds(dialoguePause);

        Debug.Log("Ingris: And what of it, archer? Are you here to test those tales?");
        yield return new WaitForSeconds(dialoguePause);

        Debug.Log("Zaya: I'm here to ensure this land doesn't become another of your conquests.");
        yield return new WaitForSeconds(dialoguePause / 2);

        // Initiate Fight
        fightActive = true;
        timer = fightDuration;
        StartCoroutine(FightSequence());

        yield return new WaitForSeconds(fightDuration);

        fightActive = false;

        // Scene 2: Noveminad Discovery

        // Realization
        Debug.Log("Ingris: (Panting) Enough! What is your purpose here?");
        yield return new WaitForSeconds(dialoguePause);

        Debug.Log("Zaya: (Panting) The same as yours, I suspect.");
        yield return new WaitForSeconds(dialoguePause / 2);

        Debug.Log("Zaya: That symbol...");
        yield return new WaitForSeconds(dialoguePause);

        GameObject symbolInstance = Instantiate(symbolPrefab, symbolSpawnPoint.position, symbolSpawnPoint.rotation);
        symbolActive = true;
        yield return new WaitForSeconds(symbolDisplayTime);
        Destroy(symbolInstance);
        symbolActive = false;

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
    }

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

    void Update()
    {
        if (fightActive)
        {
            UpdateArrows();
            if (timeSinceIngrisAttack >= ingrisAttackCooldown) ingrisCanAttack = true;
            if (timeSinceZayaAttack >= zayaAttackCooldown) zayaCanAttack = true;
        }
    }

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
                // zaya.GetComponent<Health>().TakeDamage(ingrisDamage);
            }
        }
    }

    void ZayaAttack()
    {
        Debug.Log("Zaya fires an arrow!");
        zayaAnimator.SetTrigger("Shoot");
        FireArrow();
    }

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
            }
        }
    }
}