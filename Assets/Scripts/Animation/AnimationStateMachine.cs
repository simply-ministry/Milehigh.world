// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// A simple state machine for managing animation states.
/// It handles the initialization, transition, and updating of states.
/// </summary>
public class AnimationStateMachine : MonoBehaviour
{
    /// <summary>
    /// The currently active animation state.
    /// </summary>
    public AnimationState CurrentState { get; private set; }

    /// <summary>
    /// Initializes the state machine with a starting state.
    /// </summary>
    /// <param name="startingState">The initial state to enter.</param>
    public void Initialize(AnimationState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Transitions from the current state to a new state.
    /// </summary>
    /// <param name="newState">The new state to enter.</param>
    public void ChangeState(AnimationState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Called every frame. Updates the current state.
    /// </summary>
    void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
