// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;

/// <summary>
/// An abstract base class for managing scene-specific logic, events, and transitions.
/// Each major scene in the game should have a corresponding controller that inherits from this class.
/// </summary>
public abstract class SceneController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("A descriptive name for the scene, used for logging and identification.")]
    public string sceneName;

    [Tooltip("The current state of the scene's primary narrative or gameplay sequence.")]
    protected string currentState = "Initialization";

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            // Use the GameObject's name as a fallback if sceneName is not set.
            sceneName = gameObject.name;
        }
        Debug.Log($"[{sceneName}] Scene Controller is waking up.");
    }

    /// <summary>
    /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// This is where the main scene sequence should be initiated.
    /// </summary>
    protected virtual void Start()
    {
        Debug.Log($"[{sceneName}] Starting scene sequence.");
        StartCoroutine(SceneSequence());
    }

    /// <summary>
    /// The main coroutine that defines the scene's flow, including narrative events,
    /// character spawning, and objectives. This must be implemented by inheriting classes.
    /// </summary>
    /// <returns>An IEnumerator to allow for sequencing over multiple frames.</returns>
    protected abstract IEnumerator SceneSequence();

    /// <summary>
    /// Updates the state of the scene and logs the change.
    /// </summary>
    /// <param name="newState">A descriptive name for the new state.</param>
    protected void UpdateState(string newState)
    {
        if (currentState != newState)
        {
            Debug.Log($"[{sceneName}] State changed from '{currentState}' to '{newState}'.");
            currentState = newState;
        }
    }

    /// <summary>
    /// Provides a standardized way to log messages specific to this scene.
    /// </summary>
    /// <param name="message">The log message.</param>
    protected void Log(string message)
    {
        Debug.Log($"[{sceneName}] {message}");
    }
}