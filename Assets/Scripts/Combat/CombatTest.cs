using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple script to trigger a test combat encounter.
/// </summary>
public class CombatTest : MonoBehaviour
{
    [Header("Combatants")]
    public List<Character> playerParty;
    public List<Character> enemyParty;

    void Start()
    {
        // A simple way to start combat for testing purposes.
        // Press the 'T' key to begin the fight.
        Debug.Log("Combat Test scene started. Press 'T' to begin combat.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (CombatManager.Instance != null)
            {
                Debug.Log("Starting combat via test script...");
                CombatManager.Instance.StartCombat(playerParty, enemyParty);
            }
            else
            {
                Debug.LogError("CombatManager instance not found in the scene!");
            }
        }
    }
}