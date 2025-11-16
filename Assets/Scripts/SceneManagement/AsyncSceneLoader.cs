using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// This script manages asynchronous scene loading with a progress bar.
// Attach this to a GameObject in your loading screen scene.
public class AsyncSceneLoader : MonoBehaviour
{
[Tooltip("The UI Slider to display loading progress.")]
public Slider progressBar;
[Tooltip("The name of the scene to load.")]
public string sceneToLoad;

void Start()
{
// Start the loading process as soon as the scene begins.
StartCoroutine(LoadSceneAsyncProcess());
}

private IEnumerator LoadSceneAsyncProcess()
{
// Start loading the scene in the background.
AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

// Prevent the scene from activating as soon as it's finished loading.
asyncLoad.allowSceneActivation = false;

// While the scene is still loading...
while (!asyncLoad.isDone)
{
// The loading process halts at 0.9 when allowSceneActivation is false.
// We can use this to create a smooth progress bar.
float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

if (progressBar != null)
{
progressBar.value = progress;
}

// Check if the loading is almost complete.
if (asyncLoad.progress >= 0.9f)
{
// You could add a "Press any key to continue" prompt here.
// For this example, we'll just activate it automatically.
Debug.Log("Scene is ready to activate. Activating now...");
asyncLoad.allowSceneActivation = true;
}

yield return null; // Wait for the next frame.
}
}
}

// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
// Copyright © 2024 The Mile-High Mythographers. All rights reserved.