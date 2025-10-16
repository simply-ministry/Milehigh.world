using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the narrative sequence for an epic battle scene in the ruins of Āɲč̣ịəŋṭ^Łīɲč̣.
/// This script controls a choreographed sequence of dialogue and actions between heroes and villains.
/// </summary>
public class EpicBattleScene : MonoBehaviour
{
    // === VILLAINS ===
    // Now referencing CharacterActionHandler directly
    /// <summary>
    /// Reference to the Nafaerius character's action handler.
    /// </summary>
    public CharacterActionHandler nafaerius;
    /// <summary>
    /// Reference to the Cyrus character's action handler.
    /// </summary>
    public CharacterActionHandler cyrus;
    /// <summary>
    /// Reference to the Lucent character's action handler.
    /// </summary>
    public CharacterActionHandler lucent;
    /// <summary>
    /// Reference to the Era (Corrupted Void) character's action handler.
    /// </summary>
    public CharacterActionHandler era;
    /// <summary>
    /// Reference to the Delilah character's action handler.
    /// </summary>
    public CharacterActionHandler delilah;
    /// <summary>
    /// Reference to The Omen character's action handler.
    /// </summary>
    public CharacterActionHandler theOmen;
    /// <summary>
    /// Reference to the Kane character's action handler.
    /// </summary>
    public CharacterActionHandler kane;

    // === HEROES (Ɲōvəmîŋāđ) ===
    /// <summary>
    /// Reference to the Anastasia character's action handler.
    /// </summary>
    public CharacterActionHandler anastasia;
    /// <summary>
    /// Reference to the Reverie character's action handler.
    /// </summary>
    public CharacterActionHandler reverie;
    /// <summary>
    /// Reference to the Aeron character's action handler.
    /// </summary>
    public CharacterActionHandler aeron;
    /// <summary>
    /// Reference to the Zaia character's action handler.
    /// </summary>
    public CharacterActionHandler zaia;
    /// <summary>
    /// Reference to the Micah character's action handler.
    /// </summary>
    public CharacterActionHandler micah;
    /// <summary>
    /// Reference to the Kael character's action handler.
    /// </summary>
    public CharacterActionHandler kael;

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
        // In the Unity Editor, drag the GameObjects with CharacterActionHandler components
        // into the public fields of this script.
        BeginScene();
    }

    /// <summary>
    /// Initiates the main scene coroutine.
    /// </summary>
    void BeginScene()
    {
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
        // --- SCENE INTRO AND STAGING ---
        ShowDialogue("In the shattered ruins of Āɲč̣ịəŋṭ^Łīɲč̣, the air crackles with a palpable tension.");
        yield return new WaitForSeconds(3f);

        ShowDialogue("On one side: Nafaerius, Cyrus, Lucent, Delilah, The Omen, and Kane, a coalition of chaos and corruption.");
        yield return new WaitForSeconds(4f);

        ShowDialogue("On the other: Anastasia, Reverie, Aeron, Zaia, Kael, and Micah—the Ɲōvəmîŋāđ, protectors of Mîlēhîgh.wørld.");
        yield return new WaitForSeconds(4f);

        // --- THE VILLAINS ATTACK ---
        ShowDialogue("Nafaerius unleashes shadow tendrils that snake towards the Ɲōvəmîŋāđ.");
        if (nafaerius != null) { nafaerius.CastSpell("ShadowTendrils"); }
        yield return new WaitForSeconds(3.5f); // Increased time for a noticeable spell cast

        ShowDialogue("Cyrus roars, summoning interdimensional rifts that spit forth distorted energies.");
        if (cyrus != null) { cyrus.CastSpell("InterdimensionalRifts"); }
        yield return new WaitForSeconds(3.5f);

        ShowDialogue("At Lucent's command, the corrupted Void, Era, surges forward like a wave of pure corruption.");
        if (lucent != null) { lucent.PlayAnimation("Command"); } // Lucent gestures to command
        if (era != null) { era.Attack("VoidSurge"); } // Era performs its surge attack
        yield return new WaitForSeconds(3.5f);

        ShowDialogue("With The Omen soaring above, Delilah launches a devastating volley of decay-infused projectiles.");
        if (theOmen != null) { theOmen.PlayAnimation("Soar"); } // The Omen's specific movement/animation
        if (delilah != null) { delilah.Attack("DecayProjectiles"); }
        yield return new WaitForSeconds(3.5f);

        ShowDialogue("Driven by bitter conviction, Kane charges directly at his brother, Aeron.");
        // A placeholder for Kane to conceptually move towards Aeron's position
        if (kane != null && aeron != null) { kane.Move(aeron.transform.position); }
        yield return new WaitForSeconds(2.5f); // Time for the charge animation

        // --- THE HEROES RESPOND ---
        ShowDialogue("But the Ɲōvəmîŋāđ meet them head-on. Anastasia conjures a shimmering barrier, deflecting the initial assault.");
        if (anastasia != null) { anastasia.CastSpell("ShimmeringBarrier"); }
        yield return new WaitForSeconds(4f);

        ShowDialogue("Reverie's illusions disorient the advancing shadows, creating openings for Zaia's swift, precise attacks.");
        if (reverie != null) { reverie.CastSpell("Illusions"); }
        if (zaia != null) { zaia.Attack("SwiftStrike"); }
        yield return new WaitForSeconds(4f);

        ShowDialogue("Kael bends time itself, momentarily slowing the chaotic energies unleashed by Cyrus.");
        if (kael != null) { kael.CastSpell("TimeBend"); }
        yield return new WaitForSeconds(4f);

        ShowDialogue("Micah the Unbreakable stands firm, his form radiating resilience as he shrugs off hits that would fell lesser beings.");
        if (micah != null) { micah.Defend("Resilience"); }
        yield return new WaitForSeconds(4f);

        ShowDialogue("Aeron meets Kane's charge with a fierce cry, their clash of power ripping through the air.");
        if (aeron != null && kane != null)
        {
            aeron.Clash(kane.characterName); // Aeron performs a clash action
            kane.Clash(aeron.characterName); // Kane also performs a clash action
        }
        yield return new WaitForSeconds(4f);

        // --- SCENE CONCLUSION ---
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
            Debug.Log($"<color=lightblue>[DIALOGUE]</color> {text}"); // Fallback for logging with color
        }
    }

    /// <summary>
    /// Marks the end of the scene and logs a completion message.
    /// </summary>
    void EndScene()
    {
        Debug.Log("The battle has been joined. The fate of Mîlēhîgh.wørld hangs in the balance.");
        // Logic for transitioning to gameplay or the next scene would go here.
        // For example:
        // SceneManager.LoadScene("BattleArena");
        // GameManager.Instance.StartBattlePhase();
    }
}