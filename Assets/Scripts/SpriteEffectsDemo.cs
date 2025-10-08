using UnityEngine;

/// <summary>
/// A test/demo script that demonstrates the usage of the SpriteEffects static utility class.
/// Attach this to a GameObject with a SpriteRenderer to see the flash and fade effects in action.
/// </summary>
public class SpriteEffectsDemo : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The SpriteRenderer to apply effects to. If null, will try to get from this GameObject.")]
    public SpriteRenderer targetSprite;

    [Header("Flash Settings")]
    [Tooltip("Press this key to trigger a flash effect.")]
    public KeyCode flashKey = KeyCode.F;
    [Tooltip("Duration of the flash effect in seconds.")]
    public float flashDuration = 0.2f;
    [Tooltip("Color to flash to.")]
    public Color flashColor = Color.white;

    [Header("Fade Settings")]
    [Tooltip("Press this key to trigger a fade out effect.")]
    public KeyCode fadeOutKey = KeyCode.O;
    [Tooltip("Press this key to trigger a fade in effect.")]
    public KeyCode fadeInKey = KeyCode.I;
    [Tooltip("Duration of the fade effect in seconds.")]
    public float fadeDuration = 1f;

    void Start()
    {
        // If no sprite renderer is assigned, try to get it from this GameObject
        if (targetSprite == null)
        {
            targetSprite = GetComponent<SpriteRenderer>();
            if (targetSprite == null)
            {
                Debug.LogError("SpriteEffectsDemo: No SpriteRenderer found! Please assign one or attach this script to a GameObject with a SpriteRenderer.");
            }
        }
    }

    void Update()
    {
        if (targetSprite == null) return;

        // Flash effect
        if (Input.GetKeyDown(flashKey))
        {
            Debug.Log("Triggering Flash effect");
            SpriteEffects.Flash(this, targetSprite, flashDuration, flashColor);
        }

        // Fade out effect
        if (Input.GetKeyDown(fadeOutKey))
        {
            Debug.Log("Triggering Fade Out effect");
            SpriteEffects.Fade(this, targetSprite, fadeDuration, 0f);
        }

        // Fade in effect
        if (Input.GetKeyDown(fadeInKey))
        {
            Debug.Log("Triggering Fade In effect");
            SpriteEffects.Fade(this, targetSprite, fadeDuration, 1f);
        }
    }
}
