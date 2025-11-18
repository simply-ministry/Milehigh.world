using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

// This is an Editor script. It must be placed in a folder named "Editor".
public class CharacterFactory
{
// Path to the JSON file in the Assets folder.
private static string jsonPath = "/characters.json";

// This method is called from the Unity Editor menu.
[MenuItem("Milehigh.World/Generate Characters from JSON")]
public static void GenerateCharacters()
{
string fullPath = Application.dataPath + jsonPath;
if (!File.Exists(fullPath))
{
Debug.LogError("JSON file not found at: " + fullPath);
return;
}

string jsonString = File.ReadAllText(fullPath);
CharacterList characterList = JsonUtility.FromJson<CharacterList>(jsonString);

foreach (var charInfo in characterList.characters)
{
// Define the path where the asset will be created.
string assetPath = $"Assets/CharacterData/{charInfo.name}.asset";

CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
if (characterData == null)
{
// If the asset doesn't exist, create a new one.
characterData = ScriptableObject.CreateInstance<CharacterData>();
AssetDatabase.CreateAsset(characterData, assetPath);
}

// Populate the ScriptableObject with data from the JSON.
characterData.characterName = charInfo.name;
characterData.title = charInfo.title;
characterData.description = charInfo.description;
characterData.strength = charInfo.stats.strength;
characterData.dexterity = charInfo.stats.dexterity;
// ... and so on for all other stats.

// Mark the asset as "dirty" to ensure changes are saved.
EditorUtility.SetDirty(characterData);
}

// Save all changes to the assets.
AssetDatabase.SaveAssets();
AssetDatabase.Refresh();
Debug.Log("Character assets generated successfully from JSON!");
}

// Helper classes to match the JSON structure.
[System.Serializable]
private class CharacterList
{
public List<CharacterInfo> characters;
}

[System.Serializable]
private class CharacterInfo
{
public string name;
public string title;
public string description;
public StatInfo stats;
}

[System.Serializable]
private class StatInfo
{
public int strength;
public int dexterity;
// ... etc
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.