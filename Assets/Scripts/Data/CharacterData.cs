using UnityEngine;

// This ScriptableObject acts as a data container for character stats.
// You can create instances of this in the Project window via:
// Assets > Create > Milehigh/Character Data
[CreateAssetMenu(fileName = "NewCharacter", menuName = "Milehigh/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Identity")]
    public string characterName;
    public string title;
    [TextArea(3, 5)]
    public string description;

    [Header("Core Stats")]
    public float health;
    public int strength;
    public int dexterity;
    public int defense;
    public int vigor;
    public int heart;

    [Header("Affinities & Multipliers")]
    public float resonance;
    public float integrity;
    public float vanguardMultiplier = 1.0f;
    public int voidAffinity;
    public int nexusAttunement;
}

namespace Milehigh.World.Core
{
    /// <summary>
    /// A lightweight data structure for passing character attributes between systems.
    /// </summary>
    [System.Serializable]
    public struct CharacterDataStruct
    {
        public string characterName;
        public float health;
        public float resonance;
        public float integrity;
        public float vanguardMultiplier;

        /// <summary>
        /// Creates a struct copy from a CharacterData ScriptableObject.
        /// </summary>
        public static CharacterDataStruct FromScriptableObject(CharacterData data)
        {
            return new CharacterDataStruct
            {
                characterName = data.characterName,
                health = data.health,
                resonance = data.resonance,
                integrity = data.integrity,
                vanguardMultiplier = data.vanguardMultiplier
            };
        }
    }
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.