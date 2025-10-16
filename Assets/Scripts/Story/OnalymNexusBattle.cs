using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the gameplay mechanics for the battle that takes place at the Onalym Nexus.
/// This script controls enemy AI, player abilities, and win/loss conditions for this specific encounter.
/// </summary>
public class OnalymNexusBattle : MonoBehaviour
{
    // These sections link the script to the actual character GameObjects in the Unity scene.
    [Header("Hero References")]
    [Tooltip("The GameObject for the Aeron character.")]
    public GameObject aeron;
    [Tooltip("The GameObject for the Zaia character.")]
    public GameObject zaia;
    [Tooltip("The GameObject for the Micah character.")]
    public GameObject micah;

    // This section links to the enemy GameObjects involved in the battle.
    [Header("Enemy References")]
    [Tooltip("The GameObject for the War Machine enemy.")]
    public GameObject warMachine;
    [Tooltip("A list of all active enemies in the battle.")]
    public List<GameObject> activeEnemies = new List<GameObject>();

    // These are tunable parameters for Aeron's special attacks.
    [Header("Aeron Abilities")]
    [Tooltip("The force of Aeron's wind gust attack.")]
    public float windGustForce = 50f;

    [Header("Micah Abilities")]
    [Tooltip("The damage of Micah's TSIDKENU attack.")]
    public float electroBlastDamage = 40f;
    [Tooltip("The impact force of Micah's TSIDKENU attack.")]
    public float impactForce = 10f;

    // Tunable parameters for Zaia's special attacks.
    [Header("Zaia Abilities")]
    [Tooltip("The damage of Zaia's rock eruption attack.")]
    public float rockEruptionDamage = 60f;
    [Tooltip("The radius of the rock eruption attack.")]
    public float rockEruptionRadius = 5f;
    [Tooltip("The prefab for the molten rock created by the eruption.")]
    public GameObject moltenRockPrefab;
    [Tooltip("The spawn point for the rock eruption.")]
    public Transform rockEruptionSpawnPoint;
    [Tooltip("The duration of the molten rock effect.")]
    public float rockEruptionDuration = 5f;

    // This section defines the behavior and stats of the War Machine enemy.
    [Header("War Machine Settings")]
    [Tooltip("The time between the war machine's attacks.")]
    public float timeBetweenMachineAttacks = 3f;
    [Tooltip("The rotation speed of the war machine when targeting.")]
    public float machineRotationSpeed = 2f;
    [Tooltip("The damage per second of the war machine's beam.")]
    public float machineBeamDamage = 10f;
    [Tooltip("The transform of the war machine's cannon.")]
    public Transform machineCannon;
    [Tooltip("The prefab for the war machine's beam attack.")]
    public GameObject machineBeamPrefab;
    [Tooltip("The duration of the beam attack.")]
    public float machineBeamDuration = 2f;
    [Tooltip("The slowdown multiplier applied when the war machine is hit by molten rock.")]
    public float machineMoltenSlowdown = 0.5f;

    // --- Private variables to track the state of the battle ---
    private Rigidbody warMachineRb;
    private Health warMachineHealth;
    private bool machineIsAttacking = false;
    private float nextMachineAttackTime;
    private GameObject currentMachineBeam;
    private Transform currentTarget;

