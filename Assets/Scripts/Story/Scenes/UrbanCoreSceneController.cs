// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the scene logic for Linq, the central hub of Milehigh.World.
/// This controller handles initial player spawn points, key NPC introductions,
/// and the primary quest lines that originate in this area.
/// </summary>
public class UrbanCoreSceneController : SceneController
{
    // --- Scene-Specific References ---
    // In a full implementation, you would link these in the Unity Editor.
    // [Header("Scene-Specific References")]
    // public Transform playerSpawnPoint;
    // public GameObject keyNpcPrefab;

    /// <summary>
    /// Overrides the base Start method to set the scene name before the sequence begins.
    /// </summary>
    protected override void Start()
    {
        sceneName = "Linq";
        base.Start();
    }

    /// <summary>
    /// Defines the narrative and gameplay sequence for the Linq scene.
    /// This coroutine will guide the player through the initial experience of the area.
    /// </summary>
    protected override IEnumerator SceneSequence()
    {
        // State 1: Initialization
        UpdateState("Initializing Linq");
        Log("The towering chrome spires of Linq pierce the clouds. Welcome to the heart of the city.");
        yield return new WaitForSeconds(2f); // Wait for 2 seconds to let the player take in the view.

        // State 2: Introduce a Key NPC (Placeholder)
        UpdateState("NPC Introduction");
        Log("A holographic figure flickers to life nearby, its voice a calm, synthesized tone: 'Greetings, Noveminaad. I am the Steward. Your journey begins here.'");
        // In a real game, you would instantiate and position the NPC here.
        yield return new WaitForSeconds(3f);

        // State 3: Assigning the First Objective
        UpdateState("First Objective");
        Log("The Steward continues, 'Corruption has taken root in the lower sectors. Seek out the source. Your path is your own, but the city's fate is in your hands.'");
        // Here, you would interface with a QuestManager to assign the first quest.
        yield return new WaitForSeconds(1f);

        // State 4: Scene is now in an 'Active' or 'Idle' state
        UpdateState("Active");
        Log("Linq is now fully active. The player is free to explore.");

        // The coroutine concludes, but the scene controller remains active for any future events.
    }
}