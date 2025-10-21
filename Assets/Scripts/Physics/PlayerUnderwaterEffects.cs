// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

/// <summary>
/// Manages the visual and audio effects for a player character when they are underwater.
/// This script should be attached to the player GameObject.
/// </summary>
public class PlayerUnderwaterEffects : MonoBehaviour
{
    [Header("Post-Processing")]
    [Tooltip("The post-processing volume to enable when the player is underwater.")]
    public Volume underwaterPostProcessingVolume;

    [Header("Audio")]
    [Tooltip("The AudioMixer snapshot to transition to when underwater.")]
    public AudioMixerSnapshot underwaterSnapshot;
    [Tooltip("The AudioMixer snapshot to transition to when above water.")]
    public AudioMixerSnapshot aboveWaterSnapshot;
    [Tooltip("The time it takes to transition between audio snapshots.")]
    public float audioTransitionTime = 0.5f;

    [Header("Particles")]
    [Tooltip("The particle system for bubbles, activated when underwater.")]
    public ParticleSystem bubbleParticleSystem;

    private void Start()
    {
        // Ensure all effects are initially in the "above water" state.
        if (underwaterPostProcessingVolume != null)
        {
            underwaterPostProcessingVolume.enabled = false;
        }
        if (bubbleParticleSystem != null)
        {
            bubbleParticleSystem.Stop();
        }
        if (aboveWaterSnapshot != null)
        {
            aboveWaterSnapshot.TransitionTo(0f);
        }
    }

    /// <summary>
    /// Activates all underwater effects.
    /// </summary>
    public void EnterWater()
    {
        if (underwaterPostProcessingVolume != null)
        {
            underwaterPostProcessingVolume.enabled = true;
        }

        if (underwaterSnapshot != null)
        {
            underwaterSnapshot.TransitionTo(audioTransitionTime);
        }

        if (bubbleParticleSystem != null)
        {
            bubbleParticleSystem.Play();
        }
    }

    /// <summary>
    /// Deactivates all underwater effects and returns to the default state.
    /// </summary>
    public void ExitWater()
    {
        if (underwaterPostProcessingVolume != null)
        {
            underwaterPostProcessingVolume.enabled = false;
        }

        if (aboveWaterSnapshot != null)
        {
            aboveWaterSnapshot.TransitionTo(audioTransitionTime);
        }

        if (bubbleParticleSystem != null)
        {
            bubbleParticleSystem.Stop();
        }
    }
}