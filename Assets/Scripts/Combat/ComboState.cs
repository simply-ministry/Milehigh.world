// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// A data class that holds the state of a character's combo.
/// This includes the current hit count, the timestamp of the last hit,
/// and a reference to the coroutine that will reset the combo.
/// </summary>
public class ComboState
{
    /// <summary>
    /// The number of consecutive hits in the current combo.
    /// </summary>
    public int HitCount { get; set; }

    /// <summary>
    /// The time (using Time.time) when the last hit of the combo occurred.
    /// </summary>
    public float LastHitTime { get; set; }

    /// <summary>
    /// A reference to the active coroutine that is counting down to reset this combo.
    /// This is stored so it can be stopped if a new hit lands, extending the combo window.
    /// </summary>
    public Coroutine ResetCoroutine { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboState"/> class.
    /// </summary>
    public ComboState()
    {
        HitCount = 0;
        LastHitTime = 0f;
        ResetCoroutine = null;
    }
}