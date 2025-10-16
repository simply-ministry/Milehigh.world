using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the pivotal narrative scene where the Ɲōvəmîŋāđ meet for the first time.
/// This script controls the dialogue and sequence of events as the heroes confront their shared destiny.
/// </summary>
public class NoveminadMeetingScene : MonoBehaviour
{
    [Header("Character References")]
    /// <summary>
    /// Reference to the Aeron character GameObject.
    /// </summary>
    public GameObject aeron;
    /// <summary>
    /// Reference to the Lyra character GameObject.
    /// </summary>
    public GameObject lyra;
    /// <summary>
    /// A list of GameObjects representing all members of the Ɲōvəmîŋāđ present in the scene.
    /// </summary>
    public List<GameObject> noveminad = new List<GameObject>();

    [Header("Scene References")]
    /// <summary>
    /// The GameObject representing the Nexus Chamber where the meeting takes place.
    /// </summary>
    public GameObject nexusChamber;

    /// <summary>
    /// A delegate defining the signature for dialogue actions.
    /// </summary>
    /// <param name="text">The line of dialogue to be displayed.</param>
    public delegate void DialogueAction(string text);
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
        // In a real implementation, we would find or instantiate these GameObjects.
        // For this script, we assume they are assigned in the Unity Editor.
        BeginScene();
    }

    /// <summary>
    /// Initiates the main scene coroutine.
    /// </summary>
    void BeginScene()
    {
        // Initial staging (positioning characters, playing animations, etc.) would happen here.
        StartCoroutine(SceneSequence());
    }

    /// <summary>
    /// Coroutine that controls the step-by-step flow of the meeting scene,
    /// including dialogue and character reactions.
    /// </summary>
    /// <returns>An IEnumerator to be used by StartCoroutine.</returns>
    IEnumerator SceneSequence()
    {
        // Dialogue sequence
        ShowDialogue("Aeron steps forward, his hand outstretched towards a shimmering portal.");
        yield return new WaitForSeconds(2f);

        ShowDialogue("Aeron: (His voice strong, yet laced with a hint of vulnerability) Lyra? Can you hear me? It's time.");
        yield return new WaitForSeconds(3f);

        ShowDialogue("From the portal emerges a figure of ethereal beauty and strength – Lyra. She moves with a regal grace, her eyes holding ancient wisdom and a hint of sadness.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Zaiya: (Her voice sharp with caution) She is powerful. Can we trust her?");
        yield return new WaitForSeconds(3f);

        ShowDialogue("Aeron: (Turns, his hand resting on Zaiya's arm) She is my mate, Zaiya. She has seen what the Void can do, just like I have.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Lyra: (Her gaze sweeps across the Noveminad) I know why you are here. The shadow of the Void lengthens, threatening to consume all that we hold dear.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Omega.one: (Its voice synthesized and curious) You know of the Void's nature?");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Lyra: I have walked its desolate paths, witnessed its corrupting influence. It is a hunger that cannot be sated, a darkness that seeks to unravel the very fabric of existence.");
        yield return new WaitForSeconds(6f);

        ShowDialogue("Ingris/Delilah: And what do you propose we do about it?");
        yield return new WaitForSeconds(3f);

        ShowDialogue("Lyra: We must unite. The prophecy speaks of ten who will stand against the Void. Ten who will either save us or doom us all. You are those ten.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Reverie: (Intrigued) Ten? Like the prophecy of Lîŋq?");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Kai: (His eyes glowing faintly) The threads of fate converge. I have foreseen this meeting, though its path was shrouded in mist.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Micah: (His voice firm with resolve) Then we stand together. For Milehigh.World. For Millenia.");
        yield return new WaitForSeconds(3f);

        ShowDialogue("Aeron: We have been brought together for a reason. We must learn to trust each other, to fight as one, if we are to have any hope of overcoming the darkness that lies ahead.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("Ingris/Delilah: (A flicker of her former self in her eyes) Trust... a luxury we can ill afford.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("Lyra: The time for debate is over. The Void is advancing, and we must stand now, or fall forever.");
        yield return new WaitForSeconds(5f);

        ShowDialogue("(The Noveminad stand, a mix of determination and uncertainty on their faces, as the camera focuses on their resolve (or lack thereof). The fate of Milehigh.World hangs in the balance.)");
        yield return new WaitForSeconds(2f);

        EndScene();
    }

    /// <summary>
    /// Fires the OnDialogue event to display a line of text.
    /// If no UI manager is subscribed, it logs the text to the console as a fallback.
    /// </summary>
    /// <param name="text">The dialogue text to show.</param>
    void ShowDialogue(string text)
    {
        if (OnDialogue != null)
        {
            OnDialogue(text);
        }
        else
        {
            Debug.Log(text); // Basic fallback
        }
    }

    /// <summary>
    /// Marks the end of the scene and logs a completion message.
    /// </summary>
    void EndScene()
    {
        Debug.Log("Noveminad Meeting Scene Ended.");
        // Logic for transitioning to gameplay or the next scene would go here.
    }
}