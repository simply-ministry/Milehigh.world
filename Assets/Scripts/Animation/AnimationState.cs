// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// An abstract base class for all animation states in a state machine.
/// It defines the core methods that every state must implement.
/// </summary>
public abstract class AnimationState
{
    /// <summary>
    /// Called when entering this state. Use for initialization.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Called every frame while this state is active. Use for state logic.
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Called when exiting this state. Use for cleanup.
    /// </summary>
    public abstract void Exit();
}
