using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton that manages all UI elements for the combat scene.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject characterUIPanelPrefab; // A prefab for displaying a character's stats
    public Transform playerPartyPanel;      // The layout group for player UI
    public Transform enemyPartyPanel;       // The layout group for enemy UI

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Creates the UI panels for all characters at the start of combat.
    /// </summary>
    public void InitializeCombatUI(List<Character> playerParty, List<Character> enemyParty)
    {
        foreach (var character in playerParty)
        {
            GameObject panel = Instantiate(characterUIPanelPrefab, playerPartyPanel);
            panel.GetComponent<CharacterUI>().Initialize(character);
        }

        foreach (var character in enemyParty)
        {
            GameObject panel = Instantiate(characterUIPanelPrefab, enemyPartyPanel);
            panel.GetComponent<CharacterUI>().Initialize(character);
        }
    }
}