using UnityEngine;
using System.Collections;
using System; // Add this for Action

/// <summary>
/// Manages a specific, hardcoded dialogue sequence between Ingris and Zaya.
/// This script uses a coroutine to display dialogue lines with timed pauses
/// and fires an event when the sequence is complete.
/// </summary>
public class IngrisZayaDialogue : MonoBehaviour
{
    [Tooltip("The time in seconds to pause between each line of dialogue.")]
    public float dialoguePause = 2f;
    /// <summary>
    /// An event that is invoked when the dialogue sequence has finished.
    /// Other scripts can subscribe to this event to trigger actions after the dialogue.
    /// </summary>
    public static event Action OnDialogueEnd;

    /// <summary>
    /// Starts the dialogue coroutine.
    /// This method displays a sequence of lines from Zaya and Ingris.
    /// </summary>
    /// <returns>An IEnumerator to be used by StartCoroutine.</returns>
    public IEnumerator StartDialogue()
    {
        // Dialogue
        Debug.Log("Zaya: I've heard tales of your fiery wrath, Phoenix Warrior. They say you leave nothing but ash in your wake.");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Ingris: And what of it, archer? Are you here to test those tales?");
        yield return new WaitForSeconds(dialoguePause);
        Debug.Log("Zaya: I'm here to ensure this land doesn't become another of your conquests.");
        yield return new WaitForSeconds(dialoguePause / 2);
        // Signal dialogue is over
        OnDialogueEnd?.Invoke();
    }
}