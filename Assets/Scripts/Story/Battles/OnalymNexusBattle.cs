using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the Onalym Nexus battle, a complex encounter involving heroes, a war machine, and environmental mechanics.
/// This script coordinates the actions of Aeron, Zaia, and the enemy War Machine.
/// </summary>
public class OnalymNexusBattle : MonoBehaviour
{
    [Header("Hero References")]
    [Tooltip("The GameObject for the Aeron character.")]
    public GameObject aeron;
    [Tooltip("The GameObject for the Zaia character.")]
    public GameObject zaia;

    [Header("Enemy References")]
    [Tooltip("The GameObject for the War Machine enemy.")]
    public GameObject warMachine;
    [Tooltip("A list of all active enemies in the battle.")]
    public List<GameObject> activeEnemies = new List<GameObject>();

    [Header("Aeron Abilities")]
    [Tooltip("The force of Aeron's wind gust attack.")]
    public float windGustForce = 50f;
    [Tooltip("The damage of Aeron's electro blast attack.")]
    public float electroBlastDamage = 40f;
    [Tooltip("The impact force of Aeron's TSIDKENU attack.")]
    public float impactForce = 10f;

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
    [Tooltip("The slowdown multiplier applied to the war machine when hit by molten rock.")]
    public float machineMoltenSlowdown = 0.5f;

    // --- Private State ---
    private Rigidbody warMachineRb;
    private Health warMachineHealth;
    private bool machineIsAttacking = false;
    private float nextMachineAttackTime;
    private GameObject currentMachineBeam;
    private Transform currentTarget;

    /// <summary>
    /// Initializes the battle, getting references to components and setting up the initial state.
    /// </summary>
    void Start()
    {
        warMachineRb = warMachine.GetComponent<Rigidbody>();
        // NOTE: This assumes a 'Health' component exists.
        warMachineHealth = warMachine.GetComponent<Health>();
        if (!warMachineRb) Debug.LogError("War Machine needs a Rigidbody!");
        if (!warMachineHealth) Debug.LogError("War Machine needs a Health component!");

        nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;
        activeEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    /// <summary>
    /// Called every frame. Manages the war machine's AI and checks for win conditions.
    /// </summary>
    void Update()
    {
        if (Time.time >= nextMachineAttackTime && !machineIsAttacking && activeEnemies.Any())
        {
            StartMachineAttack();
            nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;
        }

        if (machineIsAttacking)
        {
            UpdateMachineAttack();
        }

        if (!activeEnemies.Any())
        {
            Debug.Log("Cyrus's forces are defeated! Nexus secured!");
            // Trigger end of battle event
        }
    }

    /// <summary>
    /// Starts the war machine's attack sequence, selecting a target.
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
            machineIsAttacking = false;
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
        // NOTE: This assumes a 'Health' component exists.
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
    /// Executes Aeron's TSIDKENU attack, dealing damage and applying force.
    /// </summary>
    /// <param name="target">The target GameObject.</param>
    public void AeronTSIDKENUAttack(GameObject target)
    {
        // NOTE: This assumes a 'Health' component exists.
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(electroBlastDamage);
        }

        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            Vector3 awayDirection = (target.transform.position - aeron.transform.position).normalized;
            targetRb.AddForce(awayDirection * impactForce, ForceMode.Impulse);
        }
        Debug.Log("Aeron uses TSIDKENU!");
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
            // NOTE: This assumes a 'Health' component exists.
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
                closestTarget = zaia.transform;
            }
        }
        return closestTarget;
    }
}
