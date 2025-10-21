// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Handles character-specific actions, animations, and VFX for a narrative scene.
/// This script is meant to be attached to individual character GameObjects.
/// </summary>
public class CharacterActionHandler : MonoBehaviour
{
    [Tooltip("Optional: A custom name for logging. If empty, uses GameObject's name.")]
    [SerializeField] private string _characterNameOverride;

    [Tooltip("Reference to the Animator component for this character.")]
    public Animator characterAnimator;

    // Public property to get the character's display name
    public string characterName
    {
        get { return string.IsNullOrEmpty(_characterNameOverride) ? gameObject.name : _characterNameOverride; }
        set { _characterNameOverride = value; }
    }

    void Awake()
    {
        // If animator isn't explicitly assigned, try to find it on the GameObject or its children.
        if (characterAnimator == null)
        {
            characterAnimator = GetComponentInChildren<Animator>();
        }
    }

    /// <summary>
    /// Plays a specified animation on the character's Animator.
    /// Assumes the animation exists as a trigger or a state name.
    /// </summary>
    /// <param name="animName">The name of the animation trigger or state.</param>
    public void PlayAnimation(string animName)
    {
        if (characterAnimator != null && characterAnimator.isActiveAndEnabled)
        {
            // You might use SetTrigger, SetBool, SetInteger, or Play depending on your Animator setup
            // For simple one-shot actions, SetTrigger is often appropriate.
            // Example: characterAnimator.SetTrigger(animName);
            Debug.Log($"[{characterName}] Playing animation: '{animName}'");
            // In a real project: characterAnimator.SetTrigger(animName); or characterAnimator.Play(animName);
        }
        else
        {
            Debug.LogWarning($"[{characterName}] Animator not found or not active for animation: '{animName}'");
        }
    }

    /// <summary>
    /// Triggers a visual effect associated with an action.
    /// In a real game, this would interface with a VFX Manager or instantiate prefabs.
    /// </summary>
    /// <param name="vfxName">The name of the VFX to trigger.</param>
    public void TriggerVFX(string vfxName)
    {
        Debug.Log($"[{characterName}] Triggering VFX: '{vfxName}'");
        // In a real project: Instantiate a VFX prefab, play a particle system, etc.
        // Example: Instantiate(Resources.Load<GameObject>($"VFX/{vfxName}"), transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Simulates a character performing an attack.
    /// </summary>
    /// <param name="attackName">The specific type of attack (e.g., "SwordSlash", "Fireball").</param>
    public void Attack(string attackName)
    {
        Debug.Log($"[{characterName}] Performing attack: '{attackName}'");
        PlayAnimation("Attack_" + attackName); // Assumes animation trigger like "Attack_SwordSlash"
        TriggerVFX("VFX_" + attackName); // Assumes VFX named "VFX_SwordSlash"
        // Add sound effects here: AudioManager.Instance.PlaySound("SFX_" + attackName);
    }

    /// <summary>
    /// Simulates a character casting a spell.
    /// </summary>
    /// <param name="spellName">The name of the spell being cast.</param>
    public void CastSpell(string spellName)
    {
        Debug.Log($"[{characterName}] Casting spell: '{spellName}'");
        PlayAnimation("Cast_" + spellName); // Assumes animation trigger like "Cast_ShadowTendrils"
        TriggerVFX("VFX_" + spellName); // Assumes VFX named "VFX_ShadowTendrils"
        // Add sound effects here: AudioManager.Instance.PlaySound("SFX_" + spellName);
    }

    /// <summary>
    /// Simulates a character moving to a position or performing a charge.
    /// </summary>
    /// <param name="targetPosition">The target world position for the movement.</param>
    public void Move(Vector3 targetPosition)
    {
        Debug.Log($"[{characterName}] Moving towards target: {targetPosition}");
        PlayAnimation("Move"); // General move animation (e.g., "Walk", "Run", "Charge")
        // In a real project, you'd use NavMeshAgent, Rigidbody.MovePosition, or a custom movement script.
        // Example: StartCoroutine(SmoothMoveToPosition(targetPosition, 2f));
    }

    /// <summary>
    /// Simulates a character taking a defensive stance or blocking.
    /// </summary>
    /// <param name="defenseType">The type of defense (e.g., "ShieldBlock", "Resilience").</param>
    public void Defend(string defenseType)
    {
        Debug.Log($"[{characterName}] Taking a defensive stance: '{defenseType}'");
        PlayAnimation("Defend_" + defenseType); // Assumes animation trigger like "Defend_ShieldBlock"
    }

    /// <summary>
    /// Simulates two characters clashing in combat.
    /// </summary>
    /// <param name="opponentName">The name of the character being clashed with, for logging.</param>
    public void Clash(string opponentName)
    {
        Debug.Log($"[{characterName}] Clashing with {opponentName}!");
        PlayAnimation("Clash");
        TriggerVFX("VFX_Impact"); // Generic impact VFX
    }

    // You can add more specific action methods here as your scene demands.
    // E.g., TakeDamage(), Die(), Idle(), Taunt(), Teleport(), etc.
}