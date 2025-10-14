using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI elements for a single character, such as health and mana bars.
/// This component subscribes to the character's events to update the UI efficiently.
/// </summary>
public class CharacterUI : MonoBehaviour
{
    [Header("UI Element References")]
    [Tooltip("The Text component for displaying the character's name.")]
    public Text characterNameText;
    [Tooltip("The Slider component for the health bar.")]
    public Slider healthSlider;
    [Tooltip("The Slider component for the mana bar.")]
    public Slider manaSlider;

    private Character _targetCharacter;

    /// <summary>
    /// Initializes the UI with a target character, setting up initial values and subscribing to events.
    /// </summary>
    /// <param name="character">The character this UI will represent.</param>
    public void Initialize(Character character)
    {
        _targetCharacter = character;

        // Set initial values
        characterNameText.text = _targetCharacter.characterName;
        UpdateHealthUI(_targetCharacter.Health, _targetCharacter.maxHealth);
        UpdateManaUI(_targetCharacter.Mana, _targetCharacter.maxMana);

        // Subscribe to character events
        _targetCharacter.OnHealthChanged += UpdateHealthUI;
        _targetCharacter.OnManaChanged += UpdateManaUI;
    }

    /// <summary>
    /// Updates the health slider's values.
    /// </summary>
    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Visually indicate if the character is defeated
        if (currentHealth <= 0)
        {
            characterNameText.color = Color.gray;
        }
    }

    /// <summary>
    /// Updates the mana slider's values.
    /// </summary>
    private void UpdateManaUI(float currentMana, float maxMana)
    {
        if (manaSlider != null)
        {
            manaSlider.maxValue = maxMana;
            manaSlider.value = currentMana;
        }
    }

    /// <summary>
    /// Unsubscribe from events when the UI object is destroyed to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (_targetCharacter != null)
        {
            _targetCharacter.OnHealthChanged -= UpdateHealthUI;
            _targetCharacter.OnManaChanged -= UpdateManaUI;
        }
    }
}