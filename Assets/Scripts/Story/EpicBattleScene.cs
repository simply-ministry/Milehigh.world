using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the narrative sequence for an epic battle scene in the ruins of Āɲč̣ịəŋṭ^Łīɲč̣.
/// This script controls a choreographed sequence of dialogue and actions between heroes and villains.
/// </summary>
public class EpicBattleScene : MonoBehaviour
{
    // === VILLAINS ===
    /// <summary>
    /// Reference to the Nafaerius character GameObject.
    /// </summary>
    public GameObject nafaerius;
    /// <summary>
    /// Reference to the Cyrus character GameObject.
    /// </summary>
    public GameObject cyrus;
    /// <summary>
    /// Reference to the Lucent character GameObject.
    /// </summary>
    public GameObject lucent;
    /// <summary>
    /// Reference to the Era (Corrupted Void) character GameObject.
    /// </summary>
    public GameObject era;
    /// <summary>
    /// Reference to the Delilah character GameObject.
    /// </summary>
    public GameObject delilah;
    /// <summary>
    /// Reference to The Omen character GameObject.
    /// </summary>
    public GameObject theOmen;
    /// <summary>
    /// Reference to the Kane character GameObject.
    /// </summary>
    public GameObject kane;

    // === HEROES (Ɲōvəmîŋāđ) ===
    /// <summary>
    /// Reference to the Anastasia character GameObject.
    /// </summary>
    public GameObject anastasia;
    /// <summary>
    /// Reference to the Reverie character GameObject.
    /// </summary>
    public GameObject reverie;
    /// <summary>
    /// Reference to the Aeron character GameObject.
    /// </summary>
    public GameObject aeron;
    /// <summary>
    /// Reference to the Zaia character GameObject.
    /// </summary>
    public GameObject zaia;
    /// <summary>
    /// Reference to the Micah character GameObject.
    /// </summary>
    public GameObject micah;
    /// <summary>
    /// Reference to the Kael character GameObject.
    /// </summary>
    public GameObject kael;

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
        // Initial staging: position characters, set animations, etc.
        Debug.Log("The final battle for Mîlēhîgh.wørld begins in the ruins of Āɲč̣ịəŋṭ^Łīɲč̣.");
        StartCoroutine(SceneSequence());
    }

    /// <summary>
    /// Coroutine that controls the step-by-step flow of the battle scene,
    /// including dialogue and descriptions of character actions.
    /// </summary>
    /// <returns>An IEnumerator to be used by StartCoroutine.</returns>
    IEnumerator SceneSequence()
    {
        ShowDialogue("In the shattered ruins of Āɲč̣ịəŋṭ^Łīɲč̣, the air crackles with a palpable tension.");
        yield return new WaitForSeconds(3f);

        ShowDialogue("On one side: Nafaerius, Cyrus, Lucent, Delilah, The Omen, and Kane, a coalition of chaos and corruption.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("On the other: Anastasia, Reverie, Aeron, Zaia, Kael, and Micah—the Ɲōvəmîŋāđ, protectors of Mîlēhîgh.wørld.");
        yield return new WaitForSeconds(4f);

        // --- THE VILLAINS ATTACK ---
        ShowDialogue("Nafaerius unleashes shadow tendrils that snake towards the Ɲōvəmîŋāđ.");
        // TODO: Play Nafaerius's shadow tendril attack animation and VFX.
        yield return new WaitForSeconds(3f);

        ShowDialogue("Cyrus roars, summoning interdimensional rifts that spit forth distorted energies.");
        // TODO: Play Cyrus's rift summoning animation and VFX.
        yield return new WaitForSeconds(3f);

        ShowDialogue("At Lucent's command, the corrupted Void, Era, surges forward like a wave of pure corruption.");
        // TODO: Play Era's surge animation and VFX.
        yield return new WaitForSeconds(3f);

        ShowDialogue("With The Omen soaring above, Delilah launches a devastating volley of decay-infused projectiles.");
        // TODO: Play Delilah's projectile attack and The Omen's fly-over animation.
        yield return new WaitForSeconds(3f);

        ShowDialogue("Driven by bitter conviction, Kane charges directly at his brother, Aeron.");
        // TODO: Play Kane's charge animation towards Aeron.
        yield return new WaitForSeconds(2f);

        // --- THE HEROES RESPOND ---
        ShowDialogue("But the Ɲōvəmîŋāđ meet them head-on. Anastasia conjures a shimmering barrier, deflecting the initial assault.");
        // TODO: Play Anastasia's barrier spell animation and VFX.
        yield return new WaitForSeconds(4f);

        ShowDialogue("Reverie's illusions disorient the advancing shadows, creating openings for Zaia's swift, precise attacks.");
        // TODO: Play Reverie's illusion VFX and Zaia's attack animation.
        yield return new WaitForSeconds(4f);

        ShowDialogue("Kael bends time itself, momentarily slowing the chaotic energies unleashed by Cyrus.");
        // TODO: Play Kael's time manipulation VFX and apply slow-motion effect to enemy projectiles.
        yield return new WaitForSeconds(4f);

        ShowDialogue("Micah the Unbreakable stands firm, his form radiating resilience as he shrugs off hits that would fell lesser beings.");
        // TODO: Play Micah's defensive stance animation and damage absorption VFX.
        yield return new WaitForSeconds(4f);

        ShowDialogue("Aeron meets Kane's charge with a fierce cry, their clash of power ripping through the air.");
        // TODO: Play Aeron and Kane's clash animation and impact VFX.
        yield return new WaitForSeconds(4f);

        ShowDialogue("The battle for Mîlēhîgh.wørld has truly begun, a symphony of destruction and desperate hope.");
        yield return new WaitForSeconds(5f);

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
            Debug.Log(text); // Fallback for logging
        }
    }

    /// <summary>
    /// Marks the end of the scene and logs a completion message.
    /// </summary>
    void EndScene()
    {
        Debug.Log("The battle has been joined. The fate of Mîlēhîgh.wørld hangs in the balance.");
        // Logic for transitioning to gameplay or the next scene would go here.
    }
}