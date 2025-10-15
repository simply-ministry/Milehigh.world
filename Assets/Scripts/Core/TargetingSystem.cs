using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages a list of potential targets for the player and allows cycling between them.
/// This component should be attached to the same GameObject as the PlayerController.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class TargetingSystem : MonoBehaviour
{
    [Header("Targeting")]
    [Tooltip("The list of available targets in the current combat encounter.")]
    [SerializeField] private List<Character> availableTargets = new List<Character>();

    private int currentTargetIndex = -1;
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Cycles to the next available target in the list.
    /// </summary>
    public void CycleTarget()
    {
        // Remove any dead targets from the list first.
        availableTargets = availableTargets.Where(t => t != null && t.CurrentHealth > 0).ToList();

        if (availableTargets.Count == 0)
        {
            playerController.SetTarget(null);
            Debug.Log("No targets available.");
            return;
        }

        currentTargetIndex++;
        if (currentTargetIndex >= availableTargets.Count)
        {
            currentTargetIndex = 0;
        }

        Character newTarget = availableTargets[currentTargetIndex];
        playerController.SetTarget(newTarget);
        Debug.Log($"Target switched to: {newTarget.characterName}");
    }

    /// <summary>
    /// Adds a target to the list of available targets.
    /// </summary>
    /// <param name="target">The character to add.</param>
    public void AddTarget(Character target)
    {
        if (target != null && !availableTargets.Contains(target))
        {
            availableTargets.Add(target);
        }
    }

    /// <summary>
    /// Removes a target from the list of available targets.
    /// </summary>
    /// <param name="target">The character to remove.</param>
    public void RemoveTarget(Character target)
    {
        if (target != null && availableTargets.Contains(target))
        {
            availableTargets.Remove(target);

            // If the removed target was the current target, select the next one or clear it.
            if (playerController.CurrentTarget == target)
            {
                if (availableTargets.Count > 0)
                {
                    // To avoid skipping a target, reset index and cycle.
                    currentTargetIndex = -1;
                    CycleTarget();
                }
                else
                {
                    playerController.SetTarget(null);
                    currentTargetIndex = -1;
                }
            }
        }
    }

    /// <summary>
    /// Clears all targets from the list.
    /// </summary>
    public void ClearTargets()
    {
        availableTargets.Clear();
        playerController.SetTarget(null);
        currentTargetIndex = -1;
    }
}