using UnityEditor;
using UnityEditor.SceneManagement;

// This is an editor script. It must be placed in a folder named "Editor".
// It adds a custom menu item to automate opening a multi-scene setup.
public class MultiSceneSetup
{
// The path to the menu item in the Unity Editor.
private const string MenuPath = "Milehigh.World/Open Scenes/Core Gameplay Setup";

[MenuItem(MenuPath)]
public static void OpenCoreGameplaySetup()
{
// Before opening new scenes, prompt the user to save any unsaved changes.
if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
{
Debug.Log("Restoring core gameplay multi-scene setup...");

// Define the array of scenes to open.
// The first scene is set as the active scene.
var sceneSetups = new SceneSetup[]
{
new SceneSetup { path = "Assets/Scenes/PersistentManagers.unity", isActive = true, isLoaded = true },
new SceneSetup { path = "Assets/Scenes/Core_UI.unity", isActive = false, isLoaded = true },
new SceneSetup { path = "Assets/Scenes/Level_Aethelgard.unity", isActive = false, isLoaded = true }
};

// Use RestoreSceneManagerSetup to open all defined scenes at once.
EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);

Debug.Log("Scene setup restored successfully.");
}
else
{
Debug.Log("User cancelled the operation.");
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.