using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script for a UI button that represents a character's ability.
/// </summary>
public class ActionButton : MonoBehaviour
{
    public Text abilityNameText;

    private Ability assignedAbility;
    private Character caster;

    public void Initialize(Ability ability, Character caster)
    {
        assignedAbility = ability;
        this.caster = caster;
        abilityNameText.text = ability.abilityName;

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Find the PlayerController in the scene to get the player's selected target.
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("ActionButton: Could not find PlayerController in the scene!");
            return;
        }

        Character currentTarget = playerController.CurrentTarget;

        if (currentTarget == null)
        {
            Debug.LogWarning("No target selected. Right-click on an enemy to select a target before using an ability.");
            return;
        }

        if (caster != null && assignedAbility != null)
        {
            Debug.Log($"UI: Player chose to use '{assignedAbility.abilityName}' on '{currentTarget.characterName}'.");
            CombatManager.Instance.PlayerAction(caster, currentTarget, assignedAbility);
        }
        else
        {
            // This case should be less likely now, but good to keep for debugging.
            Debug.LogWarning("UI: Action could not be performed. Caster or ability is missing.");
        }
    }
}