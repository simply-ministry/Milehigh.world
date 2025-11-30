// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)

// --- UNITY SCENE SETUP --- //
//
// 1. Create an empty GameObject in your scene and name it "SceneController".
// 2. Attach this script (`NightmareEchoConfrontation.cs`) to the "SceneController" GameObject.
// 3. Create or place the prefabs for the characters "Reverie," "Zaia," and "Kane" into the scene.
// 4. Ensure each character prefab has the following components attached:
//    - An Animator component with a configured Animator Controller.
//    - An AudioSource component to play their voice lines.
//    - Their respective ability script (e.g., Zaia_Character should have `Ability_Zaia.cs`).
// 5. Set up the UI (DialogueBox, SpeakerNameText, DialogueText).
// 6. Drag the corresponding GameObjects and UI elements into the public fields of this script.
// 7. Press Play to run the cinematic sequence.
//

using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the cinematic sequence for the mission: Infiltrating Kane's 'Nightmare Echo'.
/// This script manages the confrontation between Zaia, Reverie, and the projection of Kane
/// within his corrupted psychic realm.
/// </summary>
public class NightmareEchoConfrontation : MonoBehaviour
{
    // ====================================================================
    //
    // CHARACTER ASSET & VOICE REFERENCE BLOCK (Zaia, Reverie, Kane)
    //
    // ====================================================================

    // Protagonist: Zaia the The Just
    /* VOICE PROFILE: Uncompromising, Highly Formal, Logical. */
    [Header("Character & Audio References")]
    public GameObject Zaia_Character;
    public AudioSource Zaia_VoiceSource;

    // Protagonist: Reverie the The Unpredictable
    /* VOICE PROFILE: Low and Flat Monotone, Sarcastic, Cynical. */
    public GameObject Reverie_Character;
    public AudioSource Reverie_VoiceSource;

    // Antagonist: Kane the The Usurper
    /* VOICE PROFILE: Not available (Rage, Inner Conflict). */
    public GameObject Kane_Character;
    public AudioSource Kane_VoiceSource;

    [Header("UI Components")]
    public GameObject DialogueBox;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI DialogueText;

    #region Private Fields
    private Animator zaiaAnimator;
    private Animator reverieAnimator;
    private Animator kaneAnimator;
    #endregion

    #region Unity Event Methods
    private void Awake()
    {
        // Cache animator components
        if (Zaia_Character != null) { zaiaAnimator = Zaia_Character.GetComponent<Animator>(); }
        if (Reverie_Character != null) { reverieAnimator = Reverie_Character.GetComponent<Animator>(); }
        if (Kane_Character != null) { kaneAnimator = Kane_Character.GetComponent<Animator>(); }

        // Ensure Kane starts inactive until his dramatic entrance
        if (Kane_Character != null) { Kane_Character.SetActive(false); }
    }

    private void Start()
    {
        StartCoroutine(CinematicSequence());
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// The main coroutine that executes the cinematic sequence for the Nightmare Echo.
    /// </summary>
    private IEnumerator CinematicSequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras, set initial character positions]
        Debug.Log("Starting Nightmare Echo cinematic: Kane Confrontation.");
        DialogueBox.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // --- LINE 1: Reverie ---
        // [CAMERA: Establish a wide shot of the twisted cyber-natural landscape, then pan to Reverie.]
        // [ANIMATION: reverieAnimator.SetTrigger("Analyze_Surroundings");]
        yield return new WaitForSeconds(1.5f);
        SpeakerNameText.text = "Reverie";
        DialogueText.text = "This place is a scar on the consciousness. It is built from his jealousy and his rage, but the architecture... this is Lucent's design. Kane is merely the prisoner who thinks he is the warden.";
        // Reverie_VoiceSource.Play();
        yield return new WaitForSeconds(6.5f);

