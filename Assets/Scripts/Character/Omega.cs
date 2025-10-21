// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the character Omega.one, an ancient and powerful being that balances celestial and void energies.
/// This class implements its unique abilities, such as environmental scanning and power channeling.
/// </summary>
public class Omega : Character
{
    /// <summary>
    /// The time when the last environment scan was performed.
    /// </summary>
    private float lastScanTime;
    /// <summary>
    /// The cooldown period in seconds between environment scans.
    /// </summary>
    private float scanCooldown = 5f; // Scan every 5 seconds

    /// <summary>
    /// Initializes Omega.one's specific attributes, setting its name and base stats.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        characterName = "Omega.one";
        // Initialize Omega.one's specific stats here
        attack = 70;
        defense = 60;
    }

    /// <summary>
    /// Called every frame. Manages the cooldown for the environment scan.
    /// </summary>
    void Update()
    {
        if (Time.time - lastScanTime > scanCooldown)
        {
            ScanEnvironment();
            lastScanTime = Time.time;
        }
    }

    /// <summary>
    /// Scans the environment for energy signatures and other anomalies.
    /// In a real game, this could detect nearby enemies or interactive objects.
    /// </summary>
    private void ScanEnvironment()
    {
        // Simple functional implementation: log the scan action.
        Debug.Log($"{characterName} performs a wide-area energy scan. No immediate threats detected.");
    }

    /// <summary>
    /// Taps into celestial power to perform a special ability, like healing.
    /// </summary>
    public void UseCelestialPower()
    {
        Say("Harnessing celestial energy for restoration.");
        Heal(25); // Use the Heal method from the base Character class
    }

    /// <summary>
    /// Taps into Void power to perform a special ability, like a temporary damage boost.
    /// </summary>
    public void UseVoidPower()
    {
        Say("Channeling Void energy for a powerful strike.");
        StartCoroutine(VoidPowerBoost());
    }

    /// <summary>
    /// A coroutine that temporarily boosts attack power for a set duration.
    /// </summary>
    /// <returns>An IEnumerator to be used by StartCoroutine.</returns>
    private IEnumerator VoidPowerBoost()
    {
        int originalAttack = attack;
        attack += 30; // Boost attack
        Debug.Log($"{characterName}'s attack is temporarily boosted to {attack}!");
        yield return new WaitForSeconds(10f); // Boost lasts for 10 seconds
        attack = originalAttack;
        Debug.Log($"{characterName}'s attack power returns to normal.");
    }
}