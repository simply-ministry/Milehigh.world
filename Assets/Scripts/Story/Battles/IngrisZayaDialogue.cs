using UnityEngine;
using System.Collections;
using System; // Add this for Action

public class IngrisZayaDialogue : MonoBehaviour
{
    public float dialoguePause = 2f;
    public static event Action OnDialogueEnd; // Event to signal dialogue is over

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