        // --- LINE 2: Zaia ---
        // [CAMERA: Cut to a medium close-up on Zaia, her expression firm and resolute.]
        // [ANIMATION: zaiaAnimator.SetTrigger("Steely_Gaze");]
        yield return new WaitForSeconds(1.0f);
        SpeakerNameText.text = "Zaia";
        DialogueText.text = "Prison or not, he is guilty. This gilded cage is built upon the bones of his father. We are not here to pity him, we are here to deliver judgment.";
        // Zaia_VoiceSource.Play();
        yield return new WaitForSeconds(6.0f);

        // --- ACTION: Kane makes his dramatic entrance ---
        // [SFX: Play a loud, echoing psychic boom.]
        // [VFX: Play a glitchy, cybernetic teleport effect at Kane's destination.]
        yield return new WaitForSeconds(1.0f);
        if (Kane_Character != null) { Kane_Character.SetActive(true); }
        // [ANIMATION: kaneAnimator.SetTrigger("Teleport_In_Aggressive");]
        yield return new WaitForSeconds(2.0f);

        // --- LINE 3: Kane ---
        // [CAMERA: Whip pan to a low-angle shot of Kane, making him look imposing and powerful.]
        // [ANIMATION: kaneAnimator.SetTrigger("Grand_Gesture_Rage");]
        yield return new WaitForSeconds(0.5f);
        SpeakerNameText.text = "Kane";
        DialogueText.text = "Judgment? Look around you, 'executor'! This is my birthright, perfected! I tore the world from the hands of a weak old man and made it strong. Aeron would have let it crumble!";
        // Kane_VoiceSource.Play();
        yield return new WaitForSeconds(7.5f);

        // --- LINE 4: Reverie ---
        // [CAMERA: Rack focus from the distant Kane to a sharp focus on Reverie's unimpressed face.]
        // [ANIMATION: reverieAnimator.SetTrigger("Dismissive_Headshake");]
        yield return new WaitForSeconds(1.2f);
        SpeakerNameText.text = "Reverie";
        DialogueText.text = "You are echoing words that are not your own. This strength you feel is a chain, and every link is a lie you were fed.";
        // Reverie_VoiceSource.Play();
        yield return new WaitForSeconds(5.5f);

        // --- LINE 5: Zaia ---
        // [CAMERA: Extreme close-up on Zaia's eyes, then pull back as she places a hand on her weapon's hilt.]
        // [ANIMATION: zaiaAnimator.SetTrigger("Prepare_For_Combat_Stance");]
        yield return new WaitForSeconds(1.8f);
        SpeakerNameText.text = "Zaia";
        DialogueText.text = "Lies do not absolve you of the truth of your actions. You murdered your own blood. This 'perfection' is a monument to your weakness, not your strength.";
        // Zaia_VoiceSource.Play();
        yield return new WaitForSeconds(7.0f);

        // --- LINE 6: Kane ---
        // [CAMERA: Zoom in on Kane's face with a shaky-cam effect, showing his rage and inner conflict.]
        // [ANIMATION: kaneAnimator.SetTrigger("Conflicted_Rage_ClutchHead");]
        yield return new WaitForSeconds(1.0f);
        SpeakerNameText.text = "Kane";
        DialogueText.text = "I AM a king! I earned this throne! It is mine! Not his... never his!";
        // Kane_VoiceSource.Play();
        yield return new WaitForSeconds(5.0f);

        // --- ACTION: Kane unleashes a wave of psychic energy, ending the conversation and beginning the encounter ---
        // [ANIMATION: kaneAnimator.SetTrigger("Unleash_Psychic_Blast");]
        // [VFX: Instantiate large area-of-effect psychic energy wave from Kane.]
        // [SFX: Play psychic energy explosion sound, followed by boss music.]
        yield return new WaitForSeconds(2.0f);

        DialogueBox.SetActive(false);
        // [SCENE CLEANUP: Re-enable player controls, reset cameras to gameplay mode, start boss AI]
        Debug.Log("Cinematic complete. Transitioning to gameplay.");
    }
    #endregion
}