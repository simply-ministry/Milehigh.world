using UnityEngine;

/// <summary>
/// A singleton manager responsible for creating and managing floating damage text instances.
/// It listens for damage events from all characters and spawns text prefabs at their positions.
/// </summary>
public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    [Header("Prefab and Settings")]
    [Tooltip("The prefab for the floating damage text.")]
    public FloatingDamageText damageTextPrefab;
    [Tooltip("The main canvas to spawn the UI text onto.")]
    public Canvas worldSpaceCanvas;
    [Tooltip("Vertical offset to apply to the spawn position.")]
    public float spawnOffset = 2.0f;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Subscribes to the OnDamageTaken event for all characters in the scene.
    /// </summary>
    void Start()
    {
        // Find all Character objects in the scene and subscribe to their OnDamageTaken event.
        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            // We need to capture the character instance to pass its position correctly.
            Character capturedCharacter = character;
            character.OnDamageTaken += (damage) => CreateDamageText(damage, capturedCharacter.transform.position);
        }
    }

    /// <summary>
    /// Creates an instance of the damage text prefab at a specified world position.
    /// </summary>
    /// <param name="damageAmount">The damage value to display.</param>
    /// <param name="worldPosition">The position in the world where the damage occurred.</param>
    public void CreateDamageText(float damageAmount, Vector3 worldPosition)
    {
        if (damageTextPrefab == null || worldSpaceCanvas == null)
        {
            Debug.LogError("DamageTextManager is missing required references (Prefab or Canvas).");
            return;
        }

        Vector3 spawnPosition = worldPosition + Vector3.up * spawnOffset;

        // Instantiate the text prefab as a child of the canvas
        FloatingDamageText textInstance = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, worldSpaceCanvas.transform);
        textInstance.SetText(damageAmount);
    }
}
