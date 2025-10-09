using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider and Text

/// <summary>
/// Updates the UI elements for a single character (health bar, name, etc.).
/// </summary>
public class CharacterUI : MonoBehaviour
{
    public Text characterNameText;
    public Slider healthSlider;
    public Slider manaSlider; // Added for mana

    private Character targetCharacter;

    /// <summary>
    /// Links this UI panel to a specific character.
    /// </summary>
    public void Initialize(Character character)
    {
        targetCharacter = character;
        characterNameText.text = character.characterName;
        UpdateUI();
    }

    void Update()
    {
        // Continuously update stats. In a real game, you might use events for better performance.
        if (targetCharacter != null && targetCharacter.isAlive)
        {
            UpdateUI();
        }
        else if (targetCharacter != null && !targetCharacter.isAlive)
        {
            // Optional: Visually show the character is defeated
            characterNameText.color = Color.gray;
        }
    }

    private void UpdateUI()
    {
        healthSlider.maxValue = targetCharacter.maxHealth;
        healthSlider.value = targetCharacter.health;

        if (manaSlider != null)
        {
            manaSlider.maxValue = targetCharacter.maxMana;
            manaSlider.value = targetCharacter.mana;
        }
    }
}