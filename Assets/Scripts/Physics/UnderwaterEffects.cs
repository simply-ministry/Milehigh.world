using UnityEngine;

/// <summary>
/// Manages the visual and audio effects when a GameObject enters or exits a trigger volume tagged as "Water".
/// This script controls post-processing effects, ambient lighting, and underwater audio.
/// </summary>
public class UnderwaterEffects : MonoBehaviour
{
    /// <summary>
    /// The GameObject containing the underwater post-processing volume.
    /// </summary>
    public GameObject underwaterPostProcess;
    /// <summary>
    /// The AudioSource for the underwater ambient sounds.
    /// </summary>
    public AudioSource underwaterAudio;
    /// <summary>
    /// The ambient color to be applied to the scene when underwater.
    /// </summary>
    public Color underwaterAmbientColor = new Color(0.0f, 0.2f, 0.35f);

    /// <summary>
    /// Stores the scene's default ambient color to restore it upon exiting the water.
    /// </summary>
    private Color defaultAmbientColor;

    /// <summary>
    /// Initializes the component by storing the default ambient color and ensuring underwater effects are disabled.
    /// </summary>
    void Start()
    {
        defaultAmbientColor = RenderSettings.ambientLight;
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(false);
        if (underwaterAudio != null)
            underwaterAudio.Stop();
    }

    /// <summary>
    /// Called when the GameObject enters a trigger collider.
    /// Checks if the trigger is tagged "Water" and enables underwater effects if it is.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            EnableUnderwaterEffects();
    }

    /// <summary>
    /// Called when the GameObject exits a trigger collider.
    /// Checks if the trigger is tagged "Water" and disables underwater effects if it is.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            DisableUnderwaterEffects();
    }

    /// <summary>
    /// Enables the underwater post-processing, plays the underwater audio, and sets the ambient light color.
    /// </summary>
    void EnableUnderwaterEffects()
    {
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(true);
        if (underwaterAudio != null)
            underwaterAudio.Play();
        RenderSettings.ambientLight = underwaterAmbientColor;
    }

    /// <summary>
    /// Disables the underwater post-processing, stops the underwater audio, and restores the default ambient light color.
    /// </summary>
    void DisableUnderwaterEffects()
    {
        if (underwaterPostProcess != null)
            underwaterPostProcess.SetActive(false);
        if (underwaterAudio != null)
            underwaterAudio.Stop();
        RenderSettings.ambientLight = defaultAmbientColor;
    }
}