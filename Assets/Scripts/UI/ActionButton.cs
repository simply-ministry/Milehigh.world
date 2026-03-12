using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script for a UI button that represents a character's ability.
/// </summary>
/// <summary>
/// A script for a UI button that represents a character's ability.
/// It holds a reference to an ability and a caster, and executes the
/// action via the CombatManager when clicked.
/// </summary>
public class ActionButton : MonoBehaviour
{
    [Tooltip("The UI Text element that displays the ability's name.")]
    public Text abilityNameText;

    private Ability assignedAbility;
    private Character caster;

    // In a real game, a more robust targeting system would set this.
    private Character currentTarget;

    /// <summary>
    /// Initializes the action button with a specific ability and caster.
    /// </summary>
    /// <param name="ability">The ability this button will represent.</param>
    /// <param name="caster">The character who will use the ability.</param>
    public void Initialize(Ability ability, Character caster)
    {
        assignedAbility = ability;
        this.caster = caster;
        abilityNameText.text = ability.abilityName;

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// Handles the button's click event. Finds a target and tells the CombatManager to perform the action.
    /// </summary>
    private void OnButtonClick()
    {
        // For now, let's assume a simple targeting system.
        // Find the first alive enemy to target.
        foreach (var enemy in FindObjectsOfType<Character>())
        {
            if (enemy.CompareTag("Enemy") && enemy.isAlive)
            {
                currentTarget = enemy;
                break;
            }
        }

        if (caster != null && currentTarget != null && assignedAbility != null)
        {
            Debug.Log($"UI: Player chose to use '{assignedAbility.abilityName}' on '{currentTarget.characterName}'.");
            CombatManager.Instance.PlayerAction(caster, currentTarget, assignedAbility);
        }
        else
        {
            Debug.LogWarning("UI: Action could not be performed. Caster, target, or ability is missing.");
        }
    }
}