using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A persistent singleton that manages the overall game state,
/// including scene loading, pausing, and references to other key managers.
/// This object is configured to persist across scene loads.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the GameManager, providing a global access point.
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Represents the different high-level states the game can be in.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        InCutscene
    }

    [Header("Game State")]
    [SerializeField]
    [Tooltip("The current state of the game.")]
    private GameState currentState;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Implements the singleton pattern and ensures essential managers are present.
    /// </summary>
    private void Awake()
    {
        // --- Singleton Implementation ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this new instance if one already exists
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persist between scenes

        // --- Initial State ---
        // For development, we start directly in 'Playing' state.
        // In a final build, this would likely start at 'MainMenu'.
        currentState = GameState.Playing;

        // --- Manager Creation ---
        // Ensure that other core managers exist in the scene.
        // This is a robust way to handle manager dependencies.
        EnsureManagerExists<CombatManager>("_CombatManager");
        EnsureManagerExists<UIManager>("_UIManager");
    }

    /// <summary>
    /// Checks if a manager of a specific type exists in the scene. If not, it creates one.
    /// This ensures that manager singletons are always available.
    /// </summary>
    /// <typeparam name="T">The type of the manager component (must be a MonoBehaviour).</typeparam>
    /// <param name="name">The name for the new GameObject if one needs to be created.</param>
    private void EnsureManagerExists<T>(string name) where T : MonoBehaviour
    {
        if (FindObjectOfType<T>() == null)
        {
            GameObject managerGO = new GameObject(name);
            managerGO.AddComponent<T>();
            // Typically, managers created this way should also persist.
            DontDestroyOnLoad(managerGO);
            Debug.Log($"'{name}' was not found in the scene. A new instance has been created.");
        }
    }

    /// <summary>
    /// Changes the current state of the game and applies state-specific logic (e.g., pausing).
    /// </summary>
    /// <param name="newState">The state to switch to.</param>
    public void SetGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"Game state changed to: {newState}");

        // Handle state-specific logic
        switch (currentState)
        {
            case GameState.Paused:
                Time.timeScale = 0f; // Pause all physics and time-based operations
                break;
            case GameState.Playing:
                Time.timeScale = 1f; // Resume normal time
                break;
            case GameState.InCutscene:
                Time.timeScale = 1f; // Ensure cutscenes play at normal speed
                // In a full implementation, you might disable player input here.
                break;
        }
    }

    /// <summary>
    /// Gets the current state of the game.
    /// </summary>
    /// <returns>The current GameState enum value.</returns>
    public GameState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Loads a new scene asynchronously by its name.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load, as defined in Build Settings.</param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}