    /// <summary>
    /// This is a standard Unity function. It runs once when the battle starts.
    /// It's used here to get necessary components from the GameObjects for later use.
    /// </summary>
    void Start()
    {
        warMachineRb = warMachine.GetComponent<Rigidbody>();
        warMachineHealth = warMachine.GetComponent<Health>();
        if (!warMachineRb) Debug.LogError("War Machine needs a Rigidbody!");
        if (!warMachineHealth) Debug.LogError("War Machine needs a Health component!");

        nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;
        activeEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    /// <summary>
    /// This is a standard Unity function that runs on every single frame.
    /// It's the main "heartbeat" of the script, managing the enemy's decisions.
    /// </summary>
    void Update()
    {
        // If enough time has passed, and the machine isn't already attacking, start a new attack.
        if (Time.time >= nextMachineAttackTime && !machineIsAttacking && activeEnemies.Any())
        {
            StartMachineAttack();
            nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;
        }

        // If the machine is in the middle of an attack, keep updating its aim and beam.
        if (machineIsAttacking)
        {
            UpdateMachineAttack();
        }

        // Check for the win condition: if no enemies are left, the battle is won.
        if (!activeEnemies.Any())
        {
            Debug.Log("Cyrus's forces are defeated! Nexus secured!");
            // Here you would trigger a cutscene or end the level.
        }
    }

    /// <summary>
    /// Begins the enemy's attack sequence by finding the closest hero to target.
    /// </summary>
    void StartMachineAttack()
    {
        machineIsAttacking = true;
        currentTarget = FindClosestTarget();
        if (currentTarget != null)
        {
            Debug.Log("War Machine starts attack!");
        }
        else
        {
            machineIsAttacking = false; // No targets left.
        }
    }

    /// <summary>
    /// Updates the war machine's attack, aiming and firing the beam.
    /// </summary>
    void UpdateMachineAttack()
    {
        if (currentTarget == null)
        {
            EndMachineAttack();
            return;
        }

        Vector3 targetDirection = (currentTarget.position - machineCannon.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        warMachine.transform.rotation = Quaternion.RotateTowards(warMachine.transform.rotation, targetRotation, machineRotationSpeed * Time.deltaTime);

        if (currentMachineBeam == null)
        {
            currentMachineBeam = Instantiate(machineBeamPrefab, machineCannon.position, machineCannon.rotation);
            Destroy(currentMachineBeam, machineBeamDuration);
        }

        DealBeamDamageOverTime(currentTarget.gameObject);
    }

    /// <summary>
    /// Deals damage over time to a target from the war machine's beam.
    /// </summary>
    /// <param name="target">The target GameObject.</param>
    void DealBeamDamageOverTime(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(machineBeamDamage * Time.deltaTime);
            Debug.Log($"{target.name} takes beam damage!");
        }
    }

    /// <summary>
    /// Ends the war machine's attack sequence.
    /// </summary>
    void EndMachineAttack()
    {
        machineIsAttacking = false;
        currentTarget = null;
        if (currentMachineBeam != null)
        {
            Destroy(currentMachineBeam);
        }
        Debug.Log("War Machine ends attack.");
    }

    /// <summary>
    /// Executes Aeron's wind gust attack, pushing back targets.
    /// </summary>
    /// <param name="targets">An array of target GameObjects.</param>
    public void AeronWindGustAttack(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Vector3 awayDirection = (target.transform.position - aeron.transform.position).normalized;
                targetRb.AddForce(awayDirection * windGustForce, ForceMode.Impulse);
            }
        }
        Debug.Log("Aeron unleashes a wind gust!");
    }

    /// <summary>
    /// Executes Micah's TSIDKENU attack, dealing damage and applying force.
    /// </summary>
    /// <param name="target">The target GameObject.</param>
    public void MicahTSIDKENUAttack(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(electroBlastDamage);
        }

        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            Vector3 awayDirection = (target.transform.position - micah.transform.position).normalized;
            targetRb.AddForce(awayDirection * impactForce, ForceMode.Impulse);
        }
        Debug.Log("Micah uses TSIDKENU!");
    }

    /// <summary>
    /// Activates or deactivates Zaia's defensive barrier.
    /// </summary>
    /// <param name="active">Whether the barrier should be active.</param>
    public void ZaiaBarrier(bool active)
    {
        Debug.Log(active ? "Zaia activates barrier!" : "Zaia deactivates barrier!");
    }

    /// <summary>
    /// Executes Zaia's rock eruption attack, dealing area damage and slowing the war machine.
    /// </summary>
    public void ZaiaRockEruptionAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(rockEruptionSpawnPoint.position, rockEruptionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            Health targetHealth = hitCollider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(rockEruptionDamage);
            }
        }

        if (moltenRockPrefab != null)
        {
            GameObject rockInstance = Instantiate(moltenRockPrefab, rockEruptionSpawnPoint.position, Quaternion.identity);
            Destroy(rockInstance, rockEruptionDuration);
        }

        float distanceToMachine = Vector3.Distance(rockEruptionSpawnPoint.position, warMachine.transform.position);
        if (distanceToMachine <= rockEruptionRadius)
        {
            warMachineRb.velocity *= machineMoltenSlowdown;
        }
        Debug.Log("Zaia triggers a rock eruption!");
    }

    /// <summary>
    /// Removes a defeated enemy from the active list.
    /// </summary>
    /// <param name="enemy">The defeated enemy GameObject.</param>
    public void EnemyDefeated(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }

    /// <summary>
    /// Finds the closest valid target for the war machine.
    /// </summary>
    /// <returns>The transform of the closest target, or null if no targets are available.</returns>
    private Transform FindClosestTarget()
    {
        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        if (aeron != null && aeron.activeInHierarchy)
        {
            float dist = Vector3.Distance(warMachine.transform.position, aeron.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestTarget = aeron.transform;
            }
        }

        if (zaia != null && zaia.activeInHierarchy)
        {
            float dist = Vector3.Distance(warMachine.transform.position, zaia.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestTarget = zaia.transform;
            }
        }

        if (micah != null && micah.activeInHierarchy)
        {
            float dist = Vector3.Distance(warMachine.transform.position, micah.transform.position);
            if (dist < minDistance)
            {
                closestTarget = micah.transform;
            }
        }
        return closestTarget;
    }
}