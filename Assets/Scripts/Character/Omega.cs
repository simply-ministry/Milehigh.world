using UnityEngine;

/// <summary>
/// Represents the character Omega.one, an ancient and powerful being.
/// </summary>
public class Omega : Character
{
    private float lastScanTime;
    private float scanCooldown = 5f; // Scan every 5 seconds

    protected override void Awake()
    {
        base.Awake();
        characterName = "Omega.one";
        // Initialize Omega.one's specific stats here
        attack = 70;
        defense = 60;
    }

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
    /// </summary>
    private void ScanEnvironment()
    {
        // Simple functional implementation: log the scan action.
        // In a real game, this could detect nearby enemies or objects.
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
    /// Temporarily boosts attack power.
    /// </summary>
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