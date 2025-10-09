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
    public Transform actionButtonPanel;     // The layout group for the current player's abilities
    public GameObject actionButtonPrefab;   // A prefab for an ability button

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
        // Clear any old UI
        foreach (Transform child in playerPartyPanel) Destroy(child.gameObject);
        foreach (Transform child in enemyPartyPanel) Destroy(child.gameObject);

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

    /// <summary>
    /// Creates the action buttons for the current player's turn.
    /// </summary>
    public void DisplayPlayerActions(Character character)
    {
        // Clear old buttons
        foreach (Transform child in actionButtonPanel) Destroy(child.gameObject);

        // Assuming the character has an AbilitySystem component with a list of abilities
        AbilitySystem abilitySystem = character.GetComponent<AbilitySystem>();
        if (abilitySystem == null) return;

        foreach (var ability in abilitySystem.abilities)
        {
            GameObject buttonGO = Instantiate(actionButtonPrefab, actionButtonPanel);
            buttonGO.GetComponent<ActionButton>().Initialize(ability, character);
        }
    }
}