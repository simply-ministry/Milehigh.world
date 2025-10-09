using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider and Text

/// <summary>
/// Updates the UI elements for a single character (health bar, name, etc.).
/// </summary>
public class CharacterUI : MonoBehaviour
{
    public Text characterNameText;
    public Slider healthSlider;

    private Character targetCharacter;

    /// <summary>
    /// Links this UI panel to a specific character.
    /// </summary>
    public void Initialize(Character character)
    {
        targetCharacter = character;
        characterNameText.text = character.characterName;
        UpdateHealth();
    }

    void Update()
    {
        // Continuously update health. In a real game, you might use events for better performance.
        if (targetCharacter != null)
        {
            UpdateHealth();
        }
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = targetCharacter.maxHealth;
        healthSlider.value = targetCharacter.health;
    }
}