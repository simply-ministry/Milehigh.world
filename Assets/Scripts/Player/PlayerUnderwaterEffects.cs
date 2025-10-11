using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

/// <summary>
/// Handles enabling/disabling underwater visual and audio effects.
/// Attach this to your player GameObject.
/// </summary>
public class PlayerUnderwaterEffects : MonoBehaviour
{
    [Header("Visual Effects")]
    public Volume underwaterPostProcessingVolume; // Assign a Post Processing Volume with underwater effects.
    public ParticleSystem bubbles; // Optional: Particle system for bubbles.

    [Header("Audio Effects")]
    public AudioMixerSnapshot aboveWaterSnapshot;
    public AudioMixerSnapshot underwaterSnapshot;
    public float transitionTime = 1f; // Smooth transition time.

    private bool isUnderwater = false;

    public void SetUnderwaterState(bool underwater)
    {
        if (isUnderwater == underwater) return;
        isUnderwater = underwater;

        // Visual Effects
        if (underwaterPostProcessingVolume != null)
            underwaterPostProcessingVolume.enabled = underwater;

        if (bubbles != null)
        {
            if (underwater)
                bubbles.Play();
            else
                bubbles.Stop();
        }

        // Audio Effects
        if (underwater)
            underwaterSnapshot?.TransitionTo(transitionTime);
        else
            aboveWaterSnapshot?.TransitionTo(transitionTime);
    }

    // Optional: For other scripts to check
    public bool IsUnderwater() => isUnderwater;
}