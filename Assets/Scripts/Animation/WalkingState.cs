// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// Represents the walking state for the character's animation state machine.
/// </summary>
public class WalkingState : AnimationState
{
    private readonly Animator _animator;
    private const string WalkingAnimationName = "Walking";

    /// <summary>
    /// Initializes a new instance of the <see cref="WalkingState"/> class.
    /// </summary>
    /// <param name="animator">The Animator component to control.</param>
    public WalkingState(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// Called when entering the walking state. Plays the walking animation.
    /// </summary>
    public override void Enter()
    {
        Debug.Log("Entering Walking State");
        _animator.Play(WalkingAnimationName);
    }

    /// <summary>
    /// Called every frame while in the walking state. No action needed.
    /// </summary>
    public override void Update()
    {
        // Transition logic is handled by the CharacterAnimationController.
    }

    /// <summary>
    /// Called when exiting the walking state.
    /// </summary>
    public override void Exit()
    {
        Debug.Log("Exiting Walking State");
        // No specific exit behavior needed.
    }
}
