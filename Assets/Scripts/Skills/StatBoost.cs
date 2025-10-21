// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;

/// <summary>
/// An enum to define the character stats that can be boosted by skills.
/// </summary>
public enum StatType
{
    MaxHealth,
    Attack,
    Defense,
    Speed
}

/// <summary>
/// A simple, serializable class to define a stat modification.
/// This allows for defining boosts like "+10 Health" or "+5 Attack" in the Unity Inspector.
/// </summary>
[System.Serializable]
public class StatBoost
{
    [Tooltip("The type of stat to be modified.")]
    public StatType stat;

    [Tooltip("The value of the modification.")]
    public int value;
}