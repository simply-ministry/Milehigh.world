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
    private Character currentTarget;

    public void Initialize(Ability ability, Character caster)
    {
        assignedAbility = ability;
        this.caster = caster;
        abilityNameText.text = ability.abilityName;

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    // This would be called by a targeting system when the player selects an enemy
    public void SetTarget(Character target)
    {
        currentTarget = target;
    }

    private void OnButtonClick()
    {
        if (caster != null && currentTarget != null && assignedAbility != null)
        {
            Debug.Log($"UI: Player chose to use '{assignedAbility.abilityName}' on '{currentTarget.characterName}'.");
            CombatManager.Instance.PlayerAction(caster, currentTarget, assignedAbility);
        }
        else
        {
            Debug.LogWarning("UI: Action cannot be performed. Caster, target, or ability is missing.");
        }
    }
}