// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)

using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the narrative sequence for the Onalym Nexus mission.
/// This script controls the dialogue and key events for the team entering the Nexus.
/// </summary>
public class OnalymNexusScene : MonoBehaviour
{
    // === TEAM MEMBERS ===
    /// <summary>
    /// Reference to the Sky.ix character's action handler.
    /// </summary>
    public CharacterActionHandler skyix;
    /// <summary>
    /// Reference to the Kai character's action handler.
    /// </summary>
    public CharacterActionHandler kai;
    /// <summary>
    /// Reference to the Zaia character's action handler.
    /// </summary>
    public CharacterActionHandler zaia;
    /// <summary>
    /// Reference to the Micah character's action handler.
    /// </summary>
    public CharacterActionHandler micah;
    /// <summary>
    /// Reference to the Otis/X character's action handler.
    /// </summary>
    public CharacterActionHandler otis;

    /// <summary>
    /// A delegate defining the signature for dialogue actions.
    /// </summary>
    /// <param name="speaker">The name of the character speaking.</param>
    /// <param name="text">The line of dialogue to be displayed.</param>
    public delegate void DialogueAction(string speaker, string text);

    /// <summary>
    /// An event that is fired to display a line of dialogue.
    /// A UI manager should subscribe to this event to show the text to the player.
    /// </summary>
    public static event DialogueAction OnDialogue;

    /// <summary>
    /// Called when the script instance is being loaded. Starts the scene sequence.
    /// </summary>
    void Start()
    {
        BeginScene();
    }

    /// <summary>
    /// Initiates the main scene coroutine.
    /// </summary>
    void BeginScene()
    {
        Debug.Log("Mission Start: The team prepares to enter the Onalym Nexus.");
        StartCoroutine(SceneSequence());
    }

    /// <summary>
    /// Coroutine that controls the step-by-step flow of the Onalym Nexus scene.
    /// </summary>
    /// <returns>An IEnumerator to be used by StartCoroutine.</returns>
    IEnumerator SceneSequence()
    {
        // --- SCENE NARRATIVE & STAGING ---
        Debug.Log("The scene opens on the rain-slicked, neon-lit lower sectors of ŁĪƝĈ. Data-stream ghosts flicker and distort.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Omega.one", "Attention, designated assets. We are detecting an anomalous energy surge of unprecedented magnitude originating from the core of the Onalym Nexus within ŁĪƝĈ.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Omega.one", "This surge is actively corrupting the city's data-streams and attracting Void entities to the lower levels.");
        yield return new WaitForSeconds(5f);

        // --- TEAM BRIEFING & ACTIONS ---
        ShowDialogue("Omega.one", "Sky.ix, your intimate knowledge of the Nexus architecture is paramount to navigating the facility and accessing the core stabilization controls.");
        if (skyix != null) skyix.PlayAnimation("ScanEnvironment");
        Debug.Log("Sky.ix's cybernetic eyes scan the decaying entryways, her bionic arm glowing.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Omega.one", "Kai, your analytical mind is required to decipher the anomaly's signature and anticipate its behavior.");
        if (kai != null) kai.PlayAnimation("AnalyzeData");
        Debug.Log("Kai raises a hand, analyzing invisible lines of corrupted data.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Omega.one", "Zaia, your presence is needed to shield the team from the immense spiritual corruption bleeding from the rift.");
        if (zaia != null) zaia.CastSpell("ActivateMagen");
        Debug.Log("A golden luminescence emanates from Zaia, forming a protective Magen.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Omega.one", "Micah, you will act as an anchor for your father; the Nexus is where Otis was lost, and its influence will be a severe test of his fractured will.");
        yield return new WaitForSeconds(4f);

        if (otis != null) otis.PlayAnimation("Struggle");
        Debug.Log("Otis/X stands rigid, trembling as corrupted memories flash through his mind.");
        yield return new WaitForSeconds(2f);

        if (micah != null) micah.PlayAnimation("Reassure");
        Debug.Log("Micah places a firm, reassuring hand on his father's shoulder.");
        yield return new WaitForSeconds(2f);

        ShowDialogue("Omega.one", "We need the Sentinel, not the puppet.");
        yield return new WaitForSeconds(3f);

        // --- DIALOGUE SCRIPT ---
        ShowDialogue("Sky.ix", "The core stabilization mainframe is three levels down, past the chronosynch labs. It’s a straight path, but the architecture will be… unstable. No detours.");
        yield return new WaitForSeconds(6f);

        ShowDialogue("Kai", "This isn't just a power surge. The anomaly has a signature, a malicious intelligence. It’s rewriting the Nexus’s security protocols into viral code. It learns. It's Lucent's methodology.");
        yield return new WaitForSeconds(7f);

        ShowDialogue("Zaia", "His logic is a perversion of order. It will not stand. My Magen will protect our spirits. Focus on the mission, not the ghosts in the machine.");
        yield return new WaitForSeconds(6f);

        ShowDialogue("Micah", "Dad. You hear me? This is just another mission. Like the old days. Focus on my voice. I am your anchor. I am here.");
        yield return new WaitForSeconds(6f);

        ShowDialogue("Otis/X", "…separated… the file is partitioned… they were… right there…");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Sky.ix", "The Void taunts with what it takes. It weaponizes memory. We all lost something in this place, Sentinel.");
        yield return new WaitForSeconds(6f);

        ShowDialogue("Micah", "And we're here to take it back. What was broken can be reforged. Stay with us, Father.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Zaia", "Enough talk. Every moment we delay, the wound deepens. Judgment awaits. We move now.");
        if (zaia != null) zaia.PlayAnimation("ReadyStance");
        yield return new WaitForSeconds(5f);

        EndScene();
    }

    /// <summary>
    /// Fires the OnDialogue event to display a line of text.
    /// If no UI manager is subscribed, it logs the text to the console as a fallback.
    /// </summary>
    /// <param name="speaker">The name of the character speaking.</param>
    /// <param name="text">The dialogue text to show.</param>
    void ShowDialogue(string speaker, string text)
    {
        if (OnDialogue != null)
        {
            OnDialogue(speaker, text);
        }
        else
        {
            Debug.Log($"<color=cyan>[{speaker}]</color> {text}"); // Fallback for logging
        }
    }

    /// <summary>
    /// Marks the end of the scene and logs a completion message.
    /// </summary>
    void EndScene()
    {
        Debug.Log("The team moves into the Onalym Nexus. The mission has begun.");
    }
}
