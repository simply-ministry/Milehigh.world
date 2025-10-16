using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OnalymNexusBattle : MonoBehaviour
{
    public GameObject aeron;
    public GameObject zaia;
    public GameObject warMachine;
    public GameObject nexusArea;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public float windGustForce = 50f;
    public float electroBlastDamage = 40f;
    public float rockEruptionDamage = 60f;
    public float rockEruptionRadius = 5f;
    public GameObject moltenRockPrefab;
    public Transform rockEruptionSpawnPoint;
    public float rockEruptionDuration = 5f;
    public float timeBetweenMachineAttacks = 3f;
    private float nextMachineAttackTime;
    public float machineRotationSpeed = 2f;
    public float machineBeamDamage = 10f;
    public Transform machineCannon;
    public GameObject machineBeamPrefab;
    public float machineBeamDuration = 2f;
    public float machineBeamSpeed = 20f;
    public float aeronFlySpeed = 20f;
    public float aeronDamageReduction = 0.8f;
    public float impactForce = 10f; // Add this line
    public float machineMoltenSlowdown = 0.5f; // Add this line
    private Rigidbody warMachineRb;
    private Health warMachineHealth;
    private bool machineIsAttacking = false;
    private GameObject currentMachineBeam;
    private Transform currentTarget;

    void Start()
    {
        warMachineRb = warMachine.GetComponent<Rigidbody>();
        warMachineHealth = warMachine.GetComponent<Health>();
        if (!warMachineRb)
            Debug.LogError("War Machine needs a Rigidbody!");
        if (!warMachineHealth)
            Debug.LogError("War Machine needs a Health component!");

        nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;

        // Populate initial enemies (example)
        activeEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    void Update()
    {
        // Basic AI for the War Machine
        if (Time.time >= nextMachineAttackTime && !machineIsAttacking && activeEnemies.Count > 0)
        {
            StartMachineAttack();
            nextMachineAttackTime = Time.time + timeBetweenMachineAttacks;
        }

        if (machineIsAttacking)
        {
            UpdateMachineAttack();
        }

        // Win condition (example: all enemies defeated)
        if (activeEnemies.Count == 0)
        {
            Debug.Log("Cyrus's forces are defeated! Nexus secured!");
            // Trigger end of battle event
        }
    }

    void StartMachineAttack()
    {
        machineIsAttacking = true;
        // Select a target (prioritize Aeron?)
        if (aeron != null && zaia != null)
        {
            if (Vector3.Distance(warMachine.transform.position, aeron.transform.position) <
                Vector3.Distance(warMachine.transform.position, zaia.transform.position))
            {
                currentTarget = aeron.transform;
            }
            else
            {
                currentTarget = zaia.transform;
            }
        }
        else if (aeron != null)
        {
            currentTarget = aeron.transform;
        }
        else if (zaia != null)
        {
            currentTarget = zaia.transform;
        }
        else
        {
            machineIsAttacking = false;
            return; // No target
        }
        // Trigger attack animation/effects
        Debug.Log("War Machine starts attack!");
    }

    void UpdateMachineAttack()
    {
        if (currentTarget == null)
        {
            machineIsAttacking = false;
            return;
        }

        // Aim the cannon
        Vector3 targetDirection = (currentTarget.position - machineCannon.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        warMachine.transform.rotation = Quaternion.RotateTowards(warMachine.transform.rotation, targetRotation, machineRotationSpeed * Time.deltaTime);

        // Fire the beam (if it's ready)
        if (currentMachineBeam == null)
        {
            currentMachineBeam = Instantiate(machineBeamPrefab, machineCannon.position, machineCannon.rotation);
            Destroy(currentMachineBeam, machineBeamDuration); // Destroy the beam after its duration
            // Apply damage over time to the target
            DealBeamDamageOverTime(currentTarget.gameObject);
        }
    }

    void DealBeamDamageOverTime(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(machineBeamDamage * Time.deltaTime);
            Debug.Log($"{target.name} takes beam damage!");
        }
    }

    void EndMachineAttack()
    {
        machineIsAttacking = false;
        currentTarget = null;
        currentMachineBeam = null;
        // Reset animation/effects
        Debug.Log("War Machine ends attack.");
    }

    void AeronWindGustAttack(GameObject[] targets)
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
        // Play VFX and SFX for wind gust
        Debug.Log("Aeron unleashes a wind gust!");
    }

    void AeronTSIDKENUAttack(GameObject target)
    {
        // Apply damage
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(electroBlastDamage);
        }

        // Apply force to the target (if it has a Rigidbody)
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            Vector3 awayDirection = (target.transform.position - aeron.transform.position).normalized;
            targetRb.AddForce(awayDirection * impactForce, ForceMode.Impulse);
        }

        // Play VFX and SFX for the electrical blast
        Debug.Log("Aeron uses TSIDKENU!");
    }

    void ZaiaBarrier(bool active)
    {
        // Activate/deactivate a visual barrier effect
        // (e.g., enable/disable a GameObject, change material)
        // This is a placeholder; implement the actual visual effect
        if (active)
        {
            Debug.Log("Zaia activates barrier!");
        }
        else
        {
            Debug.Log("Zaia deactivates barrier!");
        }
    }

    void ZaiaRockEruptionAttack()
    {
        // Damage enemies in an area
        Collider[] hitColliders = Physics.OverlapSphere(rockEruptionSpawnPoint.position, rockEruptionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            Health targetHealth = hitCollider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(rockEruptionDamage);
            }
        }

        // Instantiate a molten rock prefab
        if (moltenRockPrefab != null)
        {
            GameObject rockInstance = Instantiate(moltenRockPrefab, rockEruptionSpawnPoint.position, Quaternion.identity);
            Destroy(rockInstance, rockEruptionDuration); // Destroy after a duration
        }

        // Slow down the war machine (if within range)
        float distanceToMachine = Vector3.Distance(rockEruptionSpawnPoint.position, warMachine.transform.position);
        if (distanceToMachine <= rockEruptionRadius)
        {
            warMachineRb.velocity *= machineMoltenSlowdown;
        }

        // Play VFX and SFX for the rock eruption
        Debug.Log("Zaia triggers a rock eruption!");
    }

    public void EnemyDefeated(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }
}