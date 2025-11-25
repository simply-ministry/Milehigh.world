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
public int strength;
public int dexterity;
public int defense;
public int vigor;
public int heart;

[Header("Affinities")]
public int voidAffinity;
public int nexusAttunement;
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.