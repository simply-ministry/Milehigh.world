// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Represents the idle state for the character's animation state machine.
/// </summary>
public class IdleState : AnimationState
{
    private readonly Animator _animator;
    private const string IdleAnimationName = "Idle";

    /// <summary>
    /// Initializes a new instance of the <see cref="IdleState"/> class.
    /// </summary>
    /// <param name="animator">The Animator component to control.</param>
    public IdleState(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// Called when entering the idle state. Plays the idle animation.
    /// </summary>
    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        _animator.Play(IdleAnimationName);
    }

    /// <summary>
    /// Called every frame while in the idle state. No action needed.
    /// </summary>
    public override void Update()
    {
        // Transition logic is handled by the CharacterAnimationController.
    }

    /// <summary>
    /// Called when exiting the idle state.
    /// </summary>
    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
        // No specific exit behavior needed.
    }
}
