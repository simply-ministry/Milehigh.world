using UnityEngine;
using System.Collections;

/// <summary>
/// Simulates a server-authoritative combat manager.
/// This class is responsible for receiving attack requests, validating them based on server-side state (like cooldowns and range),
/// and applying damage if the actions are legitimate. This helps prevent cheating.
/// </summary>
public class ServerCombatManager : MonoBehaviour
{
    [Header("Character References")]
    /// <summary>
    /// The GameObject for the Ingris character.
    /// </summary>
    public GameObject ingris;
    /// <summary>
    /// The GameObject for the Zaya character.
    /// </summary>
    public GameObject zaya;

    [Header("Combat Parameters")]
    /// <summary>
    /// The valid attack range for Ingris's sword attack.
    /// </summary>
    public float ingrisSwordAttackRange = 3f;
    /// <summary>
    /// The maximum travel distance for Zaya's arrows.
    /// </summary>
    public float zayaMaxAttackDistance = 15f;
    /// <summary>
    /// The amount of damage Ingris deals per attack.
    /// </summary>
    public int ingrisDamage = 20;
    /// <summary>
    /// The amount of damage Zaya deals per arrow hit.
    /// </summary>
    public int zayaArrowDamage = 15;

    // --- Server-side state ---
    /// <summary>
    /// The cooldown duration for Ingris's attack.
    /// </summary>
    private float ingrisAttackCooldown = 1f;
    /// <summary>
    /// The cooldown duration for Zaya's attack.
    /// </summary>
    private float zayaAttackCooldown = 0.5f;
    /// <summary>
    /// The server's timestamp for when Ingris can attack next.
    /// </summary>
    private float ingrisNextAvailableAttackTime = 0f;
    /// <summary>
    /// The server's timestamp for when Zaya can attack next.
    /// </summary>
    private float zayaNextAvailableAttackTime = 0f;

    // In a real implementation, these would be proper player health components synced over the network.
    private int ingrisHealth = 100;
    private int zayaHealth = 100;

    /// <summary>
    /// Handles an attack request from a client for Ingris.
    /// It validates the request against cooldowns, attacker identity, and range before applying damage.
    /// </summary>
    /// <param name="attacker">The GameObject that initiated the attack request.</param>
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

        // Server-side validation of range
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

    /// <summary>
    /// Handles an attack request from a client for Zaya.
    /// It validates the request against cooldowns and attacker identity, then performs a server-side raycast to detect hits.
    /// </summary>
    /// <param name="attacker">The GameObject that initiated the attack request.</param>
    /// <param name="direction">The direction in which the arrow was fired.</param>
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