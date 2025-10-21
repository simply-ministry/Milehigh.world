// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections;

/// <summary>
/// A static utility class for performing common visual manipulations on SpriteRenderers.
/// This includes effects like flashing, fading, and changing colors.
/// Because it's a static class, its methods can be called from anywhere
/// without needing an instance of the class.
/// Example Usage: SpriteEffects.Flash(this, mySpriteRenderer, 0.5f, Color.white);
/// </summary>
public static class SpriteEffects
{
    /// <summary>
    /// Makes a sprite flash a specific color for a given duration.
    /// Useful for damage indicators or highlighting objects.
    /// </summary>
    /// <param name="owner">The MonoBehaviour script that will own and run the coroutine (e.g., 'this').</param>
    /// <param name="spriteRenderer">The SpriteRenderer to manipulate.</param>
    /// <param name="duration">The total duration of the flash effect.</param>
    /// <param name="flashColor">The color the sprite will flash.</param>
    public static void Flash(MonoBehaviour owner, SpriteRenderer spriteRenderer, float duration, Color flashColor)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is null. Cannot perform flash effect.");
            return;
        }
        // Start the coroutine on the provided MonoBehaviour instance.
        owner.StartCoroutine(FlashRoutine(spriteRenderer, duration, flashColor));
    }

    /// <summary>
    /// The coroutine that handles the logic for the flash effect.
    /// </summary>
    private static IEnumerator FlashRoutine(SpriteRenderer spriteRenderer, float duration, Color flashColor)
    {
        // Store the original color to revert back to it later.
        Color originalColor = spriteRenderer.color;

        // Set the sprite to the flash color.
        spriteRenderer.color = flashColor;

        // Wait for the specified duration.
        yield return new WaitForSeconds(duration);

        // Revert the sprite back to its original color.
        // Check if the spriteRenderer still exists in case the object was destroyed during the flash.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    /// <summary>
    /// Fades a sprite's alpha to a target value over a specified duration.
    /// </summary>
    /// <param name="owner">The MonoBehaviour script that will own and run the coroutine (e.g., 'this').</param>
    /// <param name="spriteRenderer">The SpriteRenderer to manipulate.</param>
    /// <param name="duration">How long the fade should take in seconds.</param>
    /// <param name="targetAlpha">The target alpha value (0 for fully transparent, 1 for fully opaque).</param>
    public static void Fade(MonoBehaviour owner, SpriteRenderer spriteRenderer, float duration, float targetAlpha)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is null. Cannot perform fade effect.");
            return;
        }
        owner.StartCoroutine(FadeRoutine(spriteRenderer, duration, targetAlpha));
    }

    /// <summary>
    /// The coroutine that handles the logic for the fade effect.
    /// </summary>
    private static IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float duration, float targetAlpha)
    {
        Color startColor = spriteRenderer.color;
        float startAlpha = startColor.a;
        float elapsedTime = 0f;

        // Fade the sprite over the specified duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            // Calculate the interpolation factor (0 to 1)
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            // Interpolate the alpha value
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            // Apply the new alpha to the sprite color
            Color newColor = spriteRenderer.color;
            newColor.a = currentAlpha;
            spriteRenderer.color = newColor;
            
            yield return null;
        }

        // Ensure we end at exactly the target alpha
        if (spriteRenderer != null)
        {
            Color finalColor = spriteRenderer.color;
            finalColor.a = targetAlpha;
            spriteRenderer.color = finalColor;
        }
    }
}
