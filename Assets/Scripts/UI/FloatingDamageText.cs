// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior of a floating damage text element.
/// It handles its own animation (floating up and fading out) and destruction.
/// </summary>
[RequireComponent(typeof(Text))]
public class FloatingDamageText : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How fast the text floats upwards.")]
    public float floatSpeed = 1.0f;
    [Tooltip("How long the text remains visible before fading.")]
    public float lifetime = 1.5f;

    private Text _damageText;
    private float _timer;
    private Color _originalColor;

    void Awake()
    {
        _damageText = GetComponent<Text>();
        _originalColor = _damageText.color;
        _timer = lifetime;
    }

    /// <summary>
    /// Initializes the damage text with a specific value.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to display.</param>
    public void SetText(float damageAmount)
    {
        _damageText.text = Mathf.RoundToInt(damageAmount).ToString();
    }

    void Update()
    {
        // Animate position
        transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);

        // Animate alpha fade
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            // Fade out in the last portion of its lifetime
            float alpha = Mathf.Clamp01(_timer / (lifetime * 0.5f));
            _damageText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
        }
    }
}