using UnityEngine;
using System.Collections;

public class ServerCombatManager : MonoBehaviour
{
    public GameObject ingris;
    public GameObject zaya;
    public float ingrisSwordAttackRange = 3f;
    public float zayaMaxAttackDistance = 15f;
    public int ingrisDamage = 20;
    public int zayaArrowDamage = 15;

    // Server-side state
    private float ingrisAttackCooldown = 1f;
    private float zayaAttackCooldown = 0.5f;
    private float ingrisNextAvailableAttackTime = 0f;
    private float zayaNextAvailableAttackTime = 0f;

    // In a real implementation, these would be proper player health components
    private int ingrisHealth = 100;
    private int zayaHealth = 100;

    // Public methods to be called by client requests
    public void HandleIngrisAttack(GameObject attacker)
    {
        if (Time.time < ingrisNextAvailableAttackTime)
        {
            Debug.Log("[SERVER] Ingris attack is on cooldown.");
            return;
        }

        if (attacker != ingris)
        {
            Debug.LogError("[SERVER] Invalid attacker for Ingris's attack!");
            return;
        }

        Debug.Log("[SERVER] Processing Ingris's attack.");
        ingrisNextAvailableAttackTime = Time.time + ingrisAttackCooldown;

        // Server-side validation
        float distance = Vector3.Distance(ingris.transform.position, zaya.transform.position);
        if (distance <= ingrisSwordAttackRange)
        {
            Debug.Log("[SERVER] Ingris's attack is in range. Zaya takes damage.");
            zayaHealth -= ingrisDamage;
            Debug.Log($"[SERVER] Zaya health: {zayaHealth}");
            // In a real scenario, you would send this health update to all clients.
        }
        else
        {
            Debug.Log("[SERVER] Ingris's attack is out of range.");
        }
    }

    public void HandleZayaAttack(GameObject attacker, Vector3 direction)
    {
        if (Time.time < zayaNextAvailableAttackTime)
        {
            Debug.Log("[SERVER] Zaya attack is on cooldown.");
            return;
        }

        if (attacker != zaya)
        {
            Debug.LogError("[SERVER] Invalid attacker for Zaya's attack!");
            return;
        }

        Debug.Log("[SERVER] Processing Zaya's attack.");
        zayaNextAvailableAttackTime = Time.time + zayaAttackCooldown;

        // Server-side raycast for hit detection
        RaycastHit hit;
        if (Physics.Raycast(zaya.transform.position, direction, out hit, zayaMaxAttackDistance))
        {
            if (hit.collider.gameObject == ingris)
            {
                Debug.Log("[SERVER] Zaya's arrow hit Ingris. Ingris takes damage.");
                ingrisHealth -= zayaArrowDamage;
                Debug.Log($"[SERVER] Ingris health: {ingrisHealth}");
                // In a real scenario, you would send this health update to all clients.
            }
            else
            {
                Debug.Log($"[SERVER] Zaya's arrow hit {hit.collider.name}, but not Ingris.");
            }
        }
        else
        {
            Debug.Log("[SERVER] Zaya's arrow missed.");
        }
    }
}